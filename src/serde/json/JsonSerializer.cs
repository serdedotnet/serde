
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Serde.Json
{
    internal sealed class KeyNotStringException : Exception { }

    public sealed partial class JsonSerializer : ISerializer
    {
        /// <summary>
        /// Serialize the given type to a string.
        /// </summary>
        public static string Serialize<T>(T s) where T : ISerialize
        {
            using var bufferWriter = new PooledByteBufferWriter(16 * 1024);
            using var writer = new Utf8JsonWriter(bufferWriter, new JsonWriterOptions
            {
                Indented = false,
                SkipValidation = true
            });
            var serializer = new JsonSerializer(writer);
            s.Serialize(serializer);
            writer.Flush();
            return Encoding.UTF8.GetString(bufferWriter.WrittenMemory.Span);
        }

        public static T Deserialize<T>(string source)
            where T : IDeserialize<T>
            => Deserialize<T, T>(source);

        public static List<T> DeserializeList<T>(string source)
            where T : IDeserialize<T>
            => Deserialize<List<T>, ListWrap.DeserializeImpl<T, T>>(source);

        public static T Deserialize<T, D>(string source)
            where D : IDeserialize<T>
        {
            var deserializer = JsonDeserializer.FromString(source);
            return D.Deserialize(ref deserializer);
        }
    }
}