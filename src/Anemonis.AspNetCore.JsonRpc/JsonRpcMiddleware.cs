// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

using Anemonis.AspNetCore.JsonRpc.Resources;
using Anemonis.JsonRpc;

namespace Anemonis.AspNetCore.JsonRpc
{
    /// <summary>Represents a middleware for adding a JSON-RPC handler to the application's request pipeline.</summary>
    public abstract class JsonRpcMiddleware
    {
        private protected static readonly string ContentTypeHeaderValue = $"{MediaTypes.ApplicationJson}; charset=utf-8";
        private protected static readonly Dictionary<string, Encoding> SupportedEncodings = CreateSupportedEncodings();
        private protected static readonly Dictionary<long, JsonRpcError> StandardJsonRpcErrors = CreateStandardJsonRpcErrors();
        private protected static readonly Dictionary<long, JsonRpcResponse> StandardJsonRpcResponses = CreateStandardJsonRpcResponses();

        private protected JsonRpcMiddleware()
        {
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
                    new JsonRpcError(JsonRpcErrorCode.InvalidMessage, Strings.GetString("rpc.error.invalid_message"))
            };
        }

        private static Dictionary<long, JsonRpcResponse> CreateStandardJsonRpcResponses()
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
