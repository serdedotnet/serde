
using System;
using System.Text;
using System.Text.Json;

namespace Serde.Json
{
    public sealed class KeyNotStringException : Exception { }

    public sealed partial class JsonSerializer
    {
        /// <summary>
        /// Serialize the given type to a string.
        /// </summary>
        public static string Serialize<T>(T s) where T : ISerialize
        {
            using var bufferWriter = new PooledByteBufferWriter(16 * 1024);
            using var writer = new Utf8JsonWriter(bufferWriter);
            var serializer = new JsonSerializerImpl(writer);
            s.Serialize<JsonSerializerImpl, SerializeType, SerializeEnumerable, SerializeDictionary>(ref serializer);
            writer.Flush();
            return Encoding.UTF8.GetString(bufferWriter.WrittenMemory.Span);
        }

        public static T Deserialize<T>(string source)
            where T : IDeserialize<T>
            => Deserialize<T, T>(source);

        public static T Deserialize<T, D>(string source)
            where D : IDeserialize<T>
        {
            var deserializer = JsonDeserializer.FromString(source);
            return D.Deserialize(ref deserializer);
        }
    }
}