using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Anemonis.AspNetCore.JsonRpc.UnitTests.TestStubs;
using Anemonis.JsonRpc;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Anemonis.AspNetCore.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcServiceHandlerTests
    {
        [TestMethod]
        public void ConstructorWithServicesWhenServicesIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcServiceHandler<JsonRpcTestService1>(null));
        }

        [TestMethod]
        public void Dispose()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
            var jsonRpcService = new JsonRpcTestService1();
            var jsonRpcServiceDisposed = false;

            jsonRpcService.Disposed += (sender, e) => jsonRpcServiceDisposed = true;

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(jsonRpcService);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);

            jsonRpcServiceHandler.Dispose();

            Assert.IsTrue(jsonRpcServiceDisposed);
        }

        [TestMethod]
        public void GetContracts()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);
            var jsonRpcContracts = jsonRpcServiceHandler.GetContracts();

            Assert.IsNotNull(jsonRpcContracts);
            Assert.AreEqual(18, jsonRpcContracts.Count);

            var jsonRpcContract00 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t0p0e0d0");
            var jsonRpcContract01 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t0p0e1d0");
            var jsonRpcContract02 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t0p0e1d1");
            var jsonRpcContract03 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t0p1e0d0");
            var jsonRpcContract04 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t0p1e1d0");
            var jsonRpcContract05 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t0p1e1d1");
            var jsonRpcContract06 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t0p2e0d0");
            var jsonRpcContract07 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t0p2e1d0");
            var jsonRpcContract08 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t0p2e1d1");
            var jsonRpcContract09 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t1p0e0d0");
            var jsonRpcContract10 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t1p0e1d0");
            var jsonRpcContract11 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t1p0e1d1");
            var jsonRpcContract12 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t1p1e0d0");
            var jsonRpcContract13 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t1p1e1d0");
            var jsonRpcContract14 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t1p1e1d1");
            var jsonRpcContract15 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t1p2e0d0");
            var jsonRpcContract16 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t1p2e1d0");
            var jsonRpcContract17 = jsonRpcContracts.FirstOrDefault(c => c.Key == "t1p2e1d1");

            Assert.IsNotNull(jsonRpcContract00);
            Assert.IsNotNull(jsonRpcContract01);
            Assert.IsNotNull(jsonRpcContract02);
            Assert.IsNotNull(jsonRpcContract03);
            Assert.IsNotNull(jsonRpcContract04);
            Assert.IsNotNull(jsonRpcContract05);
            Assert.IsNotNull(jsonRpcContract06);
            Assert.IsNotNull(jsonRpcContract07);
            Assert.IsNotNull(jsonRpcContract08);
            Assert.IsNotNull(jsonRpcContract09);
            Assert.IsNotNull(jsonRpcContract10);
            Assert.IsNotNull(jsonRpcContract11);
            Assert.IsNotNull(jsonRpcContract12);
            Assert.IsNotNull(jsonRpcContract13);
            Assert.IsNotNull(jsonRpcContract14);
            Assert.IsNotNull(jsonRpcContract15);
            Assert.IsNotNull(jsonRpcContract16);
            Assert.IsNotNull(jsonRpcContract17);

            Assert.AreEqual(JsonRpcParametersType.None, jsonRpcContract00.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.None, jsonRpcContract01.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.None, jsonRpcContract02.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcContract03.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcContract04.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcContract05.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.ByName, jsonRpcContract06.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.ByName, jsonRpcContract07.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.ByName, jsonRpcContract08.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.None, jsonRpcContract09.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.None, jsonRpcContract10.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.None, jsonRpcContract11.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcContract12.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcContract13.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcContract14.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.ByName, jsonRpcContract15.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.ByName, jsonRpcContract16.Value.ParametersType);
            Assert.AreEqual(JsonRpcParametersType.ByName, jsonRpcContract17.Value.ParametersType);

            Assert.AreEqual(2, jsonRpcContract03.Value.ParametersByPosition.Count);
            Assert.AreEqual(2, jsonRpcContract04.Value.ParametersByPosition.Count);
            Assert.AreEqual(2, jsonRpcContract05.Value.ParametersByPosition.Count);
            Assert.AreEqual(2, jsonRpcContract06.Value.ParametersByName.Count);
            Assert.AreEqual(2, jsonRpcContract07.Value.ParametersByName.Count);
            Assert.AreEqual(2, jsonRpcContract08.Value.ParametersByName.Count);
            Assert.AreEqual(2, jsonRpcContract12.Value.ParametersByPosition.Count);
            Assert.AreEqual(2, jsonRpcContract13.Value.ParametersByPosition.Count);
            Assert.AreEqual(2, jsonRpcContract14.Value.ParametersByPosition.Count);
            Assert.AreEqual(2, jsonRpcContract15.Value.ParametersByName.Count);
            Assert.AreEqual(2, jsonRpcContract16.Value.ParametersByName.Count);
            Assert.AreEqual(2, jsonRpcContract17.Value.ParametersByName.Count);

            Assert.IsTrue(jsonRpcContract06.Value.ParametersByName.ContainsKey("p0"));
            Assert.IsTrue(jsonRpcContract06.Value.ParametersByName.ContainsKey("p1"));
            Assert.IsTrue(jsonRpcContract07.Value.ParametersByName.ContainsKey("p0"));
            Assert.IsTrue(jsonRpcContract07.Value.ParametersByName.ContainsKey("p1"));
            Assert.IsTrue(jsonRpcContract08.Value.ParametersByName.ContainsKey("p0"));
            Assert.IsTrue(jsonRpcContract08.Value.ParametersByName.ContainsKey("p1"));
            Assert.IsTrue(jsonRpcContract15.Value.ParametersByName.ContainsKey("p0"));
            Assert.IsTrue(jsonRpcContract15.Value.ParametersByName.ContainsKey("p1"));
            Assert.IsTrue(jsonRpcContract16.Value.ParametersByName.ContainsKey("p0"));
            Assert.IsTrue(jsonRpcContract16.Value.ParametersByName.ContainsKey("p1"));
            Assert.IsTrue(jsonRpcContract17.Value.ParametersByName.ContainsKey("p0"));
            Assert.IsTrue(jsonRpcContract17.Value.ParametersByName.ContainsKey("p1"));

            Assert.AreEqual(typeof(long), jsonRpcContract03.Value.ParametersByPosition[0]);
            Assert.AreEqual(typeof(string), jsonRpcContract03.Value.ParametersByPosition[1]);
            Assert.AreEqual(typeof(long), jsonRpcContract04.Value.ParametersByPosition[0]);
            Assert.AreEqual(typeof(string), jsonRpcContract04.Value.ParametersByPosition[1]);
            Assert.AreEqual(typeof(long), jsonRpcContract05.Value.ParametersByPosition[0]);
            Assert.AreEqual(typeof(string), jsonRpcContract05.Value.ParametersByPosition[1]);
            Assert.AreEqual(typeof(long), jsonRpcContract06.Value.ParametersByName["p0"]);
            Assert.AreEqual(typeof(string), jsonRpcContract06.Value.ParametersByName["p1"]);
            Assert.AreEqual(typeof(long), jsonRpcContract07.Value.ParametersByName["p0"]);
            Assert.AreEqual(typeof(string), jsonRpcContract07.Value.ParametersByName["p1"]);
            Assert.AreEqual(typeof(long), jsonRpcContract08.Value.ParametersByName["p0"]);
            Assert.AreEqual(typeof(string), jsonRpcContract08.Value.ParametersByName["p1"]);
            Assert.AreEqual(typeof(long), jsonRpcContract12.Value.ParametersByPosition[0]);
            Assert.AreEqual(typeof(string), jsonRpcContract12.Value.ParametersByPosition[1]);
            Assert.AreEqual(typeof(long), jsonRpcContract13.Value.ParametersByPosition[0]);
            Assert.AreEqual(typeof(string), jsonRpcContract13.Value.ParametersByPosition[1]);
            Assert.AreEqual(typeof(long), jsonRpcContract14.Value.ParametersByPosition[0]);
            Assert.AreEqual(typeof(string), jsonRpcContract14.Value.ParametersByPosition[1]);
            Assert.AreEqual(typeof(long), jsonRpcContract15.Value.ParametersByName["p0"]);
            Assert.AreEqual(typeof(string), jsonRpcContract15.Value.ParametersByName["p1"]);
            Assert.AreEqual(typeof(long), jsonRpcContract16.Value.ParametersByName["p0"]);
            Assert.AreEqual(typeof(string), jsonRpcContract16.Value.ParametersByName["p1"]);
            Assert.AreEqual(typeof(long), jsonRpcContract17.Value.ParametersByName["p0"]);
            Assert.AreEqual(typeof(string), jsonRpcContract17.Value.ParametersByName["p1"]);
        }

        [TestMethod]
        public async Task HandleAsyncT0P0E0D0()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);
            var jsonRpcRequest = new JsonRpcRequest(default, "t0p0e0d0");
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNull(jsonRpcResponse);
        }

        [TestMethod]
        public async Task HandleAsyncT0P0E1D0()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);
            var jsonRpcRequest = new JsonRpcRequest(default, "t0p0e1d0");
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsFalse(jsonRpcResponse.Success);
            Assert.AreEqual(1L, jsonRpcResponse.Error.Code);
            Assert.AreEqual("m", jsonRpcResponse.Error.Message);
            Assert.IsFalse(jsonRpcResponse.Error.HasData);
        }

        [TestMethod]
        public async Task HandleAsyncT0P0E1D1()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);
            var jsonRpcRequest = new JsonRpcRequest(default, "t0p0e1d1");
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsFalse(jsonRpcResponse.Success);
            Assert.AreEqual(1L, jsonRpcResponse.Error.Code);
            Assert.AreEqual("m", jsonRpcResponse.Error.Message);
            Assert.IsTrue(jsonRpcResponse.Error.HasData);
            Assert.AreEqual(null, jsonRpcResponse.Error.Data);
        }

        [TestMethod]
        public async Task HandleAsyncT0P1E0D0()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);

            var jsonRpcParams = new List<object>
            {
                1L,
                "!"
            };

            var jsonRpcRequest = new JsonRpcRequest(0L, "t0p1e0d0", jsonRpcParams);
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNull(jsonRpcResponse);
        }

        [TestMethod]
        public async Task HandleAsyncT0P1E1D0()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);

            var jsonRpcParams = new List<object>
            {
                1L,
                "!"
            };

            var jsonRpcRequest = new JsonRpcRequest(0L, "t0p1e1d0", jsonRpcParams);
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsFalse(jsonRpcResponse.Success);
            Assert.AreEqual(1L, jsonRpcResponse.Error.Code);
            Assert.AreEqual("m", jsonRpcResponse.Error.Message);
            Assert.IsFalse(jsonRpcResponse.Error.HasData);
        }

        [TestMethod]
        public async Task HandleAsyncT0P1E1D1()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);

            var jsonRpcParams = new List<object>
            {
                1L,
                "!"
            };

            var jsonRpcRequest = new JsonRpcRequest(0L, "t0p1e1d1", jsonRpcParams);
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsFalse(jsonRpcResponse.Success);
            Assert.AreEqual(1L, jsonRpcResponse.Error.Code);
            Assert.AreEqual("m", jsonRpcResponse.Error.Message);
            Assert.IsTrue(jsonRpcResponse.Error.HasData);
            Assert.AreEqual("1!", jsonRpcResponse.Error.Data);
        }

        [TestMethod]
        public async Task HandleAsyncT0P2E0D0()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);

            var jsonRpcParams = new Dictionary<string, object>
            {
                { "p0", 1L },
                { "p1", "!" }
            };

            var jsonRpcRequest = new JsonRpcRequest(0L, "t0p2e0d0", jsonRpcParams);
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNull(jsonRpcResponse);
        }

        [TestMethod]
        public async Task HandleAsyncT0P2E1D0()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);

            var jsonRpcParams = new Dictionary<string, object>
            {
                { "p0", 1L },
                { "p1", "!" }
            };

            var jsonRpcRequest = new JsonRpcRequest(0L, "t0p2e1d0", jsonRpcParams);
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsFalse(jsonRpcResponse.Success);
            Assert.AreEqual(1L, jsonRpcResponse.Error.Code);
            Assert.AreEqual("m", jsonRpcResponse.Error.Message);
            Assert.IsFalse(jsonRpcResponse.Error.HasData);
        }

        [TestMethod]
        public async Task HandleAsyncT0P2E1D1()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);

            var jsonRpcParams = new Dictionary<string, object>
            {
                { "p0", 1L },
                { "p1", "!" }
            };

            var jsonRpcRequest = new JsonRpcRequest(0L, "t0p2e1d1", jsonRpcParams);
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsFalse(jsonRpcResponse.Success);
            Assert.AreEqual(1L, jsonRpcResponse.Error.Code);
            Assert.AreEqual("m", jsonRpcResponse.Error.Message);
            Assert.IsTrue(jsonRpcResponse.Error.HasData);
            Assert.AreEqual("1!", jsonRpcResponse.Error.Data);
        }

        [TestMethod]
        public async Task HandleAsyncT1P0E0D0()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);
            var jsonRpcRequest = new JsonRpcRequest(default, "t1p0e0d0");
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNull(jsonRpcResponse);
        }

        [TestMethod]
        public async Task HandleAsyncT1P0E1D0()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);
            var jsonRpcRequest = new JsonRpcRequest(default, "t1p0e1d0");
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsFalse(jsonRpcResponse.Success);
            Assert.AreEqual(1L, jsonRpcResponse.Error.Code);
            Assert.AreEqual("m", jsonRpcResponse.Error.Message);
            Assert.IsFalse(jsonRpcResponse.Error.HasData);
        }

        [TestMethod]
        public async Task HandleAsyncT1P0E1D1()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);
            var jsonRpcRequest = new JsonRpcRequest(default, "t1p0e1d1");
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsFalse(jsonRpcResponse.Success);
            Assert.AreEqual(1L, jsonRpcResponse.Error.Code);
            Assert.AreEqual("m", jsonRpcResponse.Error.Message);
            Assert.IsTrue(jsonRpcResponse.Error.HasData);
            Assert.AreEqual(null, jsonRpcResponse.Error.Data);
        }

        [TestMethod]
        public async Task HandleAsyncT1P1E0D0()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);
            var jsonRpcParams = new List<object>
            {
                1L,
                "!"
            };

            var jsonRpcRequest = new JsonRpcRequest(0L, "t1p1e0d0", jsonRpcParams);
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsTrue(jsonRpcResponse.Success);
            Assert.AreEqual("1!", jsonRpcResponse.Result);
        }

        [TestMethod]
        public async Task HandleAsyncT1P1E1D0()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);

            var jsonRpcParams = new List<object>
            {
                1L,
                "!"
            };

            var jsonRpcRequest = new JsonRpcRequest(0L, "t1p1e1d0", jsonRpcParams);
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsFalse(jsonRpcResponse.Success);
            Assert.AreEqual(1L, jsonRpcResponse.Error.Code);
            Assert.AreEqual("m", jsonRpcResponse.Error.Message);
            Assert.IsFalse(jsonRpcResponse.Error.HasData);
        }

        [TestMethod]
        public async Task HandleAsyncT1P1E1D1()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);

            var jsonRpcParams = new List<object>
            {
                1L,
                "!"
            };

            var jsonRpcRequest = new JsonRpcRequest(0L, "t1p1e1d1", jsonRpcParams);
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsFalse(jsonRpcResponse.Success);
            Assert.AreEqual(1L, jsonRpcResponse.Error.Code);
            Assert.AreEqual("m", jsonRpcResponse.Error.Message);
            Assert.IsTrue(jsonRpcResponse.Error.HasData);
            Assert.AreEqual("1!", jsonRpcResponse.Error.Data);
        }

        [TestMethod]
        public async Task HandleAsyncT1P2E0D0()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);

            var jsonRpcParams = new Dictionary<string, object>
            {
                { "p0", 1L },
                { "p1", "!" }
            };

            var jsonRpcRequest = new JsonRpcRequest(0L, "t1p2e0d0", jsonRpcParams);
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsTrue(jsonRpcResponse.Success);
            Assert.AreEqual("1!", jsonRpcResponse.Result);
        }

        [TestMethod]
        public async Task HandleAsyncT1P2E1D0()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);

            var jsonRpcParams = new Dictionary<string, object>
            {
                { "p0", 1L },
                { "p1", "!" }
            };

            var jsonRpcRequest = new JsonRpcRequest(0L, "t1p2e1d0", jsonRpcParams);
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsFalse(jsonRpcResponse.Success);
            Assert.AreEqual(1L, jsonRpcResponse.Error.Code);
            Assert.AreEqual("m", jsonRpcResponse.Error.Message);
            Assert.IsFalse(jsonRpcResponse.Error.HasData);
        }

        [TestMethod]
        public async Task HandleAsyncT1P2E1D1()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestService1)))
                .Returns(null);

            var jsonRpcServiceHandler = new JsonRpcServiceHandler<JsonRpcTestService1>(serviceProviderMock.Object);

            var jsonRpcParams = new Dictionary<string, object>
            {
                { "p0", 1L },
                { "p1", "!" }
            };

            var jsonRpcRequest = new JsonRpcRequest(0L, "t1p2e1d1", jsonRpcParams);
            var jsonRpcResponse = await jsonRpcServiceHandler.HandleAsync(jsonRpcRequest);

            Assert.IsNotNull(jsonRpcResponse);
            Assert.IsFalse(jsonRpcResponse.Success);
            Assert.AreEqual(1L, jsonRpcResponse.Error.Code);
            Assert.AreEqual("m", jsonRpcResponse.Error.Message);
            Assert.IsTrue(jsonRpcResponse.Error.HasData);
            Assert.AreEqual("1!", jsonRpcResponse.Error.Data);
        }
    }
}