using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Anemonis.JsonRpc;

namespace Anemonis.AspNetCore.JsonRpc.UnitTests.TestStubs
{
    public sealed class JsonRpcTestHandler1 : IJsonRpcHandler, IDisposable
    {
        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public IReadOnlyDictionary<string, JsonRpcRequestContract> GetContracts()
        {
            return new Dictionary<string, JsonRpcRequestContract>
            {
                ["t0p0e0d0"] = new(),
                ["t0p0e1d0"] = new(),
                ["t0p0e1d1"] = new(),
                ["t0p1e0d0"] = new(
                    new[]
                    {
                        typeof(long),
                        typeof(string)
                    }),
                ["t0p1e1d0"] = new(
                    new[]
                    {
                        typeof(long),
                        typeof(string)
                    }),
                ["t0p1e1d1"] = new(
                    new[]
                    {
                        typeof(long),
                        typeof(string)
                    }),
                ["t0p2e0d0"] = new(
                    new Dictionary<string, Type>
                    {
                        ["p0"] = typeof(long),
                        ["p1"] = typeof(string)
                    }),
                ["t0p2e1d0"] = new(
                    new Dictionary<string, Type>
                    {
                        ["p0"] = typeof(long),
                        ["p1"] = typeof(string)
                    }),
                ["t0p2e1d1"] = new(
                    new Dictionary<string, Type>
                    {
                        ["p0"] = typeof(long),
                        ["p1"] = typeof(string)
                    }),
                ["t1p0e0d0"] = new(),
                ["t1p0e1d0"] = new(),
                ["t1p0e1d1"] = new(),
                ["t1p1e0d0"] = new(
                    new[]
                    {
                        typeof(long),
                        typeof(string)
                    }),
                ["t1p1e1d0"] = new(
                    new[]
                    {
                        typeof(long),
                        typeof(string)
                    }),
                ["t1p1e1d1"] = new(
                    new[]
                    {
                        typeof(long),
                        typeof(string)
                    }),
                ["t1p2e0d0"] = new(
                    new Dictionary<string, Type>
                    {
                        ["p0"] = typeof(long),
                        ["p1"] = typeof(string)
                    }),
                ["t1p2e1d0"] = new(
                    new Dictionary<string, Type>
                    {
                        ["p0"] = typeof(long),
                        ["p1"] = typeof(string)
                    }),
                ["t1p2e1d1"] = new(
                    new Dictionary<string, Type>
                    {
                        ["p0"] = typeof(long),
                        ["p1"] = typeof(string)
                    }),
                ["b0it"] = new(
                    new[]
                    {
                        typeof(long)
                    }),
                ["b1it"] = new(
                    new[]
                    {
                        typeof(long)
                    })
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
                        response = new(request.Id, new(1L, "m"));
                    }
                    break;
                case "t0p0e1d1":
                    {
                        response = new(request.Id, new(1L, "m", null));
                    }
                    break;
                case "t0p1e0d0":
                    {
                    }
                    break;
                case "t0p1e1d0":
                    {
                        response = new(request.Id, new(1L, "m"));
                    }
                    break;
                case "t0p1e1d1":
                    {
                        var parameters = request.ParametersByPosition;

                        response = new(request.Id, new(1L, "m", $"{parameters[0]}{parameters[1]}"));
                    }
                    break;
                case "t0p2e0d0":
                    {
                    }
                    break;
                case "t0p2e1d0":
                    {
                        response = new(request.Id, new(1L, "m"));
                    }
                    break;
                case "t0p2e1d1":
                    {
                        var parameters = request.ParametersByName;

                        response = new(request.Id, new(1L, "m", $"{parameters["p0"]}{parameters["p1"]}"));
                    }
                    break;
                case "t1p0e0d0":
                    {
                        response = new(request.Id, default(string));
                    }
                    break;
                case "t1p0e1d0":
                    {
                        response = new(request.Id, new(1L, "m"));
                    }
                    break;
                case "t1p0e1d1":
                    {
                        response = new(request.Id, new(1L, "m", null));
                    }
                    break;
                case "t1p1e0d0":
                    {
                        var parameters = request.ParametersByPosition;

                        response = new(request.Id, $"{parameters[0]}{parameters[1]}");
                    }
                    break;
                case "t1p1e1d0":
                    {
                        response = new(request.Id, new(1L, "m"));
                    }
                    break;
                case "t1p1e1d1":
                    {
                        var parameters = request.ParametersByPosition;

                        response = new(request.Id, new(1L, "m", $"{parameters[0]}{parameters[1]}"));
                    }
                    break;
                case "t1p2e0d0":
                    {
                        var parameters = request.ParametersByName;

                        response = new(request.Id, $"{parameters["p0"]}{parameters["p1"]}");
                    }
                    break;
                case "t1p2e1d0":
                    {
                        response = new(request.Id, new(1L, "m"));
                    }
                    break;
                case "t1p2e1d1":
                    {
                        var parameters = request.ParametersByName;

                        response = new(request.Id, new(1L, "m", $"{parameters["p0"]}{parameters["p1"]}"));
                    }
                    break;
                default:
                    {
                        throw new NotSupportedException();
                    }
            }

            return Task.FromResult(response);
        }

        public event EventHandler<EventArgs> Disposed;
    }
}
