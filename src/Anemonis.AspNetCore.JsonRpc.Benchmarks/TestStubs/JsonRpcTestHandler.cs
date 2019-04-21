using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Anemonis.JsonRpc;

namespace Anemonis.AspNetCore.JsonRpc.Benchmarks.TestStubs
{
    internal sealed class JsonRpcTestHandler : IJsonRpcHandler
    {
        public IReadOnlyDictionary<string, JsonRpcRequestContract> GetContracts()
        {
            return new Dictionary<string, JsonRpcRequestContract>
            {
                ["t0p0e0d0"] = new JsonRpcRequestContract(),
                ["t0p0e1d0"] = new JsonRpcRequestContract(),
                ["t0p0e1d1"] = new JsonRpcRequestContract(),
                ["t0p1e0d0"] = new JsonRpcRequestContract(
                    new[]
                    {
                        typeof(long),
                    }),
                ["t0p1e1d0"] = new JsonRpcRequestContract(
                    new[]
                    {
                        typeof(long),
                    }),
                ["t0p1e1d1"] = new JsonRpcRequestContract(
                    new[]
                    {
                        typeof(long),
                    }),
                ["t0p2e0d0"] = new JsonRpcRequestContract(
                    new Dictionary<string, Type>
                    {
                        ["p0"] = typeof(long)
                    }),
                ["t0p2e1d0"] = new JsonRpcRequestContract(
                    new Dictionary<string, Type>
                    {
                        ["p0"] = typeof(long)
                    }),
                ["t0p2e1d1"] = new JsonRpcRequestContract(
                    new Dictionary<string, Type>
                    {
                        ["p0"] = typeof(long)
                    }),
                ["t1p0e0d0"] = new JsonRpcRequestContract(),
                ["t1p0e1d0"] = new JsonRpcRequestContract(),
                ["t1p0e1d1"] = new JsonRpcRequestContract(),
                ["t1p1e0d0"] = new JsonRpcRequestContract(
                    new[]
                    {
                        typeof(long),
                    }),
                ["t1p1e1d0"] = new JsonRpcRequestContract(
                    new[]
                    {
                        typeof(long),
                    }),
                ["t1p1e1d1"] = new JsonRpcRequestContract(
                    new[]
                    {
                        typeof(long),
                    }),
                ["t1p2e0d0"] = new JsonRpcRequestContract(
                    new Dictionary<string, Type>
                    {
                        ["p0"] = typeof(long)
                    }),
                ["t1p2e1d0"] = new JsonRpcRequestContract(
                    new Dictionary<string, Type>
                    {
                        ["p0"] = typeof(long)
                    }),
                ["t1p2e1d1"] = new JsonRpcRequestContract(
                    new Dictionary<string, Type>
                    {
                        ["p0"] = typeof(long)
                    }),
            };
        }

        public Task<JsonRpcResponse> HandleAsync(JsonRpcRequest request)
        {
            var response = default(JsonRpcResponse);

            switch (request.Method)
            {
                case "t0p0e0d0":
                    {
                    }
                    break;
                case "t0p0e1d0":
                    {
                        response = new JsonRpcResponse(request.Id, new JsonRpcError(1L, "m"));
                    }
                    break;
                case "t0p0e1d1":
                    {
                        response = new JsonRpcResponse(request.Id, new JsonRpcError(1L, "m", null));
                    }
                    break;
                case "t0p1e0d0":
                    {
                    }
                    break;
                case "t0p1e1d0":
                    {
                        response = new JsonRpcResponse(request.Id, new JsonRpcError(1L, "m"));
                    }
                    break;
                case "t0p1e1d1":
                    {
                        var parameters = request.ParametersByPosition;

                        response = new JsonRpcResponse(request.Id, new JsonRpcError(1L, "m", parameters[0]));
                    }
                    break;
                case "t0p2e0d0":
                    {
                    }
                    break;
                case "t0p2e1d0":
                    {
                        response = new JsonRpcResponse(request.Id, new JsonRpcError(1L, "m"));
                    }
                    break;
                case "t0p2e1d1":
                    {
                        var parameters = request.ParametersByName;

                        response = new JsonRpcResponse(request.Id, new JsonRpcError(1L, "m", parameters["p0"]));
                    }
                    break;
                case "t1p0e0d0":
                    {
                        response = new JsonRpcResponse(request.Id, default(string));
                    }
                    break;
                case "t1p0e1d0":
                    {
                        response = new JsonRpcResponse(request.Id, new JsonRpcError(1L, "m"));
                    }
                    break;
                case "t1p0e1d1":
                    {
                        response = new JsonRpcResponse(request.Id, new JsonRpcError(1L, "m", null));
                    }
                    break;
                case "t1p1e0d0":
                    {
                        var parameters = request.ParametersByPosition;

                        response = new JsonRpcResponse(request.Id, parameters[0]);
                    }
                    break;
                case "t1p1e1d0":
                    {
                        response = new JsonRpcResponse(request.Id, new JsonRpcError(1L, "m"));
                    }
                    break;
                case "t1p1e1d1":
                    {
                        var parameters = request.ParametersByPosition;

                        response = new JsonRpcResponse(request.Id, new JsonRpcError(1L, "m", parameters[0]));
                    }
                    break;
                case "t1p2e0d0":
                    {
                        var parameters = request.ParametersByName;

                        response = new JsonRpcResponse(request.Id, parameters["p0"]);
                    }
                    break;
                case "t1p2e1d0":
                    {
                        response = new JsonRpcResponse(request.Id, new JsonRpcError(1L, "m"));
                    }
                    break;
                case "t1p2e1d1":
                    {
                        var parameters = request.ParametersByName;

                        response = new JsonRpcResponse(request.Id, new JsonRpcError(1L, "m", parameters["p0"]));
                    }
                    break;
                default:
                    {
                        throw new NotSupportedException();
                    }
            }

            return Task.FromResult(response);
        }
    }
}