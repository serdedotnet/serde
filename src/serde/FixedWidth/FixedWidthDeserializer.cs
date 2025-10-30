// Contains implementations of data interfaces for core types

using Serde.FixedWidth.Reader;
using Serde.IO;
using System.Text;

namespace Serde.FixedWidth
{
    /// <summary>
    /// Defines a type which handles deserializing a fixed-width file.
    /// </summary>
    internal sealed partial class FixedWidthDeserializer(string document)
    {
        private readonly FixedWidthReader _reader = new(document);
    }
}
