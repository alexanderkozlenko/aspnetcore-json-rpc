// © Alexander Kozlenko. Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

using Anemonis.JsonRpc;

namespace Anemonis.AspNetCore.JsonRpc
{
    /// <summary>Defines a JSON-RPC 2.0 handler.</summary>
    public interface IJsonRpcHandler
    {
        /// <summary>Get contracts for JSON-RPC requests deserialization.</summary>
        /// <returns>A dictionary with JSON-RPC request contracts.</returns>
        IReadOnlyDictionary<string, JsonRpcRequestContract> GetContracts();

        /// <summary>Handles a JSON-RPC request and returns a JSON-RPC response or <see langword="null" /> for a notification as an asynchronous operation.</summary>
        /// <param name="request">The JSON-RPC request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a JSON-RPC response or <see langword="null" /> for a notification.</returns>
        Task<JsonRpcResponse> HandleAsync(JsonRpcRequest request);
    }
}
