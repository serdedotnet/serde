using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Serde.FixedWidth
{
    /// <summary>
    /// Defines a type which handles (de)serialization of fixed-width text files.
    /// </summary>
    /// <typeparam name="T">The underlying model for the file.</typeparam>
    /// <param name="options">Options for configuring the serialization of the type.</param>
    [Obsolete("This is now handled with the serializer/deserializer")]
    public class FixedWidthSerdeObject<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(FixedWidthSerializationOptions options) : ISerde<T>
    {
        /// <inheritdoc/>
        public ISerdeInfo SerdeInfo => StringProxy.SerdeInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedWidthSerdeObject{T}"/> class.
        /// </summary>
        public FixedWidthSerdeObject() : this(FixedWidthSerializationOptions.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedWidthSerdeObject{T}"/> class.
        /// </summary>
        /// <param name="overflowHandling">Determines how to handle overflows, i.e. when the field value is longer than the field length.</param>
        public FixedWidthSerdeObject(FieldOverflowHandling overflowHandling)
            : this(new FixedWidthSerializationOptions { FieldOverflowHandling = overflowHandling })
        {
        }

        /// <inheritdoc/>
        public virtual void Serialize(T obj, ISerializer serializer)
        {
            var fieldInfo = FixedFieldInfo.FromProperties(typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)).OrderBy(it => it.Offset);
            StringBuilder sb = new();

            int index = 0;
            const char padding = ' ';
            foreach (var field in fieldInfo)
            {
                if (index <= field.Offset)
                {
                    // Fill in any missing space with padding.
                    sb.Append(padding, field.Offset - index);

                    object? value = field.Property.GetValue(obj);

                    string valueText = value is IFormattable formattable
                        ? formattable.ToString(field.Format, CultureInfo.InvariantCulture)
                        : value?.ToString() ?? string.Empty;

                    if (valueText.Length == 0)
                    {
                        // value is null or empty
                        continue;
                    }

                    if (valueText.Length > field.Length)
                    {
                        if (options.FieldOverflowHandling is FieldOverflowHandling.Throw)
                        {
                            throw new InvalidOperationException($"Value '{field.Property.Name}' ({valueText}) is too long for field! Expected: {field.Length}; Actual: {valueText.Length}");
                        }

                        // Truncate value to the maximum field length.
                        valueText = valueText[..field.Length];
                    }

                    sb.Append(valueText.PadRight(field.Length));
                    index += field.Length;
                }
            }

            serializer.WriteString(sb.ToString());
        }

        /// <inheritdoc/>
        public virtual T Deserialize(IDeserializer deserializer)
        {
            var fieldInfo = FixedFieldInfo.FromProperties(typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)).OrderBy(it => it.Offset);
            string line = deserializer.ReadString();

            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Gets information for the property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="attribute">Its attribute.</param>
    file class FixedFieldInfo(PropertyInfo property, FixedFieldInfoAttribute attribute)
    {
        /// <summary>
        /// Gets the offset for the start of the field.
        /// </summary>
        public int Offset => _attribute.Offset;

        /// <summary>
        /// Gets the max length of the field.
        /// </summary>
        public int Length => _attribute.Length;

        /// <summary>
        /// Gets the output format.
        /// </summary>
        public string Format => _attribute.Format;

        /// <summary>
        /// Gets the property described by this fixed field info.
        /// </summary>
        public PropertyInfo Property => property;

        private readonly FixedFieldInfoAttribute _attribute = attribute;

        /// <summary>
        /// Enumerates a set of FixedFieldInfos from a set of <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="properties">A set of property infos.</param>
        /// <returns>An enumerable iterating over property infos.</returns>
        public static IEnumerable<FixedFieldInfo> FromProperties(PropertyInfo[] properties)
        {
            foreach (PropertyInfo property in properties)
            {
                if (property.GetCustomAttribute<FixedFieldInfoAttribute>() is not { } attribute)
                {
                    // If not decorated with the attribute, skip.
                    continue;
                }

                yield return new(property, attribute);
            }
        }
    }
}
