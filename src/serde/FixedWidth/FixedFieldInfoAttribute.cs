using System;
using System.Collections.Generic;
using System.Text;

namespace Serde.FixedWidth
{
    /// <summary>
    /// Decorates a field in a fixed-width file with meta information about that field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class FixedFieldInfoAttribute(int offset, int length, string format = "") : Attribute
    {
        /// <summary>
        /// Gets the offset that indicates where the field begins.
        /// </summary>
        public int Offset => offset;

        /// <summary>
        /// Gets the length of the field.
        /// </summary>
        public int Length => length;

        /// <summary>
        /// Gets the format string for providing to <c>TryParseExact</c>.
        /// </summary>
        public string Format => format;
    }
}
