# Anemonis.AspNetCore.JsonRpc

[JSON-RPC 2.0](http://www.jsonrpc.org/specification) middleware for ASP.NET Core 3 based on the [JSON-RPC 2.0 Transport: HTTP](https://www.simple-is-better.org/json-rpc/transport_http.html) specification and the [Anemonis.JsonRpc](https://github.com/alexanderkozlenko/json-rpc)
 serializer.

| [![](https://img.shields.io/gitter/room/nwjs/nw.js.svg?style=flat-square)](https://gitter.im/anemonis/aspnetcore-json-rpc) | Release | Current |
|---|---|---|
| Artifacts | [![](https://img.shields.io/nuget/vpre/Anemonis.AspNetCore.JsonRpc.svg?style=flat-square)](https://www.nuget.org/packages/Anemonis.AspNetCore.JsonRpc) | [![](https://img.shields.io/myget/alexanderkozlenko/vpre/Anemonis.AspNetCore.JsonRpc.svg?label=myget&style=flat-square)](https://www.myget.org/feed/alexanderkozlenko/package/nuget/Anemonis.AspNetCore.JsonRpc) |
| Code Health | | [![](https://img.shields.io/sonar/coverage/aspnetcore-json-rpc?format=long&server=https%3A%2F%2Fsonarcloud.io&style=flat-square)](https://sonarcloud.io/component_measures?id=aspnetcore-json-rpc&metric=coverage&view=list) [![](https://img.shields.io/sonar/violations/aspnetcore-json-rpc?format=long&server=https%3A%2F%2Fsonarcloud.io&style=flat-square)](https://sonarcloud.io/project/issues?id=aspnetcore-json-rpc&resolved=false) |
| Build Status | | [![](https://img.shields.io/azure-devops/build/alexanderkozlenko/github-pipelines/4?label=master&style=flat-square)](https://dev.azure.com/alexanderkozlenko/github-pipelines/_build?definitionId=4&_a=summary) |

## Project Details

- The middleware transparently handles batch JSON-RPC requests.
- The middleware automatically handles standard JSON-RPC errors.
- The middleware does not verify the `Content-Length` header.
- A service supports default method parameter values for named parameters not provided in a request.

In addition to the standard JSON-RPC error codes the middleware may return the following JSON-RPC errors:

| Code | Reason |
| :---: | --- |
| `-32000` | The provided batch contains requests with duplicate identifiers |

In addition to the standard JSON-RPC HTTP error codes the middleware may return the following HTTP error codes:

| Code | Reason |
| :---: | --- |
| `415` | The `Content-Encoding` header is specified |

According to the current logging configuration, the following events may appear in a journal:

| ID | Level | Reason |
| :---: | :---: | --- |
| `1000` | `Trace` | The request contains a single JSON-RPC message |
| `1001` | `Trace` | The request contains a JSON-RPC batch |
| `1100` | `Debug` | A JSON-RPC notification handled successfully |
| `1110` | `Debug` | A JSON-RPC request handled with result successfully |
| `1111` | `Debug` | A JSON-RPC request handled with error successfully |
| `1200` | `Information` | A JSON-RPC request handled with result as notification due to client demand |
| `1201` | `Information` | A JSON-RPC request handled with error as notification due to client demand |
| `1300` | `Warning` | A JSON-RPC notification handled as request due to server configuration |
| `1400` | `Error` | The request contains invalid JSON-RPC data |
| `1401` | `Error` | A JSON-RPC request is invalid |
| `1402` | `Error` | The request contains a JSON-RPC batch with duplicate identifiers |

## Code Examples

```cs
[JsonRpcRoute("/api")]
public class JsonRpcService : IJsonRpcService
{
    [JsonRpcMethod("m1", "p1", "p2")]
    public Task<long> InvokeMethod1Async(long p1, long p2)
    {
        if (p2 == 0L)
        {
            throw new JsonRpcServiceException(100L);
        }

        return Task.FromResult(p1 / p2);
    }

    [JsonRpcMethod("m2", 0, 1)]
    public Task<long> InvokeMethod2Async(long p1, long p2)
    {
        return Task.FromResult(p1 + p2);
    }
}

public class Startup : IStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddJsonRpc();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseJsonRpc();
    }
}
```
or
```cs
[JsonRpcRoute("/api")]
public class JsonRpcHandler : IJsonRpcHandler
{
    public IReadOnlyDictionary<string, JsonRpcRequestContract> GetContracts()
    {
        var contract1Types = new Dictionary<string, Type>();
        var contract2Types = new Type[2];

        contract1Types["p1"] = typeof(long);
        contract1Types["p2"] = typeof(long);
        contract2Types[0] = typeof(long);
        contract2Types[1] = typeof(long);

        var contracts = new Dictionary<string, JsonRpcRequestContract>();

        contracts["m1"] = new JsonRpcRequestContract(contract1Types);
        contracts["m2"] = new JsonRpcRequestContract(contract2Types);

        return contracts;
    }

    public Task<JsonRpcResponse> HandleAsync(JsonRpcRequest request)
    {
        var response = default(JsonRpcResponse);

        switch (request.Method)
        {
            case "m1":
                {
                    var p1 = (long)request.ParametersByName["p1"];
                    var p2 = (long)request.ParametersByName["p2"];

                    response = p2 != 0L ?
                        new JsonRpcResponse(p1 / p2, request.Id) :
                        new JsonRpcResponse(new JsonRpcError(100L), request.Id);
                }
                break;
            case "m2":
                {
                    var p1 = (long)request.ParametersByPosition[0];
                    var p2 = (long)request.ParametersByPosition[1];

                    response = new JsonRpcResponse(p1 + p2, request.Id);
                }
                break;
        }

        return Task.FromResult(response);
    }
}

public class Startup : IStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddJsonRpc();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseJsonRpc();
    }
}
```

## Quicklinks

- [Contributing Guidelines](./CONTRIBUTING.md)
- [Code of Conduct](./CODE_OF_CONDUCT.md)
