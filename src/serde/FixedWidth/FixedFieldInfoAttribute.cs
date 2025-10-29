using System;

namespace Serde.FixedWidth
{
    /// <summary>
    /// Decorates a field in a fixed-width file with meta information about that field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class FixedFieldInfoAttribute : Attribute
    {
        public FixedFieldInfoAttribute(int offset, int length, string format = "")
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(offset);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);
            Offset = offset;
            Length = length;
            Format = string.IsNullOrWhiteSpace(format) ? string.Empty : format;
        }

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
        public string Format { get; }
    }
}
