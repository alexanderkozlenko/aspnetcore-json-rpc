// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Anemonis.AspNetCore.JsonRpc.Resources;
using Anemonis.JsonRpc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Anemonis.AspNetCore.JsonRpc
{
    /// <summary>Represents a middleware for adding a JSON-RPC handler to the application's request pipeline.</summary>
    /// <typeparam name="T">The type of the handler.</typeparam>
    public sealed class JsonRpcMiddleware<T> : IMiddleware, IDisposable
        where T : class, IJsonRpcHandler
    {
        private const int _streamBufferSize = 1024;

        private static readonly string _contentTypeHeaderValue = $"{MediaTypes.ApplicationJson}; charset=utf-8";
        private static readonly Dictionary<string, Encoding> _supportedEncodings;
        private static readonly Dictionary<long, JsonRpcError> _standardJsonRpcErrors;
        private static readonly Dictionary<long, JsonRpcResponse> _standardJsonRpcResponses;

        private readonly IHostingEnvironment _environment;
        private readonly ILogger _logger;
        private readonly T _handler;
        private readonly JsonRpcSerializer _serializer;

        static JsonRpcMiddleware()
        {
            _supportedEncodings = CreateSupportedEncodings();
            _standardJsonRpcErrors = CreateStandardJsonRpcErrors();
            _standardJsonRpcResponses = CreateStandardJsonRpcResponses(_standardJsonRpcErrors);
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcMiddleware{T}" /> class.</summary>
        /// <param name="services">The <see cref="IServiceProvider" /> instance for retrieving service objects.</param>
        /// <param name="environment">The <see cref="IHostingEnvironment" /> instance for retrieving hosting environment information.</param>
        /// <param name="logger">The <see cref="ILogger{T}" /> instance for logging.</param>
        /// <exception cref="ArgumentNullException"><paramref name="services" />, <paramref name="environment" />, or <paramref name="logger" /> is <see langword="null" />.</exception>
        public JsonRpcMiddleware(IServiceProvider services, IHostingEnvironment environment, ILogger<JsonRpcMiddleware<T>> logger)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _environment = environment;
            _logger = logger;
            _handler = services.GetService<T>() ?? ActivatorUtilities.CreateInstance<T>(services);
            _serializer = new JsonRpcSerializer(CreateJsonRpcContractResolver(_handler), services.GetService<IOptions<JsonRpcOptions>>()?.Value?.JsonSerializer);
        }

        private static Dictionary<string, Encoding> CreateSupportedEncodings()
        {
            return new Dictionary<string, Encoding>(StringComparer.OrdinalIgnoreCase)
            {
                [Encoding.UTF8.WebName] = new UTF8Encoding(false, true),
                [Encoding.Unicode.WebName] = new UnicodeEncoding(false, false, true),
                [Encoding.UTF32.WebName] = new UTF32Encoding(false, false, true)
            };
        }

        private static Dictionary<long, JsonRpcError> CreateStandardJsonRpcErrors()
        {
            return new Dictionary<long, JsonRpcError>
            {
                [JsonRpcErrorCode.InvalidFormat] =
                    new JsonRpcError(JsonRpcErrorCode.InvalidFormat, Strings.GetString("rpc.error.invalid_format")),
                [JsonRpcErrorCode.InvalidOperation] =
                    new JsonRpcError(JsonRpcErrorCode.InvalidOperation, Strings.GetString("rpc.error.invalid_operation")),
                [JsonRpcErrorCode.InvalidParameters] =
                    new JsonRpcError(JsonRpcErrorCode.InvalidParameters, Strings.GetString("rpc.error.invalid_parameters")),
                [JsonRpcErrorCode.InvalidMethod] =
                    new JsonRpcError(JsonRpcErrorCode.InvalidMethod, Strings.GetString("rpc.error.invalid_method")),
                [JsonRpcErrorCode.InvalidMessage] =
                    new JsonRpcError(JsonRpcErrorCode.InvalidMessage, Strings.GetString("rpc.error.invalid_message")),
                [JsonRpcHandlerErrorCode.BatchHasDuplicateIdentifiers] =
                    new JsonRpcError(JsonRpcHandlerErrorCode.BatchHasDuplicateIdentifiers, Strings.GetString("rpc.error.batch_has_duplicate_identifiers"))
            };
        }

        private static Dictionary<long, JsonRpcResponse> CreateStandardJsonRpcResponses(IReadOnlyDictionary<long, JsonRpcError> standardJsonRpcErrors)
        {
            return new Dictionary<long, JsonRpcResponse>
            {
                [JsonRpcErrorCode.InvalidFormat] =
                    new JsonRpcResponse(default, standardJsonRpcErrors[JsonRpcErrorCode.InvalidFormat]),
                [JsonRpcErrorCode.InvalidOperation] =
                    new JsonRpcResponse(default, standardJsonRpcErrors[JsonRpcErrorCode.InvalidOperation]),
                [JsonRpcHandlerErrorCode.BatchHasDuplicateIdentifiers] =
                    new JsonRpcResponse(default, standardJsonRpcErrors[JsonRpcHandlerErrorCode.BatchHasDuplicateIdentifiers])
            };
        }

        private static bool IsReservedErrorCode(long code)
        {
            return
                (code == JsonRpcErrorCode.InvalidFormat) ||
                (code == JsonRpcErrorCode.InvalidMethod) ||
                (code == JsonRpcErrorCode.InvalidMessage) ||
                (code == JsonRpcHandlerErrorCode.BatchHasDuplicateIdentifiers);
        }

        private static JsonRpcContractResolver CreateJsonRpcContractResolver(T handler)
        {
            var contracts = handler.GetContracts();
            var resolver = new JsonRpcContractResolver();

            foreach (var kvp in contracts)
            {
                if (kvp.Key == null)
                {
                    throw new InvalidOperationException(Strings.GetString("handler.contract.method_name.invalid_value"));
                }
                if (JsonRpcProtocol.IsSystemMethod(kvp.Key))
                {
                    throw new InvalidOperationException(string.Format(Strings.GetString("handler.contract.method_name.reserved_prefix"), kvp.Key));
                }

                resolver.AddRequestContract(kvp.Key, kvp.Value);
            }

            return resolver;
        }

        /// <summary>Handles an HTTP request as an asynchronous operation.</summary>
        /// <param name="context">The <see cref="HttpContext" /> instance for the current request.</param>
        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!string.Equals(context.Request.Method, HttpMethods.Post, StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;

                return;
            }
            if (context.Request.Headers.ContainsKey(HeaderNames.ContentEncoding))
            {
                context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;

                return;
            }

            var requestStreamEncoding = default(Encoding);
            var responseStreamEncoding = default(Encoding);

            if (!context.Request.Headers.TryGetValue(HeaderNames.ContentType, out var contentTypeHeaderValueString))
            {
                context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;

                return;
            }
            if (!MediaTypeHeaderValue.TryParse((string)contentTypeHeaderValueString, out var contentTypeHeaderValue))
            {
                context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;

                return;
            }
            if (!contentTypeHeaderValue.MediaType.Equals(MediaTypes.ApplicationJson, StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;

                return;
            }
            if (contentTypeHeaderValue.Charset.HasValue && !_supportedEncodings.TryGetValue(contentTypeHeaderValue.Charset.Value, out requestStreamEncoding))
            {
                context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;

                return;
            }
            if (!context.Request.Headers.TryGetValue(HeaderNames.Accept, out var acceptTypeHeaderValueString))
            {
                context.Response.StatusCode = StatusCodes.Status406NotAcceptable;

                return;
            }
            if (!MediaTypeHeaderValue.TryParse((string)acceptTypeHeaderValueString, out var acceptTypeHeaderValue))
            {
                context.Response.StatusCode = StatusCodes.Status406NotAcceptable;

                return;
            }
            if (!acceptTypeHeaderValue.MediaType.Equals(MediaTypes.ApplicationJson, StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status406NotAcceptable;

                return;
            }
            if (acceptTypeHeaderValue.Charset.HasValue && !_supportedEncodings.TryGetValue(acceptTypeHeaderValue.Charset.Value, out responseStreamEncoding))
            {
                context.Response.StatusCode = StatusCodes.Status406NotAcceptable;

                return;
            }

            if (requestStreamEncoding == null)
            {
                requestStreamEncoding = _supportedEncodings[Encoding.UTF8.WebName];
            }
            if (responseStreamEncoding == null)
            {
                responseStreamEncoding = _supportedEncodings[Encoding.UTF8.WebName];
            }

            var jsonRpcRequestData = default(JsonRpcData<JsonRpcRequest>);

            try
            {
                using (var streamReader = new StreamReader(context.Request.Body, requestStreamEncoding, false, _streamBufferSize, true))
                {
                    jsonRpcRequestData = await _serializer.DeserializeRequestDataAsync(streamReader, context.RequestAborted);
                }
            }
            catch (JsonException e)
            {
                _logger.LogDataIsInvalid(e);

                var jsonRpcResponse = _standardJsonRpcResponses[JsonRpcErrorCode.InvalidFormat];

                if (_environment.IsDevelopment())
                {
                    jsonRpcResponse = new JsonRpcResponse(default, new JsonRpcError(jsonRpcResponse.Error.Code, jsonRpcResponse.Error.Message, e.ToString()));
                }

                context.Response.StatusCode = StatusCodes.Status200OK;

                await SerializeJsonRpcResponseAsync(context, jsonRpcResponse, responseStreamEncoding);

                return;
            }
            catch (JsonRpcSerializationException e)
            {
                _logger.LogDataIsInvalid(e);

                var jsonRpcResponse = _standardJsonRpcResponses[JsonRpcErrorCode.InvalidOperation];

                if (_environment.IsDevelopment())
                {
                    jsonRpcResponse = new JsonRpcResponse(default, new JsonRpcError(jsonRpcResponse.Error.Code, jsonRpcResponse.Error.Message, e.ToString()));
                }

                context.Response.StatusCode = StatusCodes.Status200OK;

                await SerializeJsonRpcResponseAsync(context, jsonRpcResponse, responseStreamEncoding);

                return;
            }

            if (!jsonRpcRequestData.IsBatch)
            {
                var jsonRpcRequestItem = jsonRpcRequestData.Item;

                _logger.LogDataIsMessage();

                context.RequestAborted.ThrowIfCancellationRequested();

                var jsonRpcResponse = await CreateJsonRpcResponseAsync(jsonRpcRequestItem, 0);

                if (jsonRpcResponse == null)
                {
                    context.Response.StatusCode = StatusCodes.Status204NoContent;

                    return;
                }
                if (jsonRpcRequestItem.IsValid && jsonRpcRequestItem.Message.IsNotification)
                {
                    context.Response.StatusCode = StatusCodes.Status204NoContent;

                    return;
                }

                context.Response.StatusCode = StatusCodes.Status200OK;

                await SerializeJsonRpcResponseAsync(context, jsonRpcResponse, responseStreamEncoding);
            }
            else
            {
                var jsonRpcRequestItems = jsonRpcRequestData.Items;

                _logger.LogDataIsBatch(jsonRpcRequestItems.Count);

                var jsonRpcRequestIdentifiers = new HashSet<JsonRpcId>();

                for (var i = 0; i < jsonRpcRequestItems.Count; i++)
                {
                    var jsonRpcRequestItem = jsonRpcRequestItems[i];

                    if (jsonRpcRequestItem.IsValid && !jsonRpcRequestItem.Message.IsNotification)
                    {
                        if (!jsonRpcRequestIdentifiers.Add(jsonRpcRequestItem.Message.Id))
                        {
                            _logger.LogBatchHasDuplicateIdentifiers();

                            var jsonRpcResponse = _standardJsonRpcResponses[JsonRpcHandlerErrorCode.BatchHasDuplicateIdentifiers];

                            context.Response.StatusCode = StatusCodes.Status200OK;

                            await SerializeJsonRpcResponseAsync(context, jsonRpcResponse, responseStreamEncoding);

                            return;
                        }
                    }
                }

                var jsonRpcResponses = new List<JsonRpcResponse>(jsonRpcRequestItems.Count);

                for (var i = 0; i < jsonRpcRequestItems.Count; i++)
                {
                    context.RequestAborted.ThrowIfCancellationRequested();

                    var jsonRpcRequestItem = jsonRpcRequestItems[i];
                    var jsonRpcResponse = await CreateJsonRpcResponseAsync(jsonRpcRequestItem, i);

                    if (jsonRpcResponse != null)
                    {
                        if (jsonRpcRequestItem.IsValid && jsonRpcRequestItem.Message.IsNotification)
                        {
                            continue;
                        }

                        jsonRpcResponses.Add(jsonRpcResponse);
                    }
                }

                if (jsonRpcResponses.Count == 0)
                {
                    // Server must return empty content for empty response batch according to the JSON-RPC 2.0 specification

                    context.Response.StatusCode = StatusCodes.Status204NoContent;

                    return;
                }

                context.Response.StatusCode = StatusCodes.Status200OK;

                await SerializeJsonRpcResponsesAsync(context, jsonRpcResponses, responseStreamEncoding);
            }
        }

        private async Task SerializeJsonRpcResponseAsync(HttpContext context, JsonRpcResponse jsonRpcResponse, Encoding encoding)
        {
            context.Response.ContentType = _contentTypeHeaderValue;

            using (var responseStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(responseStream, encoding, _streamBufferSize, true))
                {
                    await _serializer.SerializeResponseAsync(jsonRpcResponse, streamWriter, context.RequestAborted);
                }

                context.Response.ContentLength = responseStream.Length;
                responseStream.Position = 0;

                await responseStream.CopyToAsync(context.Response.Body);
            }
        }

        private async Task SerializeJsonRpcResponsesAsync(HttpContext context, IReadOnlyList<JsonRpcResponse> jsonRpcResponses, Encoding encoding)
        {
            context.Response.ContentType = _contentTypeHeaderValue;

            using (var responseStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(responseStream, encoding, _streamBufferSize, true))
                {
                    await _serializer.SerializeResponsesAsync(jsonRpcResponses, streamWriter, context.RequestAborted);
                }

                context.Response.ContentLength = responseStream.Length;
                responseStream.Position = 0;

                await responseStream.CopyToAsync(context.Response.Body);
            }
        }

        private async Task<JsonRpcResponse> CreateJsonRpcResponseAsync(JsonRpcMessageInfo<JsonRpcRequest> requestInfo, int index)
        {
            if (!requestInfo.IsValid)
            {
                var exception = requestInfo.Exception;

                _logger.LogRequestIsInvalid(index, exception);

                var jsonRpcError = _standardJsonRpcErrors[exception.ErrorCode];

                if (_environment.IsDevelopment())
                {
                    jsonRpcError = new JsonRpcError(jsonRpcError.Code, jsonRpcError.Message, exception.ToString());
                }

                return new JsonRpcResponse(exception.MessageId, jsonRpcError);
            }

            var request = requestInfo.Message;
            var requestId = request.Id;
            var response = await _handler.HandleAsync(request);

            if (response != null)
            {
                var responseId = response.Id;

                if (requestId != responseId)
                {
                    throw new InvalidOperationException(Strings.GetString("handler.invalid_response_id"));
                }

                if (!response.Success)
                {
                    if (IsReservedErrorCode(response.Error.Code))
                    {
                        throw new InvalidOperationException(Strings.GetString("handler.invalid_error_code"));
                    }
                }

                if (!request.IsNotification)
                {
                    if (response.Success)
                    {
                        _logger.LogHandledRequestWithResultSuccessfully(index);
                    }
                    else
                    {
                        _logger.LogHandledRequestWithErrorSuccessfully(response.Error.Code, index);
                    }
                }
                else
                {
                    if (response.Success)
                    {
                        _logger.LogHandledRequestWithResultAsNotification(index);
                    }
                    else
                    {
                        _logger.LogHandledRequestWithErrorAsNotification(response.Error.Code, index);
                    }
                }
            }
            else
            {
                if (request.IsNotification)
                {
                    _logger.LogHandledNotificationSuccessfully(index);
                }
                else
                {
                    _logger.LogHandledNotificationAsRequest(index);
                }
            }

            return response;
        }

        /// <summary>Disposes the corresponding instance of a JSON-RPC handler.</summary>
        public void Dispose()
        {
            (_handler as IDisposable)?.Dispose();
        }
    }
}