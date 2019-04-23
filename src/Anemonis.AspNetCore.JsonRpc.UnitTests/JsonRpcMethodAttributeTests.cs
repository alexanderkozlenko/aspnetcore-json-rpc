using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.AspNetCore.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcMethodAttributeTests
    {
        [TestMethod]
        public void ConstructorWithMethodNameWhenMathodNameIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcMethodAttribute(null));
        }

        [TestMethod]
        public void Constructor()
        {
            new JsonRpcMethodAttribute("m");
        }

        [TestMethod]
        public void ConstructorWithMethodNameAndParameterPositionsWhenMathodNameIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcMethodAttribute(null, 0));
        }

        [TestMethod]
        public void ConstructorWithMethodNameAndParameterPositions()
        {
            new JsonRpcMethodAttribute("m", 0);
        }

        [TestMethod]
        public void ConstructorWithMethodNameAndParameterNamesWhenMathodNameIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcMethodAttribute(null, "a"));
        }

        [TestMethod]
        public void ConstructorWithMethodNameAndParameterNames()
        {
            new JsonRpcMethodAttribute("m", "a");
        }
    }
}
