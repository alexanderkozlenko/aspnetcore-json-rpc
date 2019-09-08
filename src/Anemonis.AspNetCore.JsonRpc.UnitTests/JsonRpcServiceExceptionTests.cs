using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.AspNetCore.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcServiceExceptionTests
    {
        [TestMethod]
        public void GetHasErrorDataIsFalse()
        {
            var exception = new JsonRpcServiceException(0L, "m");

            Assert.AreEqual(0L, exception.Code);
            Assert.AreEqual("m", exception.Message);
            Assert.IsFalse(exception.HasErrorData);
        }

        [TestMethod]
        public void GetHasErrorDataIsTrueAndDataIsNull()
        {
            var exception = new JsonRpcServiceException(0L, "m", null);

            Assert.AreEqual(0L, exception.Code);
            Assert.AreEqual("m", exception.Message);
            Assert.IsTrue(exception.HasErrorData);
            Assert.AreEqual(null, exception.ErrorData);
        }

        [TestMethod]
        public void GetHasErrorDataIsTrueAndDataIsNotNull()
        {
            var exception = new JsonRpcServiceException(0L, "m", 1L);

            Assert.AreEqual(0L, exception.Code);
            Assert.AreEqual("m", exception.Message);
            Assert.IsTrue(exception.HasErrorData);
            Assert.AreEqual(1L, exception.ErrorData);
        }
    }
}
