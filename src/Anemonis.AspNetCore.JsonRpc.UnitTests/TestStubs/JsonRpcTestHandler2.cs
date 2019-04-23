using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Anemonis.JsonRpc;

namespace Anemonis.AspNetCore.JsonRpc.UnitTests.TestStubs
{
    [JsonRpcRoute("/api")]
    public sealed class JsonRpcTestHandler2 : IJsonRpcHandler
    {
        public IReadOnlyDictionary<string, JsonRpcRequestContract> GetContracts()
        {
            throw new NotImplementedException();
        }

        public Task<JsonRpcResponse> HandleAsync(JsonRpcRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
