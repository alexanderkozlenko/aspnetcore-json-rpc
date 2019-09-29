// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Anemonis.Resources
{
    [DebuggerStepThrough]
    internal static class EmbeddedResourceManager
    {
        private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
        private static readonly string _assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        public static string GetString(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            using var resourceStream = _assembly.GetManifestResourceStream(_assemblyName + "." + name);

            if (resourceStream == null)
            {
                throw new InvalidOperationException($"The resource \"{name}\" was not found");
            }

            var buffer = new byte[resourceStream.Length];

            resourceStream.Read(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer);
        }
    }
}
