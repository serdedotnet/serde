using System;
using System.Collections.Generic;
using System.Linq;
using Serde.FixedWidth.Writer;

namespace Serde.FixedWidth
{
    /// <summary>
    /// Defines a serializer for Fixed Width files.
    /// </summary>
    public sealed partial class FixedWidthSerializer(FixedWidthWriter writer)
    {
        /// <summary>
        /// Serializes a single line of a fixed-width text file.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="value"/>.</typeparam>
        /// <param name="value">The object instance to serialize.</param>
        /// <param name="serde">A type which can serialize.</param>
        /// <returns>A fixed-width string.</returns>
        public static string Serialize<T>(T value, ISerialize<T> serde)
        {
            using var writer = new FixedWidthWriter();
            var serializer = new FixedWidthSerializer(writer);
            serde.Serialize(value, serializer);
            writer.Flush();
            return writer.GetStringBuilder().ToString();
        }

        /// <inheritdoc cref="Serialize{T}(T, ISerialize{T})"/>
        /// <typeparam name="T"/>
        /// <typeparam name="TProvider">The serialize provider..</typeparam>
        public static string Serialize<T, TProvider>(T value)
            where TProvider : ISerializeProvider<T>
            => Serialize(value, TProvider.Instance);

        /// <inheritdoc cref="Serialize{T}(T, ISerialize{T})"/>
        public static string Serialize<T>(T value)
            where T : ISerializeProvider<T>
            => Serialize(value, T.Instance);

        /// <inheritdoc cref="SerializeDocument{T}(IEnumerable{T}, ISerialize{T})"/>
        public static IEnumerable<string> SerializeDocument<T>(IEnumerable<T> values)
            where T : ISerializeProvider<T>
            => SerializeDocument(values, T.Instance);

        /// <summary>
        /// Serializes a collection of <typeparamref name="T"/>, returning an enumerable for writing to a stream.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="values">A collection of the items to serialize.</param>
        /// <param name="serde">The serialize provider.</param>
        /// <returns>An enumerable of the rows of the document.</returns>
        public static IEnumerable<string> SerializeDocument<T>(IEnumerable<T> values, ISerialize<T> serde)
            where T : ISerializeProvider<T>
        {
            foreach (var provider in values)
            {
                yield return Serialize(provider, serde);
            }
        }

        /// <summary>
        /// Deserializes a single line of a fixed-width text file.
        /// </summary>
        /// <typeparam name="T">The type of the value to return.</typeparam>
        /// <param name="source">The line to deserialize.</param>
        /// <param name="d">A type which can deerialize.</param>
        /// <returns>A fixed-width string.</returns>
        public static T Deserialize<T>(string source, IDeserialize<T> d)
        {
            T result;
            var deserializer = new FixedWidthDeserializer(source);
            result = d.Deserialize(deserializer);
            return result;
        }

        /// <inheritdoc cref="Deserialize{T}(string, IDeserialize{T})"/>
        public static T Deserialize<T>(string source)
            where T : IDeserializeProvider<T>
            => Deserialize<T, T>(source);

        /// <inheritdoc cref="Deserialize{T}(string, IDeserialize{T})"/>
        /// <typeparam name="T" />
        /// <typeparam name="TProvider">The deserialize provider to use.</typeparam>
        public static T Deserialize<T, TProvider>(string source)
            where TProvider : IDeserializeProvider<T>
            => Deserialize(source, TProvider.Instance);

        /// <inheritdoc cref="DeserializeDocument{T}(string, IDeserialize{T}, int)"/>
        public static IEnumerable<T> DeserializeDocument<T>(string document, int headerLines = 0)
            where T : IDeserializeProvider<T>
            => DeserializeDocument(document, T.Instance, headerLines);

        /// <inheritdoc cref="DeserializeDocument{T}(string, IDeserialize{T}, int)"/>
        public static IEnumerable<T> DeserializeDocument<T, TProvider>(string document, int headerLines = 0)
            where TProvider : IDeserializeProvider<T>
            => DeserializeDocument(document, TProvider.Instance, headerLines);

        /// <summary>
        /// Deserializes the provided document.
        /// </summary>
        /// <typeparam name="T">The type of the value to return.</typeparam>
        /// <param name="document">The document to deserialize.</param>
        /// <param name="d">The deserializer.</param>
        /// <param name="headerLines">The number of lines to skip.</param>
        /// <returns>An enumerable of deserialized rows.</returns>
        public static IEnumerable<T> DeserializeDocument<T>(string document, IDeserialize<T> d, int headerLines = 0)
        {
            foreach (var line in document.Split(Environment.NewLine).Skip(headerLines))
            {
                yield return Deserialize(line, d);
            }
        }
    }
}
