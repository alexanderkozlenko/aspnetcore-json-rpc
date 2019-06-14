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
        public static List<TypeInfo> GetDefinedTypes()
        {
            var types = new List<TypeInfo>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (var i = 0; i < assemblies.Length; i++)
            {
                var definedTypes = default(IEnumerable<TypeInfo>);

                try
                {
                    definedTypes = assemblies[i].DefinedTypes;
                }
                catch (ReflectionTypeLoadException)
                {
                    continue;
                }

                foreach (var type in definedTypes)
                {
                    if (typeof(T).IsAssignableFrom(type))
                    {
                        types.Add(type);
                    }
                }
            }

            return types;
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
