// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.IO;
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

            using var bufferStream = new MemoryStream((int)resourceStream.Length);

            resourceStream.CopyTo(bufferStream);

            return Encoding.UTF8.GetString(bufferStream.ToArray());
        }
    }
}
