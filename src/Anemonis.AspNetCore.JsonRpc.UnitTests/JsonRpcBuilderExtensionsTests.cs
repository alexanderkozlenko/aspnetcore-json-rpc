using System;
using System.Threading.Tasks;

using Anemonis.AspNetCore.JsonRpc.UnitTests.TestStubs;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Anemonis.AspNetCore.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcBuilderExtensionsTests
    {
        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderAndTypeWhenBuilderIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcHandler(null, typeof(JsonRpcTestHandler2)));
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderAndTypeWhenTypeIsNull()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcHandler(builderMock.Object, null));
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderAndTypeWhenTypeIsNotClass()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcHandler(builderMock.Object, typeof(int)));
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderAndTypeWhenTypeDoesNotImplementInterface()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcHandler(builderMock.Object, typeof(object)));
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderAndType()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            builderMock.Setup(o => o.New())
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Build())
                .Returns(c => Task.CompletedTask);

            JsonRpcBuilderExtensions.UseJsonRpcHandler(builderMock.Object, typeof(JsonRpcTestHandler2));

            builderMock.Verify(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderAndTypeAndPathWhenBuilderIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcHandler(null, typeof(JsonRpcTestHandler2), default));
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderAndTypeAndPathWhenTypeIsNull()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcHandler(builderMock.Object, null, default));
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderAndTypeAndPathWhenTypeIsNotClass()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcHandler(builderMock.Object, typeof(int), default));
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderAndTypeAndPathWhenTypeDoesNotImplementInterface()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcHandler(builderMock.Object, typeof(object), default));
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderAndTypeAndPath()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            builderMock.Setup(o => o.New())
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Build())
                .Returns(c => Task.CompletedTask);

            JsonRpcBuilderExtensions.UseJsonRpcHandler(builderMock.Object, typeof(JsonRpcTestHandler2), default);

            builderMock.Verify(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderWhenBuilderIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcHandler<JsonRpcTestHandler2>(null));
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilder()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            builderMock.Setup(o => o.New())
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Build())
                .Returns(c => Task.CompletedTask);

            JsonRpcBuilderExtensions.UseJsonRpcHandler<JsonRpcTestHandler2>(builderMock.Object);

            builderMock.Verify(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderAndPathWhenBuilderIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcHandler<JsonRpcTestHandler2>(null, default));
        }

        [TestMethod]
        public void UseJsonRpcHandlerWithBuilderAndPath()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            builderMock.Setup(o => o.New())
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Build())
                .Returns(c => Task.CompletedTask);

            JsonRpcBuilderExtensions.UseJsonRpcHandler<JsonRpcTestHandler2>(builderMock.Object, default);

            builderMock.Verify(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void UseJsonRpcHandlersWhenBuilderIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcHandlers(null));
        }

        [TestMethod]
        public void UseJsonRpcHandlers()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            builderMock.Setup(o => o.New())
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Build())
                .Returns(c => Task.CompletedTask);

            JsonRpcBuilderExtensions.UseJsonRpcHandlers(builderMock.Object);
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderAndTypeWhenBuilderIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcService(null, typeof(JsonRpcTestService2)));
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderAndTypeWhenTypeIsNull()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcService(builderMock.Object, null));
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderAndTypeWhenTypeIsNotClass()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcService(builderMock.Object, typeof(int)));
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderAndTypeWhenTypeDoesNotImplementInterface()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcService(builderMock.Object, typeof(object)));
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderAndType()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            builderMock.Setup(o => o.New())
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Build())
                .Returns(c => Task.CompletedTask);

            JsonRpcBuilderExtensions.UseJsonRpcService(builderMock.Object, typeof(JsonRpcTestService2));

            builderMock.Verify(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderAndTypeAndPathWhenBuilderIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcService(null, typeof(JsonRpcTestService2), default));
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderAndTypeAndPathWhenTypeIsNull()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcService(builderMock.Object, null, default));
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderAndTypeAndPathWhenTypeIsNotClass()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcService(builderMock.Object, typeof(int), default));
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderAndTypeAndPathWhenTypeDoesNotImplementInterface()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcService(builderMock.Object, typeof(object), default));
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderAndTypeAndPath()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            builderMock.Setup(o => o.New())
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Build())
                .Returns(c => Task.CompletedTask);

            JsonRpcBuilderExtensions.UseJsonRpcService(builderMock.Object, typeof(JsonRpcTestService2), default);

            builderMock.Verify(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderWhenBuilderIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcService<JsonRpcTestService2>(null));
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilder()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            builderMock.Setup(o => o.New())
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Build())
                .Returns(c => Task.CompletedTask);

            JsonRpcBuilderExtensions.UseJsonRpcService<JsonRpcTestService2>(builderMock.Object);

            builderMock.Verify(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderAndPathWhenBuilderIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcService<JsonRpcTestService2>(null, default));
        }

        [TestMethod]
        public void UseJsonRpcServiceWithBuilderAndPath()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            builderMock.Setup(o => o.New())
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Build())
                .Returns(c => Task.CompletedTask);

            JsonRpcBuilderExtensions.UseJsonRpcService<JsonRpcTestService2>(builderMock.Object, default);

            builderMock.Verify(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void UseJsonRpcServicesWhenBuilderIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpcServices(null));
        }

        [TestMethod]
        public void UseJsonRpcServices()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            builderMock.Setup(o => o.New())
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Build())
                .Returns(c => Task.CompletedTask);

            JsonRpcBuilderExtensions.UseJsonRpcServices(builderMock.Object);
        }

        [TestMethod]
        public void UseJsonRpcWhenBuilderIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcBuilderExtensions.UseJsonRpc(null));
        }

        [TestMethod]
        public void UseJsonRpc()
        {
            var builderMock = new Mock<IApplicationBuilder>(MockBehavior.Strict);

            builderMock.Setup(o => o.New())
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Use(It.IsNotNull<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(builderMock.Object);
            builderMock.Setup(o => o.Build())
                .Returns(c => Task.CompletedTask);

            JsonRpcBuilderExtensions.UseJsonRpc(builderMock.Object);
        }
    }
}
