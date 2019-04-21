using System.Threading.Tasks;

#pragma warning disable IDE0060

namespace Anemonis.AspNetCore.JsonRpc.Benchmarks.TestStubs
{
    internal sealed class JsonRpcTestService : IJsonRpcService
    {
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

        [JsonRpcMethod("t0p1e0d0", 0)]
        public Task T0P1E0D0(long param)
        {
            return Task.CompletedTask;
        }

        [JsonRpcMethod("t0p1e1d0", 0)]
        public Task T0P1E1D0(long param)
        {
            throw new JsonRpcServiceException(1L, "m");
        }

        [JsonRpcMethod("t0p1e1d1", 0)]
        public Task T0P1E1D1(long param)
        {
            throw new JsonRpcServiceException(1L, "m", param);
        }

        [JsonRpcMethod("t0p2e0d0", "p0")]
        public Task T0P2E0D0(long param)
        {
            return Task.CompletedTask;
        }

        [JsonRpcMethod("t0p2e1d0", "p0")]
        public Task T0P2E1D0(long param)
        {
            throw new JsonRpcServiceException(1L, "m");
        }

        [JsonRpcMethod("t0p2e1d1", "p0")]
        public Task T0P2E1D1(long param)
        {
            throw new JsonRpcServiceException(1L, "m", param);
        }

        [JsonRpcMethod("t1p0e0d0")]
        public Task<long> T1P0E0D0()
        {
            return Task.FromResult(default(long));
        }

        [JsonRpcMethod("t1p0e1d0")]
        public Task<long> T1P0E1D0()
        {
            throw new JsonRpcServiceException(1L, "m");
        }

        [JsonRpcMethod("t1p0e1d1")]
        public Task<long> T1P0E1D1()
        {
            throw new JsonRpcServiceException(1L, "m", null);
        }

        [JsonRpcMethod("t1p1e0d0", 0)]
        public Task<long> T1P1E0D0(long param)
        {
            return Task.FromResult(param);
        }

        [JsonRpcMethod("t1p1e1d0", 0)]
        public Task<long> T1P1E1D0(long param)
        {
            throw new JsonRpcServiceException(1L, "m");
        }

        [JsonRpcMethod("t1p1e1d1", 0)]
        public Task<long> T1P1E1D1(long param)
        {
            throw new JsonRpcServiceException(1L, "m", param);
        }

        [JsonRpcMethod("t1p2e0d0", "p0")]
        public Task<long> T1P2E0D0(long param)
        {
            return Task.FromResult(param);
        }

        [JsonRpcMethod("t1p2e1d0", "p0")]
        public Task<long> T1P2E1D0(long param)
        {
            throw new JsonRpcServiceException(1L, "m");
        }

        [JsonRpcMethod("t1p2e1d1", "p0")]
        public Task<long> T1P2E1D1(long param)
        {
            throw new JsonRpcServiceException(1L, "m", param);
        }
    }
}

#pragma warning restore IDE0060