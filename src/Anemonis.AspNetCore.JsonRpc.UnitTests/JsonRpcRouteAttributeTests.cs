using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.AspNetCore.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcRouteAttributeTests
    {
        [TestMethod]
        public void ConstructorWithPathWhenPathIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcRouteAttribute(null));
        }

        [TestMethod]
        public void ConstructorWithPath()
        {
            new JsonRpcRouteAttribute("/api");
        }
    }
}