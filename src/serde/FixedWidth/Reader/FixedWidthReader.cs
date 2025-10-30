using Serde.IO;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace Serde.FixedWidth.Reader
{
    internal struct FixedWidthReader(string line)
    {
        private const char padding = ' ';
        private readonly string _line = line;
        private int _pos = 0;

        public string ReadString(ISerdeInfo typeInfo, int index)
            => GetText(typeInfo, index, out _).ToString();

        public bool ReadBool(ISerdeInfo typeInfo, int index)
        {
            var span = GetText(typeInfo, index, out var attribute);

            if (string.IsNullOrEmpty(attribute.Format))
            {
                return bool.Parse(span);
            }

            string[] splitFormat = attribute.Format.Split('/', StringSplitOptions.TrimEntries);
            if (splitFormat.Length != 2)
            {
                throw new InvalidOperationException("Split format must be an empty string or have true and false text separated by a forward slash ('/')");
            }

            if (span.Equals(splitFormat[0], StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (span.Equals(splitFormat[1], StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else
            {
                throw new InvalidOperationException($"Value '{span}' was neither '{splitFormat[0]}' nor '{splitFormat[1]}'.");
            }
        }

        public char ReadChar(ISerdeInfo typeInfo, int index)
        {
            var span = GetText(typeInfo, index, out var attribute);

            return span.Length == 1 ? span[0] : throw new InvalidOperationException("Char field comprised of multiple non-space characters.");
        }

        public DateTime ReadDateTime(ISerdeInfo typeInfo, int index)
        {
            var span = GetText(typeInfo, index, out var attribute);

            return string.IsNullOrEmpty(attribute.Format)
                ? DateTime.Parse(span)
                : DateTime.ParseExact(span, attribute.Format, CultureInfo.CurrentCulture);
        }

        public TNumber ReadNumber<TNumber>(ISerdeInfo typeInfo, int index, NumberStyles numberStyles)
            where TNumber : struct, INumber<TNumber>
        {
            var span = GetText(typeInfo, index, out _);
            
            var trimmedValue = span.Trim();

            if (trimmedValue.IsEmpty)
            {
                return TNumber.Zero;
            }

            return TNumber.Parse(trimmedValue, numberStyles, CultureInfo.InvariantCulture);
        }

        private ReadOnlySpan<char> GetText(ISerdeInfo typeInfo, int index, out FixedFieldInfoAttribute attribute)
        {
            var customAttribute = typeInfo.GetFieldAttributes(index).FirstOrDefault(it => it.AttributeType == typeof(FixedFieldInfoAttribute));
            attribute = FixedFieldInfoAttribute.FromCustomAttributeData(customAttribute);

            _pos = attribute.Offset + attribute.Length;

            return _line.AsSpan(attribute.Offset, attribute.Length);
        }
    }
}
