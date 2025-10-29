// Contains implementations of data interfaces for core types

using Serde.FixedWidth.Reader;
using Serde.IO;
using System.Text;

namespace Serde.FixedWidth
{
    /// <summary>
    /// Defines a type which handles deserializing a fixed-width file.
    /// </summary>
    internal sealed partial class FixedWidthDeserializer
    {
        internal readonly TReader _byteReader = byteReader;

        public static FixedWidthDeserializer<FixedWidthReader> FromString(string s)
            => FromUtf8_Unsafe(Encoding.UTF8.GetBytes(s));

        public static FixedWidthDeserializer<FixedWidthReader> FromUtf8_Unsafe(byte[] utf8Bytes)
        {
            var reader = new FixedWidthReader(utf8Bytes);
            return new FixedWidthDeserializer<FixedWidthReader>(reader);
        }
    }
}
