using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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
            var attributes = typeInfo.GetFieldAttributes(index);
            var attribute = attributes.FirstOrDefault(it => it.AttributeType == typeof(FixedFieldInfoAttribute))
                ?? throw new InvalidOperationException($"Cannot write fixed field value without required '{nameof(FixedFieldInfoAttribute)}' annotation.");

            GetAttributeData(attribute, out int fieldOffset, out int fieldLength, out string format);

            WriteObject(value, fieldOffset, fieldLength, format);
        }

        private void WriteObject<T>(T value, int fieldOffset, int fieldLength, string format)
        {
            // We special case some types.
            if (value is bool b)
            {
                WriteBool(b, fieldOffset, fieldLength, format);
                return;
            }

            if (value is ReadOnlyMemory<byte> byteMemory)
            {
                WriteUtf8Span(byteMemory.Span, fieldOffset, fieldLength);
                return;
            }

            if (value is IFormattable formattable)
            {
                WriteText(formattable.ToString(format, CultureInfo.CurrentCulture), fieldOffset, fieldLength);
            }

            // Format string is either not used, null, or it's not a special case.
            WriteText(value?.ToString() ?? string.Empty, fieldOffset, fieldLength);
        }

        private void WriteUtf8Span(Utf8Span span, int fieldOffset, int fieldLength)
        {
            string text = Encoding.UTF8.GetString(span);
            WriteText(text, fieldOffset, fieldLength);
        }

        private void WriteBool(bool value, int fieldOffset, int fieldLength, string format)
        {
            // FixedFieldInfoAttribute ctor prevents format strings that are all white space.
            if (string.IsNullOrEmpty(format))
            {
                WriteText(value.ToString(), fieldOffset, fieldLength);
                return;
            }

            string[] splitFormat = format.Split('/', StringSplitOptions.TrimEntries);
            if (splitFormat.Length != 2)
            {
                throw new InvalidOperationException("Split format must have true and false text separated by a forward slash ('/')");
            }

            WriteText(value ? splitFormat[0] : splitFormat[1], fieldOffset, fieldLength);
        }

        private void WriteText(string value, int fieldOffset, int fieldLength)
        {
            if (_pos < fieldOffset)
            {
                // Fill in any missing space with padding.
                _writer.Write(new string(Padding, fieldOffset - _pos));
                _pos = fieldOffset;
            }

            if (value.Length > fieldLength)
            {
                throw new InvalidOperationException($"Cannot write {value} (length {value.Length}) to a field that is only {fieldLength} long.");
            }

            _writer.Write(value.PadRight(fieldLength));
        }

        public void WriteLine()
            => _writer.WriteLine();

        private static void GetAttributeData(CustomAttributeData customAttribute, out int offset, out int length, out string format)
        {
            if (!TryGetNamedArgumentValue(customAttribute, nameof(FixedFieldInfoAttribute.Offset), out offset))
            {
                offset = -1;
            }

            if (!TryGetNamedArgumentValue(customAttribute, nameof(FixedFieldInfoAttribute.Length), out length))
            {
                length = -1;
            }

            if (!TryGetNamedArgumentValue(customAttribute, nameof(FixedFieldInfoAttribute.Format), out string? formatValue))
            {
                format = string.Empty;
            }

            format = formatValue ?? string.Empty;

            static bool TryGetNamedArgumentValue<T>(CustomAttributeData customAttribute, string name, [NotNullWhen(true)] out T? value)
            {
                value = (T?)customAttribute.NamedArguments.FirstOrDefault(it => it.MemberInfo.Name == name).TypedValue.Value;
                return value is { };
            }
        }

        public override string ToString()
            => GetStringBuilder().ToString();
    }
}
