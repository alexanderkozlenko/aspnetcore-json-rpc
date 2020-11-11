// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

using Anemonis.AspNetCore.JsonRpc.Resources;
using Anemonis.JsonRpc;

using Microsoft.Extensions.DependencyInjection;

namespace Anemonis.AspNetCore.JsonRpc
{
    /// <summary>Represents a JSON-RPC handler for a JSON-RPC service.</summary>
    /// <typeparam name="T">The type of the service.</typeparam>
    public sealed class JsonRpcServiceHandler<T> : IJsonRpcHandler, IDisposable
        where T : class, IJsonRpcService
    {
        private static readonly Dictionary<string, JsonRpcMethodInfo> s_metadata;
        private static readonly Dictionary<string, JsonRpcRequestContract> s_contracts;

        private readonly T _service;

        static JsonRpcServiceHandler()
        {
            var blueprint = new Dictionary<string, (JsonRpcRequestContract Contract, JsonRpcMethodInfo MethodInfo)>(StringComparer.Ordinal);

            FindContracts(blueprint, typeof(T));

            var metadata = new Dictionary<string, JsonRpcMethodInfo>(blueprint.Count, StringComparer.Ordinal);
            var contracts = new Dictionary<string, JsonRpcRequestContract>(blueprint.Count, StringComparer.Ordinal);

            foreach (var kvp in blueprint)
            {
                metadata[kvp.Key] = kvp.Value.MethodInfo;
                contracts[kvp.Key] = kvp.Value.Contract;
            }

            s_metadata = metadata;
            s_contracts = contracts;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcServiceHandler{T}" /> class.</summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider" /> instance for retrieving service objects.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serviceProvider" /> is <see langword="null" />.</exception>
        public JsonRpcServiceHandler(IServiceProvider serviceProvider)
        {
            if (serviceProvider is null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _service = serviceProvider.GetService<T>() ?? ActivatorUtilities.CreateInstance<T>(serviceProvider);
        }

        private static void FindContracts(Dictionary<string, (JsonRpcRequestContract, JsonRpcMethodInfo)> blueprint, Type type)
        {
            if (type is null)
            {
                return;
            }

            FindContracts(blueprint, type.GetMethods(BindingFlags.Instance | BindingFlags.Public));
            FindContracts(blueprint, type.BaseType);

            var interfaceTypes = type.GetInterfaces();

            for (var i = 0; i < interfaceTypes.Length; i++)
            {
                FindContracts(blueprint, interfaceTypes[i]);
            }
        }

        private static void FindContracts(Dictionary<string, (JsonRpcRequestContract, JsonRpcMethodInfo)> blueprint, IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<JsonRpcMethodAttribute>();

                if (attribute is null)
                {
                    continue;
                }
                if ((method.ReturnType != typeof(Task)) && !(method.ReturnType.IsGenericType && (method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.GetString("service.method.invalid_type"), method.Name, typeof(T)));
                }
                if (blueprint.ContainsKey(attribute.MethodName))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.GetString("service.method.invalid_name"), typeof(T), attribute.MethodName));
                }

                var contract = default(JsonRpcRequestContract);
                var methodInfo = default(JsonRpcMethodInfo);
                var parameters = method.GetParameters();

                switch (attribute.ParametersType)
                {
                    case JsonRpcParametersType.ByPosition:
                        {
                            var parameterPositions = attribute.ParameterPositions;

                            if (parameterPositions.Length != parameters.Length)
                            {
                                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.GetString("service.method.invalid_parameters_count"), method.Name, typeof(T)));
                            }

                            for (var i = 0; i < parameterPositions.Length; i++)
                            {
                                if (!parameterPositions.Contains(i))
                                {
                                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.GetString("service.method.invalid_parameter_positions"), method.Name, typeof(T)));
                                }
                            }

                            var contractParameters = new Type[parameters.Length];

                            for (var i = 0; i < parameters.Length; i++)
                            {
                                contractParameters[parameterPositions[i]] = parameters[i].ParameterType;
                            }

                            contract = new(contractParameters);
                            methodInfo = new(method, parameterPositions);
                        }
                        break;
                    case JsonRpcParametersType.ByName:
                        {
                            var parameterNames = attribute.ParameterNames;

                            if (parameterNames.Length != parameters.Length)
                            {
                                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.GetString("service.method.invalid_parameters_count"), method.Name, typeof(T)));
                            }
                            if (parameterNames.Length != parameterNames.Distinct(StringComparer.Ordinal).Count())
                            {
                                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.GetString("service.method.invalid_parameter_names"), method.Name, typeof(T)));
                            }

                            var contractParameters = new Dictionary<string, Type>(parameters.Length, StringComparer.Ordinal);
                            var methodParameterNames = new string[parameters.Length];

                            for (var i = 0; i < parameters.Length; i++)
                            {
                                contractParameters[parameterNames[i]] = parameters[i].ParameterType;
                                methodParameterNames[i] = parameterNames[i];
                            }

                            contract = new(contractParameters);
                            methodInfo = new(method, methodParameterNames);
                        }
                        break;
                    default:
                        {
                            if (parameters.Length != 0)
                            {
                                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.GetString("service.method.invalid_parameters_count"), method.Name, typeof(T)));
                            }

                            contract = new();
                            methodInfo = new(method);
                        }
                        break;
                }

                blueprint[attribute.MethodName] = (contract, methodInfo);
            }
        }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, JsonRpcRequestContract> GetContracts()
        {
            return s_contracts;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is <see langword="null" />.</exception>
        public async Task<JsonRpcResponse> HandleAsync(JsonRpcRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var requestId = request.Id;
            var methodInfo = s_metadata[request.Method];
            var method = methodInfo.Method;
            var parameters = method.GetParameters();
            var parameterValues = default(object[]);

            switch (request.ParametersType)
            {
                case JsonRpcParametersType.ByPosition:
                    {
                        parameterValues = new object[parameters.Length];

                        for (var i = 0; i < parameterValues.Length; i++)
                        {
                            parameterValues[methodInfo.ParameterPositions[i]] = request.ParametersByPosition[i];
                        }
                    }
                    break;
                case JsonRpcParametersType.ByName:
                    {
                        parameterValues = new object[parameters.Length];

                        for (var i = 0; i < parameterValues.Length; i++)
                        {
                            if (!request.ParametersByName.TryGetValue(methodInfo.ParameterNames[i], out parameterValues[i]))
                            {
                                if (parameters[i].HasDefaultValue)
                                {
                                    parameterValues[i] = parameters[i].DefaultValue;
                                }
                                else
                                {
                                    var message = string.Format(CultureInfo.CurrentCulture, Strings.GetString("service.request.parameter.undefined_value"), request.Method, methodInfo.ParameterNames[i]);

                                    return new(requestId, new(JsonRpcErrorCode.InvalidParameters, message));
                                }
                            }
                        }
                    }
                    break;
            }

            try
            {
                if (request.IsNotification || !method.ReturnType.IsGenericType)
                {
                    await (dynamic)method.Invoke(_service, parameterValues);
                }
                else
                {
                    var jsonRpcResult = await (dynamic)method.Invoke(_service, parameterValues) as object;

                    return new(requestId, jsonRpcResult);
                }
            }
            catch (JsonRpcServiceException e)
            {
                var responseError = default(JsonRpcError);

                if (e.HasErrorData)
                {
                    responseError = new(e.Code, e.Message, e.ErrorData);
                }
                else
                {
                    responseError = new(e.Code, e.Message);
                }

                return new(requestId, responseError);
            }
            catch (TargetInvocationException e)
                when (e.InnerException is JsonRpcServiceException jrse)
            {
                var responseError = default(JsonRpcError);

                if (jrse.HasErrorData)
                {
                    responseError = new(jrse.Code, jrse.Message, jrse.ErrorData);
                }
                else
                {
                    responseError = new(jrse.Code, jrse.Message);
                }

                return new(requestId, responseError);
            }
            catch (TargetInvocationException e)
                when (e.InnerException is not null)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
            }

            return null;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            (_service as IDisposable)?.Dispose();
        }
    }
}
