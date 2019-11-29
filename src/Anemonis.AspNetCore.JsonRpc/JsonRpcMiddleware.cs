// © Alexander Kozlenko. Licensed under the MIT License.

using System.Collections.Generic;

using Anemonis.AspNetCore.JsonRpc.Resources;
using Anemonis.JsonRpc;

namespace Anemonis.AspNetCore.JsonRpc
{
    /// <summary>Represents a middleware for adding a JSON-RPC handler to the application's request pipeline.</summary>
    public abstract class JsonRpcMiddleware
    {
        private protected static readonly string ContentTypeHeaderValue = $"{JsonRpcTransport.MediaType}; charset={JsonRpcTransport.Charset}";
        private protected static readonly IReadOnlyDictionary<long, JsonRpcError> StandardJsonRpcErrors = CreateStandardJsonRpcErrors();
        private protected static readonly IReadOnlyDictionary<long, JsonRpcResponse> StandardJsonRpcResponses = CreateStandardJsonRpcResponses();

        private protected JsonRpcMiddleware()
        {
        }

        private static IReadOnlyDictionary<long, JsonRpcError> CreateStandardJsonRpcErrors()
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
                    new JsonRpcError(JsonRpcErrorCode.InvalidMessage, Strings.GetString("rpc.error.invalid_message"))
            };
        }

        private static IReadOnlyDictionary<long, JsonRpcResponse> CreateStandardJsonRpcResponses()
        {
            return new Dictionary<long, JsonRpcResponse>
            {
                [JsonRpcErrorCode.InvalidFormat] =
                    new JsonRpcResponse(default, new JsonRpcError(JsonRpcErrorCode.InvalidFormat, Strings.GetString("rpc.error.invalid_format"))),
                [JsonRpcErrorCode.InvalidOperation] =
                    new JsonRpcResponse(default, new JsonRpcError(JsonRpcErrorCode.InvalidOperation, Strings.GetString("rpc.error.invalid_operation"))),
                [JsonRpcHandlerErrorCode.BatchHasDuplicateIdentifiers] =
                    new JsonRpcResponse(default, new JsonRpcError(JsonRpcHandlerErrorCode.BatchHasDuplicateIdentifiers, Strings.GetString("rpc.error.batch_has_duplicate_identifiers")))
            };
        }
    }
}
