// © Alexander Kozlenko. Licensed under the MIT License.

using System;

using Anemonis.JsonRpc;

namespace Anemonis.AspNetCore.JsonRpc
{
    /// <summary>Defines a JSON-RPC method contract.</summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class JsonRpcMethodAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="JsonRpcMethodAttribute" /> class.</summary>
        /// <param name="methodName">The name of a JSON-RPC method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="methodName" /> is <see langword="null" />.</exception>
        public JsonRpcMethodAttribute(string methodName)
        {
            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            MethodName = methodName;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcMethodAttribute" /> class.</summary>
        /// <param name="methodName">The name of a JSON-RPC method.</param>
        /// <param name="parameterPositions">The corresponding positions of the JSON-RPC method parameters for the type method parameters.</param>
        /// <exception cref="ArgumentNullException"><paramref name="methodName" /> or <paramref name="parameterPositions" /> is <see langword="null" />.</exception>
        public JsonRpcMethodAttribute(string methodName, params int[] parameterPositions)
            : this(methodName)
        {
            if (parameterPositions == null)
            {
                throw new ArgumentNullException(nameof(parameterPositions));
            }

            ParameterPositions = parameterPositions;
            ParametersType = JsonRpcParametersType.ByPosition;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcMethodAttribute" /> class.</summary>
        /// <param name="methodName">The name of a JSON-RPC method.</param>
        /// <param name="parameterNames">The corresponding names of the JSON-RPC method parameters for the type method parameters.</param>
        /// <exception cref="ArgumentNullException"><paramref name="methodName" /> or <paramref name="parameterNames" /> is <see langword="null" />.</exception>
        public JsonRpcMethodAttribute(string methodName, params string[] parameterNames)
            : this(methodName)
        {
            if (parameterNames == null)
            {
                throw new ArgumentNullException(nameof(parameterNames));
            }

            ParameterNames = parameterNames;
            ParametersType = JsonRpcParametersType.ByName;
        }

        internal string MethodName
        {
            get;
        }

        internal JsonRpcParametersType ParametersType
        {
            get;
        }

        internal int[] ParameterPositions
        {
            get;
        }

        internal string[] ParameterNames
        {
            get;
        }
    }
}
