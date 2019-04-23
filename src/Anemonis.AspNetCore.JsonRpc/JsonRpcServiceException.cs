// © Alexander Kozlenko. Licensed under the MIT License.

using System;

using Anemonis.JsonRpc;

namespace Anemonis.AspNetCore.JsonRpc
{
    /// <summary>A JSON-RPC service exception which indicates a JSON-RPC error response.</summary>
    public class JsonRpcServiceException : JsonRpcException
    {
        /// <summary>Initializes a new instance of the <see cref="JsonRpcServiceException" /> class.</summary>
        /// <param name="code">The number that indicates the error type that occurred.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> is <see langword="null" />.</exception>
        public JsonRpcServiceException(long code, string message)
            : base(message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Code = code;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcServiceException" /> class.</summary>
        /// <param name="code">The number that indicates the error type that occurred.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="data">The value that contains additional information about the error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> is <see langword="null" />.</exception>
        public JsonRpcServiceException(long code, string message, object data)
            : base(message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Code = code;
            ErrorData = data;
            HasErrorData = true;
        }

        /// <summary>Gets a number that indicates the error type that occurred.</summary>
        public long Code
        {
            get;
        }

        /// <summary>Gets an optional value that contains additional information about the error.</summary>
        public object ErrorData
        {
            get;
        }

        /// <summary>Gets a value indicating whether the additional information about the error is specified.</summary>
        public bool HasErrorData
        {
            get;
        }
    }
}
