using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Anemonis.AspNetCore.JsonRpc.UnitTests.TestStubs;
using Anemonis.Resources;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json.Linq;

#pragma warning disable IDE0067

namespace Anemonis.AspNetCore.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcMiddlewareTests
    {
        [TestMethod]
        public void ConstructorWithServicesWhenServicesIsNull()
        {
            var environmentMock = new Mock<IWebHostEnvironment>(MockBehavior.Strict);
            var loggerMock = new Mock<ILogger<JsonRpcMiddleware<JsonRpcTestHandler1>>>(MockBehavior.Loose);

            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcMiddleware<JsonRpcTestHandler1>(null, environmentMock.Object, loggerMock.Object));
        }

        [TestMethod]
        public void ConstructorWithServicesWhenEnvironmentIsNull()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
            var loggerMock = new Mock<ILogger<JsonRpcMiddleware<JsonRpcTestHandler1>>>(MockBehavior.Loose);

            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcMiddleware<JsonRpcTestHandler1>(serviceProviderMock.Object, null, loggerMock.Object));
        }

        [TestMethod]
        public void ConstructorWithServicesWhenLoggerIsNull()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
            var environmentMock = new Mock<IWebHostEnvironment>(MockBehavior.Strict);

            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcMiddleware<JsonRpcTestHandler1>(serviceProviderMock.Object, environmentMock.Object, null));
        }

        [TestMethod]
        public void Dispose()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);
            var environmentMock = new Mock<IWebHostEnvironment>(MockBehavior.Strict);
            var loggerMock = new Mock<ILogger<JsonRpcMiddleware<JsonRpcTestHandler1>>>(MockBehavior.Loose);
            var jsonRpcHandler = new JsonRpcTestHandler1();
            var jsonRpcHandlerDisposed = false;

            jsonRpcHandler.Disposed += (sender, e) => jsonRpcHandlerDisposed = true;

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestHandler1)))
                .Returns(jsonRpcHandler);
            serviceProviderMock.Setup(o => o.GetService(typeof(IOptions<JsonRpcOptions>)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(ILoggerFactory)))
                .Returns(null);

            var jsonRpcMiddleware = new JsonRpcMiddleware<JsonRpcTestHandler1>(serviceProviderMock.Object, environmentMock.Object, loggerMock.Object);

            jsonRpcMiddleware.Dispose();

            Assert.IsTrue(jsonRpcHandlerDisposed);
        }

        [DataTestMethod]
        [DataRow("CONNECT")]
        [DataRow("DELETE")]
        [DataRow("GET")]
        [DataRow("HEAD")]
        [DataRow("OPTIONS")]
        [DataRow("PATCH")]
        [DataRow("PUT")]
        [DataRow("TRACE")]
        public async Task InvokeAsyncWhenHttpMethodIsInvalid(string method)
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestHandler1)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(IOptions<JsonRpcOptions>)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(ILoggerFactory)))
                .Returns(null);

            var environmentMock = new Mock<IWebHostEnvironment>(MockBehavior.Strict);
            var loggerMock = new Mock<ILogger<JsonRpcMiddleware<JsonRpcTestHandler1>>>(MockBehavior.Loose);
            var jsonRpcMiddleware = new JsonRpcMiddleware<JsonRpcTestHandler1>(serviceProviderMock.Object, environmentMock.Object, loggerMock.Object);
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Method = method;
            httpContext.Request.ContentType = "application/json; charset=utf-8";

            await jsonRpcMiddleware.InvokeAsync(httpContext, c => Task.CompletedTask);

            Assert.AreEqual(StatusCodes.Status405MethodNotAllowed, httpContext.Response.StatusCode);
        }

        [DataTestMethod]
        [DataRow("application/json; charset=us-ascii")]
        [DataRow("application/json; charset=utf")]
        [DataRow("application/x-www-form-urlencoded")]
        [DataRow("application/xml")]
        [DataRow("multipart/form-data")]
        [DataRow("text/plain")]
        public async Task InvokeAsyncWhenContentTypeHeaderIsInvalid(string mediaType)
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestHandler1)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(IOptions<JsonRpcOptions>)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(ILoggerFactory)))
                .Returns(null);

            var environmentMock = new Mock<IWebHostEnvironment>(MockBehavior.Strict);
            var loggerMock = new Mock<ILogger<JsonRpcMiddleware<JsonRpcTestHandler1>>>(MockBehavior.Loose);
            var jsonRpcMiddleware = new JsonRpcMiddleware<JsonRpcTestHandler1>(serviceProviderMock.Object, environmentMock.Object, loggerMock.Object);
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Method = HttpMethods.Post;
            httpContext.Request.ContentType = mediaType;
            httpContext.Request.Headers.Add(HeaderNames.Accept, "application/json; charset=utf-8");

            await jsonRpcMiddleware.InvokeAsync(httpContext, c => Task.CompletedTask);

            Assert.AreEqual(StatusCodes.Status415UnsupportedMediaType, httpContext.Response.StatusCode);
        }

        [TestMethod]
        public async Task InvokeAsyncWhenContentEncodingHeaderIsSpecified()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestHandler1)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(IOptions<JsonRpcOptions>)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(ILoggerFactory)))
                .Returns(null);

            var environmentMock = new Mock<IWebHostEnvironment>(MockBehavior.Strict);
            var loggerMock = new Mock<ILogger<JsonRpcMiddleware<JsonRpcTestHandler1>>>(MockBehavior.Loose);
            var jsonRpcMiddleware = new JsonRpcMiddleware<JsonRpcTestHandler1>(serviceProviderMock.Object, environmentMock.Object, loggerMock.Object);
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Method = HttpMethods.Post;
            httpContext.Request.ContentType = "application/json; charset=utf-8";
            httpContext.Request.Headers.Add(HeaderNames.ContentEncoding, "deflate");

            await jsonRpcMiddleware.InvokeAsync(httpContext, c => Task.CompletedTask);

            Assert.AreEqual(StatusCodes.Status415UnsupportedMediaType, httpContext.Response.StatusCode);
        }

        [TestMethod]
        public async Task InvokeAsyncWhenAcceptHeaderIsNotSpecified()
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestHandler1)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(IOptions<JsonRpcOptions>)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(ILoggerFactory)))
                .Returns(null);

            var environmentMock = new Mock<IWebHostEnvironment>(MockBehavior.Strict);
            var loggerMock = new Mock<ILogger<JsonRpcMiddleware<JsonRpcTestHandler1>>>(MockBehavior.Loose);
            var jsonRpcMiddleware = new JsonRpcMiddleware<JsonRpcTestHandler1>(serviceProviderMock.Object, environmentMock.Object, loggerMock.Object);
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Method = HttpMethods.Post;
            httpContext.Request.ContentType = "application/json; charset=utf-8";

            await jsonRpcMiddleware.InvokeAsync(httpContext, c => Task.CompletedTask);

            Assert.AreEqual(StatusCodes.Status406NotAcceptable, httpContext.Response.StatusCode);
        }

        [DataTestMethod]
        [DataRow("application/json; charset=us-ascii")]
        [DataRow("application/json; charset=utf")]
        [DataRow("application/x-www-form-urlencoded")]
        [DataRow("application/xml")]
        [DataRow("multipart/form-data")]
        [DataRow("text/plain")]
        public async Task InvokeAsyncWhenAcceptTypeHeaderIsInvalid(string mediaType)
        {
            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestHandler1)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(IOptions<JsonRpcOptions>)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(ILoggerFactory)))
                .Returns(null);

            var environmentMock = new Mock<IWebHostEnvironment>(MockBehavior.Strict);
            var loggerMock = new Mock<ILogger<JsonRpcMiddleware<JsonRpcTestHandler1>>>(MockBehavior.Loose);
            var jsonRpcMiddleware = new JsonRpcMiddleware<JsonRpcTestHandler1>(serviceProviderMock.Object, environmentMock.Object, loggerMock.Object);
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Method = HttpMethods.Post;
            httpContext.Request.ContentType = "application/json; charset=utf-8";
            httpContext.Request.Headers.Add(HeaderNames.Accept, mediaType);

            await jsonRpcMiddleware.InvokeAsync(httpContext, c => Task.CompletedTask);

            Assert.AreEqual(StatusCodes.Status406NotAcceptable, httpContext.Response.StatusCode);
        }

        [DataTestMethod]
        [DataRow("b0t0p0e0d0")]
        [DataRow("b0t0p0e1d0")]
        [DataRow("b0t0p0e1d1")]
        [DataRow("b0t0p1e0d0")]
        [DataRow("b0t0p1e1d0")]
        [DataRow("b0t0p1e1d1")]
        [DataRow("b0t0p2e0d0")]
        [DataRow("b0t0p2e1d0")]
        [DataRow("b0t0p2e1d1")]
        [DataRow("b0t1p0e0d0")]
        [DataRow("b0t1p0e1d0")]
        [DataRow("b0t1p0e1d1")]
        [DataRow("b0t1p1e0d0")]
        [DataRow("b0t1p1e1d0")]
        [DataRow("b0t1p1e1d1")]
        [DataRow("b0t1p2e0d0")]
        [DataRow("b0t1p2e1d0")]
        [DataRow("b0t1p2e1d1")]
        [DataRow("b0ie")]
        [DataRow("b0iu")]
        [DataRow("b0is")]
        [DataRow("b0it")]
        [DataRow("b1t0p0e0d0")]
        [DataRow("b1t0p0e1d0")]
        [DataRow("b1t0p0e1d1")]
        [DataRow("b1t0p1e0d0")]
        [DataRow("b1t0p1e1d0")]
        [DataRow("b1t0p1e1d1")]
        [DataRow("b1t0p2e0d0")]
        [DataRow("b1t0p2e1d0")]
        [DataRow("b1t0p2e1d1")]
        [DataRow("b1t1p0e0d0")]
        [DataRow("b1t1p0e1d0")]
        [DataRow("b1t1p0e1d1")]
        [DataRow("b1t1p1e0d0")]
        [DataRow("b1t1p1e1d0")]
        [DataRow("b1t1p1e1d1")]
        [DataRow("b1t1p2e0d0")]
        [DataRow("b1t1p2e1d0")]
        [DataRow("b1t1p2e1d1")]
        [DataRow("b1iu")]
        [DataRow("b1is")]
        [DataRow("b1it")]
        public async Task InvokeAsync(string code)
        {
            var requestActualContent = EmbeddedResourceManager.GetString($"Assets.{code}_req.json");
            var responseExpectedContent = EmbeddedResourceManager.GetString($"Assets.{code}_res.json");
            var responseExpectedStatusCode = !string.IsNullOrEmpty(responseExpectedContent) ? StatusCodes.Status200OK : StatusCodes.Status204NoContent;

            var serviceProviderMock = new Mock<IServiceProvider>(MockBehavior.Strict);

            serviceProviderMock.Setup(o => o.GetService(typeof(JsonRpcTestHandler1)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(IOptions<JsonRpcOptions>)))
                .Returns(null);
            serviceProviderMock.Setup(o => o.GetService(typeof(ILoggerFactory)))
                .Returns(null);

            var environmentMock = new Mock<IWebHostEnvironment>(MockBehavior.Strict);

            environmentMock
                .Setup(o => o.EnvironmentName)
                .Returns(Environments.Production);

            var loggerMock = new Mock<ILogger<JsonRpcMiddleware<JsonRpcTestHandler1>>>(MockBehavior.Loose);

            loggerMock
                .Setup(o => o.IsEnabled(It.IsAny<LogLevel>()))
                .Returns(true);

            var jsonRpcMiddleware = new JsonRpcMiddleware<JsonRpcTestHandler1>(serviceProviderMock.Object, environmentMock.Object, loggerMock.Object);
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Method = HttpMethods.Post;
            httpContext.Request.ContentType = "application/json; charset=utf-8";
            httpContext.Request.Headers.Add(HeaderNames.Accept, "application/json; charset=utf-8");
            httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestActualContent));
            httpContext.Response.Body = new MemoryStream();

            await jsonRpcMiddleware.InvokeAsync(httpContext, c => Task.CompletedTask);

            Assert.AreEqual(responseExpectedStatusCode, httpContext.Response.StatusCode);

            if (responseExpectedStatusCode == StatusCodes.Status200OK)
            {
                var responseActualContent = default(string);

                httpContext.Response.Body.Position = 0;

                using (var reader = new StreamReader(httpContext.Response.Body))
                {
                    responseActualContent = await reader.ReadToEndAsync();
                }

                Assert.IsFalse(string.IsNullOrEmpty(responseActualContent), "Actual response content is empty");

                var responseActualContentToken = JToken.Parse(responseActualContent);
                var responseExpectedContentToken = JToken.Parse(responseExpectedContent);

                Assert.IsTrue(JToken.DeepEquals(responseExpectedContentToken, responseActualContentToken), "Actual JSON token differs from expected");
            }
        }
    }
}
