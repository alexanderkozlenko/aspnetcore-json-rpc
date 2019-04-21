// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Reflection;

using Anemonis.AspNetCore.JsonRpc;

using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>The JSON-RPC 2.0 middleware extensions for the <see cref="IApplicationBuilder" />.</summary>
    public static class JsonRpcBuilderExtensions
    {
        /// <summary>Adds the specified JSON-RPC 2.0 handler to the application's request pipeline for the specified path.</summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" /> to add the middleware to.</param>
        /// <param name="type">The type of the handler.</param>
        /// <param name="path">The request path for JSON-RPC methods.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentException"><paramref name="type" /> is not class or does not implement the <see cref="IJsonRpcHandler" /> interface.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="builder" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        public static IApplicationBuilder UseJsonRpcHandler(this IApplicationBuilder builder, Type type, PathString path = default)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            InterfaceAssistant<IJsonRpcHandler>.VerifyTypeParam(type, nameof(type));

            if (!path.HasValue)
            {
                var jsonRpcRouteAtribute = type.GetCustomAttribute<JsonRpcRouteAttribute>();

                if (jsonRpcRouteAtribute != null)
                {
                    path = jsonRpcRouteAtribute.Path;
                }
            }

            var middlewareType = typeof(JsonRpcMiddleware<>).MakeGenericType(type);

            return builder.Map(path, b => b.UseMiddleware(middlewareType));
        }

        /// <summary>Adds the specified JSON-RPC 2.0 handler to the application's request pipeline for the specified path.</summary>
        /// <typeparam name="T">The type of the handler.</typeparam>
        /// <param name="builder">The <see cref="IApplicationBuilder" /> to add the middleware to.</param>
        /// <param name="path">The request path for JSON-RPC methods.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
        public static IApplicationBuilder UseJsonRpcHandler<T>(this IApplicationBuilder builder, PathString path = default)
            where T : class, IJsonRpcHandler
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (!path.HasValue)
            {
                var jsonRpcRouteAtribute = typeof(T).GetCustomAttribute<JsonRpcRouteAttribute>();

                if (jsonRpcRouteAtribute != null)
                {
                    path = jsonRpcRouteAtribute.Path;
                }
            }

            return builder.Map(path, b => b.UseMiddleware(typeof(JsonRpcMiddleware<T>)));
        }

        /// <summary>Adds JSON-RPC 2.0 handlers from the current application domain to the application's request pipeline.</summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" /> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
        public static IApplicationBuilder UseJsonRpcHandlers(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
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

                builder.Map(jsonRpcRouteAtribute.Path, b => b.UseMiddleware(middlewareType));
            }

            return builder;
        }

        /// <summary>Adds the specified JSON-RPC 2.0 service to the application's request pipeline for the specified path.</summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" /> to add the middleware to.</param>
        /// <param name="type">The type of the service.</param>
        /// <param name="path">The request path for JSON-RPC methods.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentException"><paramref name="type" /> is not class or does not implement the <see cref="IJsonRpcService" /> interface.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="builder" /> or <paramref name="type" /> is <see langword="null" />.</exception>
        public static IApplicationBuilder UseJsonRpcService(this IApplicationBuilder builder, Type type, PathString path = default)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            InterfaceAssistant<IJsonRpcService>.VerifyTypeParam(type, nameof(type));

            if (!path.HasValue)
            {
                var jsonRpcRouteAtribute = type.GetCustomAttribute<JsonRpcRouteAttribute>();

                if (jsonRpcRouteAtribute != null)
                {
                    path = jsonRpcRouteAtribute.Path;
                }
            }

            var middlewareType = typeof(JsonRpcMiddleware<>).MakeGenericType(typeof(JsonRpcServiceHandler<>).MakeGenericType(type));

            return builder.Map(path, b => b.UseMiddleware(middlewareType));
        }

        /// <summary>Adds the specified JSON-RPC 2.0 service to the application's request pipeline for the specified path.</summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="builder">The <see cref="IApplicationBuilder" /> to add the middleware to.</param>
        /// <param name="path">The request path for JSON-RPC methods.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
        public static IApplicationBuilder UseJsonRpcService<T>(this IApplicationBuilder builder, PathString path = default)
            where T : class, IJsonRpcService
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (!path.HasValue)
            {
                var jsonRpcRouteAtribute = typeof(T).GetCustomAttribute<JsonRpcRouteAttribute>();

                if (jsonRpcRouteAtribute != null)
                {
                    path = jsonRpcRouteAtribute.Path;
                }
            }

            return builder.Map(path, b => b.UseMiddleware(typeof(JsonRpcMiddleware<JsonRpcServiceHandler<T>>)));
        }

        /// <summary>Adds JSON-RPC 2.0 services from the current application domain to the application's request pipeline.</summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" /> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
        public static IApplicationBuilder UseJsonRpcServices(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
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

                builder.Map(jsonRpcRouteAtribute.Path, b => b.UseMiddleware(middlewareType));
            }

            return builder;
        }

        /// <summary>Adds JSON-RPC 2.0 handlers and services from the current application domain to the application's request pipeline.</summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" /> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder" /> is <see langword="null" />.</exception>
        public static IApplicationBuilder UseJsonRpc(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.UseJsonRpcHandlers();
            builder.UseJsonRpcServices();

            return builder;
        }
    }
}