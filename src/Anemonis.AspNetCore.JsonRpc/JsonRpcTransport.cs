// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;

namespace Anemonis.AspNetCore.JsonRpc
{
    internal static class JsonRpcTransport
    {
        public const string MediaType = "application/json";

        public static readonly string Charset = Encoding.UTF8.WebName;
        public static readonly IReadOnlyDictionary<string, Encoding> CharsetEncodings = CreateCharsetEncodingsDictionary();

        private static IReadOnlyDictionary<string, Encoding> CreateCharsetEncodingsDictionary()
        {
            return new Dictionary<string, Encoding>(StringComparer.OrdinalIgnoreCase)
            {
                [Encoding.UTF8.WebName] = new UTF8Encoding(false, true),
                [Encoding.Unicode.WebName] = new UnicodeEncoding(false, false, true),
                [Encoding.UTF32.WebName] = new UTF32Encoding(false, false, true)
            };
        }
    }
}
