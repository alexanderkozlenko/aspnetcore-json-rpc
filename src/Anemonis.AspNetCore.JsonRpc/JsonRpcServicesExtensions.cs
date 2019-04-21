// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Reflection;

using Anemonis.AspNetCore.JsonRpc;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>The JSON-RPC 2.0 middleware extensions for the <see cref="IServiceCollection" />.</summary>
    public static class JsonRpcServicesExtensions
    {
        /// <summary>Adds the specified JSON-RPC 2.0 handler to the current <see cref="IServiceCollection" /> instance.</summary>
        /// <param name="services">The <see cref="IServiceCollection" /> instance to add the handler to.</param>
        /// <param name="type">The type of the handler.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentException"><paramref name="type" /> is not class or does not implement the <see cref="IJsonRpcHandler" /> interface.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="services" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        public static IServiceCollection AddJsonRpcHandler(this IServiceCollection services, Type type)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            InterfaceAssistant<IJsonRpcHandler>.VerifyTypeParam(type, nameof(type));

            var middlewareType = typeof(JsonRpcMiddleware<>).MakeGenericType(type);

            services.TryAddScoped(middlewareType, middlewareType);

            return services;
        }

        /// <summary>Adds the specified JSON-RPC 2.0 handler to the current <see cref="IServiceCollection" /> instance.</summary>
        /// <typeparam name="T">The type of the handler.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> instance to add the handler to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services" /> is <see langword="null" />.</exception>
        public static IServiceCollection AddJsonRpcHandler<T>(this IServiceCollection services)
            where T : class, IJsonRpcHandler
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddScoped<JsonRpcMiddleware<T>, JsonRpcMiddleware<T>>();

            return services;
        }

        /// <summary>Adds JSON-RPC 2.0 handlers from the current application domain to the current <see cref="IServiceCollection" /> instance.</summary>
        /// <param name="services">The <see cref="IServiceCollection" /> instance to add the handler to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services" /> is <see langword="null" />.</exception>
        public static IServiceCollection AddJsonRpcHandlers(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var types = InterfaceAssistant<IJsonRpcHandler>.GetDefinedTypes();

            for (var i = 0; i < types.Count; i++)
            {
                var jsonRpcRouteAtribute = types[i].GetCustomAttribute<JsonRpcRouteAttribute>();

                if (jsonRpcRouteAtribute == null)
                {
                    continue;
                }

                var middlewareType = typeof(JsonRpcMiddleware<>).MakeGenericType(types[i]);

                services.TryAddScoped(middlewareType, middlewareType);
            }

            return services;
        }

        /// <summary>Adds the specified JSON-RPC 2.0 service to the current <see cref="IServiceCollection" /> instance.</summary>
        /// <param name="services">The <see cref="IServiceCollection" /> instance to add the service to.</param>
        /// <param name="type">The type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentException"><paramref name="type" /> is not class or does not implement the <see cref="IJsonRpcService" /> interface.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="services" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        public static IServiceCollection AddJsonRpcService(this IServiceCollection services, Type type)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            InterfaceAssistant<IJsonRpcService>.VerifyTypeParam(type, nameof(type));

            var middlewareType = typeof(JsonRpcMiddleware<>).MakeGenericType(typeof(JsonRpcServiceHandler<>).MakeGenericType(type));

            services.TryAddScoped(middlewareType, middlewareType);

            return services;
        }

        /// <summary>Adds the specified JSON-RPC 2.0 service to the current <see cref="IServiceCollection" /> instance.</summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> instance to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services" /> is <see langword="null" />.</exception>
        public static IServiceCollection AddJsonRpcService<T>(this IServiceCollection services)
            where T : class, IJsonRpcService
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddScoped<JsonRpcMiddleware<JsonRpcServiceHandler<T>>, JsonRpcMiddleware<JsonRpcServiceHandler<T>>>();

            return services;
        }

        /// <summary>Adds JSON-RPC 2.0 services from the current application domain to the current <see cref="IServiceCollection" /> instance.</summary>
        /// <param name="services">The <see cref="IServiceCollection" /> instance to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services" /> is <see langword="null" />.</exception>
        public static IServiceCollection AddJsonRpcServices(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var types = InterfaceAssistant<IJsonRpcService>.GetDefinedTypes();

            for (var i = 0; i < types.Count; i++)
            {
                var jsonRpcRouteAtribute = types[i].GetCustomAttribute<JsonRpcRouteAttribute>();

                if (jsonRpcRouteAtribute == null)
                {
                    continue;
                }

                var middlewareType = typeof(JsonRpcMiddleware<>).MakeGenericType(typeof(JsonRpcServiceHandler<>).MakeGenericType(types[i]));

                services.TryAddScoped(middlewareType, middlewareType);
            }

            return services;
        }

        /// <summary>Adds JSON-RPC 2.0 handlers and services from the current application domain to the current <see cref="IServiceCollection" /> instance.</summary>
        /// <param name="services">The <see cref="IServiceCollection" /> instance to add the handler to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services" /> is <see langword="null" />.</exception>
        public static IServiceCollection AddJsonRpc(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddJsonRpcHandlers();
            services.AddJsonRpcServices();

            return services;
        }

        /// <summary>Adds JSON-RPC 2.0 handlers and services from the current application domain to the current <see cref="IServiceCollection" /> instance.</summary>
        /// <param name="services">The <see cref="IServiceCollection" /> instance to add the handler to.</param>
        /// <param name="configureOptions">The delegate to configure a <see cref="JsonRpcOptions" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services" /> or <paramref name="configureOptions" /> is <see langword="null" />.</exception>
        public static IServiceCollection AddJsonRpc(this IServiceCollection services, Action<JsonRpcOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.Configure(configureOptions);
            services.AddJsonRpcHandlers();
            services.AddJsonRpcServices();

            return services;
        }
    }
}