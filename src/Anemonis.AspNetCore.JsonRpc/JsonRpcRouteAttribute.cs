// © Alexander Kozlenko. Licensed under the MIT License.

using System;

using Microsoft.AspNetCore.Http;

namespace Anemonis.AspNetCore.JsonRpc
{
    /// <summary>Specifies an attribute route on a JSON-RPC handler or service.</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class JsonRpcRouteAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="JsonRpcRouteAttribute" /> class.</summary>
        /// <param name="path">The request path for JSON-RPC methods.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is <see langword="null" />.</exception>
        public JsonRpcRouteAttribute(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            Path = new PathString(path);
        }

        internal PathString Path
        {
            get;
        }
    }
}
