using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Anemonis.AspNetCore.JsonRpc.Benchmarks.Resources;
using Anemonis.AspNetCore.JsonRpc.Benchmarks.TestStubs;

using BenchmarkDotNet.Attributes;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Anemonis.AspNetCore.JsonRpc.Benchmarks.TestSuites
{
    public class JsonRpcMiddlewareBenchmarks
    {
        private static readonly IReadOnlyDictionary<string, byte[]> _resources = CreateResourceDictionary();
        private static readonly StringValues _acceptHeaderValue = new StringValues("application/json; charset=utf-8");

        private readonly IMiddleware _middleware1 = CreateJsonRpcHandlerMiddleware();
        private readonly IMiddleware _middleware2 = CreateJsonRpcServiceMiddleware();

        private static IMiddleware CreateJsonRpcHandlerMiddleware()
        {
            var webHost = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddSingleton<JsonRpcMiddleware<JsonRpcTestHandler>, JsonRpcMiddleware<JsonRpcTestHandler>>())
                .Configure(ab => ab.UseMiddleware<JsonRpcMiddleware<JsonRpcTestHandler>>())
                .Build();

            return (IMiddleware)webHost.Services.GetService(typeof(JsonRpcMiddleware<JsonRpcTestHandler>));
        }

        public static IMiddleware CreateJsonRpcServiceMiddleware()
        {
            var webHost = new WebHostBuilder()
                .ConfigureServices(sc => sc.AddSingleton<JsonRpcMiddleware<JsonRpcServiceHandler<JsonRpcTestService>>, JsonRpcMiddleware<JsonRpcServiceHandler<JsonRpcTestService>>>())
                .Configure(ab => ab.UseMiddleware<JsonRpcMiddleware<JsonRpcServiceHandler<JsonRpcTestService>>>())
                .Build();

            return (IMiddleware)webHost.Services.GetService(typeof(JsonRpcMiddleware<JsonRpcServiceHandler<JsonRpcTestService>>));
        }

        private static IReadOnlyDictionary<string, byte[]> CreateResourceDictionary()
        {
            var resources = new Dictionary<string, byte[]>(StringComparer.Ordinal);

            foreach (var code in CreateTestCodes())
            {
                resources[code] = Encoding.UTF8.GetBytes(EmbeddedResourceManager.GetString($"Assets.{code}.json"));
            }

            return resources;
        }

        private static IEnumerable<string> CreateTestCodes()
        {
            return new[]
            {
                "b0t0p0e0d0", "b0t0p0e1d0", "b0t0p0e1d1",
                "b0t0p1e0d0", "b0t0p1e1d0", "b0t0p1e1d1",
                "b0t0p2e0d0", "b0t0p2e1d0", "b0t0p2e1d1",
                "b0t1p0e0d0", "b0t1p0e1d0", "b0t1p0e1d1",
                "b0t1p1e0d0", "b0t1p1e1d0", "b0t1p1e1d1",
                "b0t1p2e0d0", "b0t1p2e1d0", "b0t1p2e1d1",
                "b1t0p0e0d0", "b1t0p0e1d0", "b1t0p0e1d1",
                "b1t0p1e0d0", "b1t0p1e1d0", "b1t0p1e1d1",
                "b1t0p2e0d0", "b1t0p2e1d0", "b1t0p2e1d1",
                "b1t1p0e0d0", "b1t1p0e1d0", "b1t1p0e1d1",
                "b1t1p1e0d0", "b1t1p1e1d0", "b1t1p1e1d1",
                "b1t1p2e0d0", "b1t1p2e1d0", "b1t1p2e1d1"
            };
        }

        private static HttpContext CreateHttpContext(string name)
        {
            var result = new DefaultHttpContext();

            result.Request.Method = HttpMethods.Post;
            result.Request.ContentType = "application/json; charset=utf-8";
            result.Request.Headers.Add(HeaderNames.Accept, _acceptHeaderValue);
            result.Request.Body = new MemoryStream(_resources[name], false);
            result.Response.Body = new MemoryStream();

            return result;
        }

        private static Task FinishInvokeChain(HttpContext context)
        {
            return Task.CompletedTask;
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T0P0E0D0")]
        public async Task HandlerB0T0P0E0D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t0p0e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T0P0E1D0")]
        public async Task HandlerB0T0P0E1D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t0p0e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T0P0E1D1")]
        public async Task HandlerB0T0P0E1D1()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t0p0e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T0P1E0D0")]
        public async Task HandlerB0T0P1E0D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t0p1e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T0P1E1D0")]
        public async Task HandlerB0T0P1E1D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t0p1e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T0P1E1D1")]
        public async Task HandlerB0T0P1E1D1()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t0p1e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T0P2E0D0")]
        public async Task HandlerB0T0P2E0D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t0p2e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T0P2E1D0")]
        public async Task HandlerB0T0P2E1D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t0p2e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T0P2E1D1")]
        public async Task HandlerB0T0P2E1D1()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t0p2e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T1P0E0D0")]
        public async Task HandlerB0T1P0E0D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t1p0e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T1P0E1D0")]
        public async Task HandlerB0T1P0E1D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t1p0e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T1P0E1D1")]
        public async Task HandlerB0T1P0E1D1()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t1p0e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T1P1E0D0")]
        public async Task HandlerB0T1P1E0D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t1p1e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T1P1E1D0")]
        public async Task HandlerB0T1P1E1D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t1p1e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T1P1E1D1")]
        public async Task HandlerB0T1P1E1D1()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t1p1e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T1P2E0D0")]
        public async Task HandlerB0T1P2E0D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t1p2e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T1P2E1D0")]
        public async Task HandlerB0T1P2E1D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t1p2e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B0T1P2E1D1")]
        public async Task HandlerB0T1P2E1D1()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b0t1p2e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T0P0E0D0")]
        public async Task HandlerB1T0P0E0D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t0p0e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T0P0E1D0")]
        public async Task HandlerB1T0P0E1D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t0p0e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T0P0E1D1")]
        public async Task HandlerB1T0P0E1D1()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t0p0e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T0P1E0D0")]
        public async Task HandlerB1T0P1E0D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t0p1e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T0P1E1D0")]
        public async Task HandlerB1T0P1E1D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t0p1e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T0P1E1D1")]
        public async Task HandlerB1T0P1E1D1()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t0p1e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T0P2E0D0")]
        public async Task HandlerB1T0P2E0D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t0p2e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T0P2E1D0")]
        public async Task HandlerB1T0P2E1D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t0p2e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T0P2E1D1")]
        public async Task HandlerB1T0P2E1D1()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t0p2e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T1P0E0D0")]
        public async Task HandlerB1T1P0E0D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t1p0e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T1P0E1D0")]
        public async Task HandlerB1T1P0E1D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t1p0e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T1P0E1D1")]
        public async Task HandlerB1T1P0E1D1()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t1p0e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T1P1E0D0")]
        public async Task HandlerB1T1P1E0D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t1p1e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T1P1E1D0")]
        public async Task HandlerB1T1P1E1D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t1p1e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T1P1E1D1")]
        public async Task HandlerB1T1P1E1D1()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t1p1e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T1P2E0D0")]
        public async Task HandlerB1T1P2E0D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t1p2e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T1P2E1D0")]
        public async Task HandlerB1T1P2E1D0()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t1p2e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=H-CASE=B1T1P2E1D1")]
        public async Task HandlerB1T1P2E1D1()
        {
            await _middleware1.InvokeAsync(CreateHttpContext("b1t1p2e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T0P0E0D0")]
        public async Task ServiceB0T0P0E0D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t0p0e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T0P0E1D0")]
        public async Task ServiceB0T0P0E1D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t0p0e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T0P0E1D1")]
        public async Task ServiceB0T0P0E1D1()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t0p0e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T0P1E0D0")]
        public async Task ServiceB0T0P1E0D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t0p1e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T0P1E1D0")]
        public async Task ServiceB0T0P1E1D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t0p1e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T0P1E1D1")]
        public async Task ServiceB0T0P1E1D1()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t0p1e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T0P2E0D0")]
        public async Task ServiceB0T0P2E0D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t0p2e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T0P2E1D0")]
        public async Task ServiceB0T0P2E1D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t0p2e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T0P2E1D1")]
        public async Task ServiceB0T0P2E1D1()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t0p2e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T1P0E0D0")]
        public async Task ServiceB0T1P0E0D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t1p0e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T1P0E1D0")]
        public async Task ServiceB0T1P0E1D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t1p0e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T1P0E1D1")]
        public async Task ServiceB0T1P0E1D1()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t1p0e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T1P1E0D0")]
        public async Task ServiceB0T1P1E0D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t1p1e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T1P1E1D0")]
        public async Task ServiceB0T1P1E1D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t1p1e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T1P1E1D1")]
        public async Task ServiceB0T1P1E1D1()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t1p1e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T1P2E0D0")]
        public async Task ServiceB0T1P2E0D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t1p2e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T1P2E1D0")]
        public async Task ServiceB0T1P2E1D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t1p2e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B0T1P2E1D1")]
        public async Task ServiceB0T1P2E1D1()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b0t1p2e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T0P0E0D0")]
        public async Task ServiceB1T0P0E0D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t0p0e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T0P0E1D0")]
        public async Task ServiceB1T0P0E1D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t0p0e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T0P0E1D1")]
        public async Task ServiceB1T0P0E1D1()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t0p0e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T0P1E0D0")]
        public async Task ServiceB1T0P1E0D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t0p1e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T0P1E1D0")]
        public async Task ServiceB1T0P1E1D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t0p1e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T0P1E1D1")]
        public async Task ServiceB1T0P1E1D1()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t0p1e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T0P2E0D0")]
        public async Task ServiceB1T0P2E0D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t0p2e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T0P2E1D0")]
        public async Task ServiceB1T0P2E1D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t0p2e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T0P2E1D1")]
        public async Task ServiceB1T0P2E1D1()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t0p2e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T1P0E0D0")]
        public async Task ServiceB1T1P0E0D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t1p0e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T1P0E1D0")]
        public async Task ServiceB1T1P0E1D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t1p0e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T1P0E1D1")]
        public async Task ServiceB1T1P0E1D1()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t1p0e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T1P1E0D0")]
        public async Task ServiceB1T1P1E0D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t1p1e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T1P1E1D0")]
        public async Task ServiceB1T1P1E1D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t1p1e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T1P1E1D1")]
        public async Task ServiceB1T1P1E1D1()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t1p1e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T1P2E0D0")]
        public async Task ServiceB1T1P2E0D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t1p2e0d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T1P2E1D0")]
        public async Task ServiceB1T1P2E1D0()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t1p2e1d0"), FinishInvokeChain).ConfigureAwait(false);
        }

        [Benchmark(Description = "TYPE=S-CASE=B1T1P2E1D1")]
        public async Task ServiceB1T1P2E1D1()
        {
            await _middleware2.InvokeAsync(CreateHttpContext("b1t1p2e1d1"), FinishInvokeChain).ConfigureAwait(false);
        }
    }
}