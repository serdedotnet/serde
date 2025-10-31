using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Serde.FixedWidth
{
    /// <summary>
    /// Decorates a field in a fixed-width file with meta information about that field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class FixedFieldInfoAttribute : Attribute
    {
        /// <summary>
        /// Gets the offset that indicates where the field begins.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Gets the length of the field.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the format string for providing to <c>TryParseExact</c>.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="string.Empty"/>.
        /// </remarks>
        public string Format { get; }

        /// <summary>
        /// Gets a value indicating how to handle field overflows.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="FieldOverflowHandling.Throw"/>.
        /// </remarks>
        public FieldOverflowHandling OverflowHandling { get; }

        public FixedFieldInfoAttribute(int offset, int length, string format = "", FieldOverflowHandling overflowHandling = FieldOverflowHandling.Throw)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(offset);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);
            Offset = offset;
            Length = length;
            Format = string.IsNullOrWhiteSpace(format) ? string.Empty : format;
            OverflowHandling = overflowHandling;
        }

        public static FixedFieldInfoAttribute FromCustomAttributeData(CustomAttributeData? customAttribute)
        {
            if (!TryGetFixedFieldInfoAttribute(customAttribute, out var attribute))
            {
                throw new InvalidOperationException($"Cannot write fixed field value without required '{nameof(FixedFieldInfoAttribute)}' annotation.");
            }

            return attribute;
        }

        public static bool TryGetFixedFieldInfoAttribute(CustomAttributeData? customAttribute, [NotNullWhen(true)] out FixedFieldInfoAttribute? fixedFieldInfoAttribute)
        {
            fixedFieldInfoAttribute = null;

            if (customAttribute is null)
            {
                return false;
            }

            string format;

            if (!TryGetNamedArgumentValue(customAttribute, 0, out int offset))
            {
                return false;
            }

            if (!TryGetNamedArgumentValue(customAttribute, 1, out int length))
            {
                return false;
            }

            if (!TryGetNamedArgumentValue(customAttribute, 2, out string? formatValue))
            {
                format = string.Empty;
            }
            format = formatValue ?? string.Empty;

            if (!TryGetNamedArgumentValue(customAttribute, 3, out FieldOverflowHandling fieldOverflowHandling))
            {
                fieldOverflowHandling = FieldOverflowHandling.Throw;
            }

            fixedFieldInfoAttribute = new(offset, length, format, fieldOverflowHandling);
            return true;

            static bool TryGetNamedArgumentValue<T>(CustomAttributeData customAttribute, int argumentIndex, [NotNullWhen(true)] out T? value)
            {
                value = (T?)customAttribute.ConstructorArguments[argumentIndex].Value;
                return value is { };
            }
        }
    }
}
