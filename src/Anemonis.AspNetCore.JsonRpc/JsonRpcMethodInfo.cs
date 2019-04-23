// © Alexander Kozlenko. Licensed under the MIT License.

using System.Reflection;

namespace Anemonis.AspNetCore.JsonRpc
{
    internal sealed class JsonRpcMethodInfo
    {
        public JsonRpcMethodInfo(MethodInfo method)
        {
            Method = method;
        }

        public JsonRpcMethodInfo(MethodInfo method, int[] parameterPositions)
            : this(method)
        {
            ParameterPositions = parameterPositions;
        }

        public JsonRpcMethodInfo(MethodInfo method, string[] parameterNames)
            : this(method)
        {
            ParameterNames = parameterNames;
        }

        public MethodInfo Method
        {
            get;
        }

        public int[] ParameterPositions
        {
            get;
        }

        public string[] ParameterNames
        {
            get;
        }
    }
}
