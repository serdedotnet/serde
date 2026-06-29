using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Serde.Json
{
    public sealed partial class JsonSerializer : ISerializer
    {
        /// <summary>
        /// Serialize the given type as UTF-8 bytes and return them as a contiguous block of memory.
        /// </summary>
        public static ReadOnlyMemory<byte> ToBytes<T>(T provider, ISerialize<T> ser)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using var writer = new Utf8JsonWriter(
                bufferWriter,
                new JsonWriterOptions { Indented = false, SkipValidation = true }
            );
            var serializer = new JsonSerializer(writer);
            ser.Serialize(provider, serializer);
            writer.Flush();
            return bufferWriter.WrittenMemory;
        }

        public static ReadOnlyMemory<byte> ToBytes<T, TProvider>(T s)
            where TProvider : ISerializeProvider<T> => ToBytes(s, TProvider.Instance);

        public static ReadOnlyMemory<byte> ToBytes<T>(T s)
            where T : ISerializeProvider<T> => ToBytes(s, T.Instance);

        /// <summary>
        /// Serialize the given type to a string.
        /// </summary>
        public static string Serialize<T>(T provider, ISerialize<T> ser)
        {
            using var bufferWriter = new PooledByteBufferWriter(16 * 1024);
            using var writer = new Utf8JsonWriter(
                bufferWriter,
                new JsonWriterOptions { Indented = false, SkipValidation = true }
            );
            var serializer = new JsonSerializer(writer);
            ser.Serialize(provider, serializer);
            writer.Flush();
            return Encoding.UTF8.GetString(bufferWriter.WrittenMemory.Span);
        }

        public static string Serialize<T, TProvider>(T s)
            where TProvider : ISerializeProvider<T> => Serialize(s, TProvider.Instance);

        public static string Serialize<T>(T s)
            where T : ISerializeProvider<T> => Serialize(s, T.Instance);

        public static T Deserialize<T>(string source)
            where T : IDeserializeProvider<T> => Deserialize<T, T>(source);

        public static List<T> DeserializeList<T>(string source)
            where T : IDeserializeProvider<T>
#if NET10_0_OR_GREATER
            => Deserialize(source, List<T>.Deserialize);
#else
            => Deserialize(source, ListProxy.De<T, T>.Instance);
#endif

        public static T Deserialize<T, TProvider>(string source)
            where TProvider : IDeserializeProvider<T> => Deserialize(source, TProvider.Instance);

        public static T Deserialize<T>(string source, IDeserialize<T> d)
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            return Deserialize_Unsafe<T>(bytes, d);
        }

        /// <summary>
        /// Deserialize from an array of UTF-8 bytes.
        /// </summary>
        public static T Deserialize<T>(ReadOnlyMemory<byte> utf8Bytes, IDeserialize<T> d)
        {
            try
            {
                // Checks for invalid UTF-8 as a side effect.
                _ = Encoding.UTF8.GetCharCount(utf8Bytes.Span);
            }
            catch
            {
                throw new ArgumentException("Array is not valid UTF-8", nameof(utf8Bytes));
            }
            return Deserialize_Unsafe(utf8Bytes, d);
        }

        /// <summary>
        /// Assumes the input is valid UTF-8.
        /// </summary>
        private static T Deserialize_Unsafe<T>(ReadOnlyMemory<byte> utf8Bytes, IDeserialize<T> d)
        {
#if DEBUG
            var reader = new System.Text.Json.Utf8JsonReader(utf8Bytes.Span);
            bool expectedSuccess = true;
            try
            {
                while (reader.Read())
                    ;
            }
            catch (System.Text.Json.JsonException)
            {
                expectedSuccess = false;
            }
#endif // DEBUG

            T result;
#if DEBUG
            try
            {
#endif // DEBUG
                using var deserializer = JsonDeserializer.FromUtf8_Unsafe(utf8Bytes);
                result = d.Deserialize(deserializer);
                deserializer.Eof();
#if DEBUG
            }
            catch (Serde.Json.JsonException)
            {
                //Debug.Assert(!expectedSuccess, "Utf8JsonReader suceeded, but Serde.Json failed");
                throw;
            }
            Debug.Assert(expectedSuccess, "Utf8JsonReader failed, but Serde.Json suceeded");
#endif // DEBUG
            return result;
        }

        public static JsonValue DeserializeJsonValue(string source)
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            using var deserializer = JsonDeserializer.FromUtf8_Unsafe(bytes);
            var result = deserializer.ReadJsonValue();
            deserializer.Eof();
            return result;
        }
    }
}
