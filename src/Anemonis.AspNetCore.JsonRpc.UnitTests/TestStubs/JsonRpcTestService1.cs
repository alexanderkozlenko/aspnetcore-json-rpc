using System;
using System.Threading.Tasks;

#pragma warning disable IDE0060

namespace Anemonis.AspNetCore.JsonRpc.UnitTests.TestStubs
{
    public sealed class JsonRpcTestService1 : IJsonRpcService, IDisposable
    {
        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        [JsonRpcMethod("t0p0e0d0")]
        public Task T0P0E0D0()
        {
            return Task.CompletedTask;
        }

        [JsonRpcMethod("t0p0e1d0")]
        public Task T0P0E1D0()
        {
            throw new JsonRpcServiceException(1L, "m");
        }

        [JsonRpcMethod("t0p0e1d1")]
        public Task T0P0E1D1()
        {
            throw new JsonRpcServiceException(1L, "m", null);
        }

        [JsonRpcMethod("t0p1e0d0", 1, 0)]
        public Task T0P1E0D0(string param1, long param0 = 1L)
        {
            return Task.CompletedTask;
        }

        [JsonRpcMethod("t0p1e1d0", 1, 0)]
        public Task T0P1E1D0(string param1, long param0 = 1L)
        {
            throw new JsonRpcServiceException(1L, "m");
        }

        [JsonRpcMethod("t0p1e1d1", 1, 0)]
        public Task T0P1E1D1(string param1, long param0 = 1L)
        {
            throw new JsonRpcServiceException(1L, "m", param0 + param1);
        }

        [JsonRpcMethod("t0p2e0d0", "p1", "p0")]
        public Task T0P2E0D0(string param1, long param0 = 1L)
        {
            return Task.CompletedTask;
        }

        [JsonRpcMethod("t0p2e1d0", "p1", "p0")]
        public Task T0P2E1D0(string param1, long param0 = 1L)
        {
            throw new JsonRpcServiceException(1L, "m");
        }

        [JsonRpcMethod("t0p2e1d1", "p1", "p0")]
        public Task T0P2E1D1(string param1, long param0 = 1L)
        {
            throw new JsonRpcServiceException(1L, "m", param0 + param1);
        }

        [JsonRpcMethod("t1p0e0d0")]
        public Task<string> T1P0E0D0()
        {
            return Task.FromResult(default(string));
        }

        [JsonRpcMethod("t1p0e1d0")]
        public Task<string> T1P0E1D0()
        {
            throw new JsonRpcServiceException(1L, "m");
        }

        [JsonRpcMethod("t1p0e1d1")]
        public Task<string> T1P0E1D1()
        {
            throw new JsonRpcServiceException(1L, "m", null);
        }

        [JsonRpcMethod("t1p1e0d0", 1, 0)]
        public Task<string> T1P1E0D0(string param1, long param0 = 1L)
        {
            return Task.FromResult(param0 + param1);
        }

        [JsonRpcMethod("t1p1e1d0", 1, 0)]
        public Task<string> T1P1E1D0(string param1, long param0 = 1L)
        {
            throw new JsonRpcServiceException(1L, "m");
        }

        [JsonRpcMethod("t1p1e1d1", 1, 0)]
        public Task<string> T1P1E1D1(string param1, long param0 = 1L)
        {
            throw new JsonRpcServiceException(1L, "m", param0 + param1);
        }

        [JsonRpcMethod("t1p2e0d0", "p1", "p0")]
        public Task<string> T1P2E0D0(string param1, long param0 = 1L)
        {
            return Task.FromResult(param0 + param1);
        }

        [JsonRpcMethod("t1p2e1d0", "p1", "p0")]
        public Task<string> T1P2E1D0(string param1, long param0 = 1L)
        {
            throw new JsonRpcServiceException(1L, "m");
        }

        [JsonRpcMethod("t1p2e1d1", "p1", "p0")]
        public Task<string> T1P2E1D1(string param1, long param0 = 1L)
        {
            throw new JsonRpcServiceException(1L, "m", param0 + param1);
        }

        public event EventHandler<EventArgs> Disposed;
    }
}
