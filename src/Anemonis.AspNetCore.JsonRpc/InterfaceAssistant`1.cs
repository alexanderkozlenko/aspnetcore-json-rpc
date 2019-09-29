// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

using Anemonis.AspNetCore.JsonRpc.Resources;

namespace Anemonis.AspNetCore.JsonRpc
{
    internal static class InterfaceAssistant<T>
        where T : class
    {
        public static List<Type> GetDefinedTypes()
        {
            var definedTypes = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (var i = 0; i < assemblies.Length; i++)
            {
                try
                {
                    var assemblyTypes = assemblies[i].GetTypes();

                    for (var j = 0; j < assemblyTypes.Length; j++)
                    {
                        if (typeof(T).IsAssignableFrom(assemblyTypes[j]))
                        {
                            definedTypes.Add(assemblyTypes[j]);
                        }
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                }
            }

            return definedTypes;
        }

        public static void VerifyTypeParam(Type param, string paramName)
        {
            if (!param.IsClass)
            {
                throw new ArgumentException(Strings.GetString("infrastructure.type_isnt_class"), paramName);
            }
            if (!typeof(T).IsAssignableFrom(param))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.GetString("infrastructure.type_doesnt_implement_interface"), typeof(T)), paramName);
            }
        }
    }
}
