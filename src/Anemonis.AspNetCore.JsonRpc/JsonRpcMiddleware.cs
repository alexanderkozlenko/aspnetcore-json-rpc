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

        private static Dictionary<long, JsonRpcError> CreateStandardJsonRpcErrors()
        {
            return new()
            {
                [JsonRpcErrorCode.InvalidFormat] =
                    new(JsonRpcErrorCode.InvalidFormat, Strings.GetString("rpc.error.invalid_format")),
                [JsonRpcErrorCode.InvalidOperation] =
                    new(JsonRpcErrorCode.InvalidOperation, Strings.GetString("rpc.error.invalid_operation")),
                [JsonRpcErrorCode.InvalidParameters] =
                    new(JsonRpcErrorCode.InvalidParameters, Strings.GetString("rpc.error.invalid_parameters")),
                [JsonRpcErrorCode.InvalidMethod] =
                    new(JsonRpcErrorCode.InvalidMethod, Strings.GetString("rpc.error.invalid_method")),
                [JsonRpcErrorCode.InvalidMessage] =
                    new(JsonRpcErrorCode.InvalidMessage, Strings.GetString("rpc.error.invalid_message"))
            };
        }

        private static Dictionary<long, JsonRpcResponse> CreateStandardJsonRpcResponses()
        {
            return new()
            {
                [JsonRpcErrorCode.InvalidFormat] =
                    new(default, new(JsonRpcErrorCode.InvalidFormat, Strings.GetString("rpc.error.invalid_format"))),
                [JsonRpcErrorCode.InvalidOperation] =
                    new(default, new(JsonRpcErrorCode.InvalidOperation, Strings.GetString("rpc.error.invalid_operation"))),
                [JsonRpcHandlerErrorCode.BatchHasDuplicateIdentifiers] =
                    new(default, new(JsonRpcHandlerErrorCode.BatchHasDuplicateIdentifiers, Strings.GetString("rpc.error.batch_has_duplicate_identifiers")))
            };
        }
    }
}
