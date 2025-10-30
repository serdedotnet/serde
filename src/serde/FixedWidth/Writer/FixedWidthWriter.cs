using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Serde.FixedWidth.Writer
{
    public sealed class FixedWidthWriter : IDisposable
    {
        private const char Padding = ' ';
        private readonly StringWriter _writer = new();
        private int _pos = 0;

        public void Flush()
        {
            _writer.Flush();
        }

        public void Dispose()
        {
            Flush();
        }

        /// <inheritdoc cref="StringWriter.GetStringBuilder"/>
        public StringBuilder GetStringBuilder() => _writer.GetStringBuilder();

        public void WriteObject<T>(ISerdeInfo typeInfo, int index, T value)
        {
            var customAttribute = typeInfo.GetFieldAttributes(index).FirstOrDefault(it => it.AttributeType == typeof(FixedFieldInfoAttribute));
            var attribute = FixedFieldInfoAttribute.FromCustomAttributeData(customAttribute);

            WriteObject(value, attribute.Offset, attribute.Length, attribute.Format, attribute.OverflowHandling);
        }

        private void WriteObject<T>(T value, int fieldOffset, int fieldLength, string format, FieldOverflowHandling overflowHandling)
        {
            // We special case some types.
            if (value is bool b)
            {
                WriteBool(b, fieldOffset, fieldLength, format, overflowHandling);
                return;
            }

            if (value is ReadOnlyMemory<byte> byteMemory)
            {
                WriteUtf8Span(byteMemory.Span, fieldOffset, fieldLength, overflowHandling);
                return;
            }

            if (value is IFormattable formattable)
            {
                WriteText(formattable.ToString(format, CultureInfo.CurrentCulture), fieldOffset, fieldLength, overflowHandling);
            }

            // Format string is either not used, null, or it's not a special case.
            WriteText(value?.ToString() ?? string.Empty, fieldOffset, fieldLength, overflowHandling);
        }

        private void WriteUtf8Span(Utf8Span span, int fieldOffset, int fieldLength, FieldOverflowHandling overflowHandling)
        {
            string text = Encoding.UTF8.GetString(span);
            WriteText(text, fieldOffset, fieldLength, overflowHandling);
        }

        private void WriteBool(bool value, int fieldOffset, int fieldLength, string format, FieldOverflowHandling overflowHandling)
        {
            // FixedFieldInfoAttribute ctor prevents format strings that are all white space.
            if (string.IsNullOrEmpty(format))
            {
                WriteText(value.ToString(), fieldOffset, fieldLength, overflowHandling);
                return;
            }

            string[] splitFormat = format.Split('/', StringSplitOptions.TrimEntries);
            if (splitFormat.Length != 2)
            {
                throw new InvalidOperationException("Split format must be an empty string or have true and false text separated by a forward slash ('/')");
            }

            WriteText(value ? splitFormat[0] : splitFormat[1], fieldOffset, fieldLength, overflowHandling);
        }

        private void WriteText(string value, int fieldOffset, int fieldLength, FieldOverflowHandling overflowHandling)
        {
            if (_pos < fieldOffset)
            {
                // Fill in any missing space with padding.
                _writer.Write(new string(Padding, fieldOffset - _pos));
                _pos = fieldOffset;
            }

            if (value.Length > fieldLength)
            {
                value = overflowHandling switch
                {
                    FieldOverflowHandling.Throw => throw new InvalidOperationException($"Cannot write {value} (length {value.Length}) to a field that is only {fieldLength} long."),
                    FieldOverflowHandling.Truncate => value[..fieldLength],
                    _ => throw new ArgumentOutOfRangeException(nameof(overflowHandling), $"{overflowHandling} is not a valid value for {nameof(FieldOverflowHandling)}.")
                };                
            }

            _writer.Write(value.PadRight(fieldLength));
        }

        public void WriteLine()
            => _writer.WriteLine();

        public override string ToString()
            => GetStringBuilder().ToString();
    }
}
