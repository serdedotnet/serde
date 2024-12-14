
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
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
        public static string Serialize<T>(T provider) where T : ISerializeProvider<T>
        {
            using var bufferWriter = new PooledByteBufferWriter(16 * 1024);
            using var writer = new Utf8JsonWriter(bufferWriter, new JsonWriterOptions
            {
                Indented = false,
                SkipValidation = true
            });
            var serializer = new JsonSerializer(writer);
            T.SerializeInstance.Serialize(provider, serializer);
            writer.Flush();
            return Encoding.UTF8.GetString(bufferWriter.WrittenMemory.Span);
        }

        public static string Serialize<T, TProxy>(T s, TProxy proxy)
            where TProxy : ISerialize<T>
        {
            using var bufferWriter = new PooledByteBufferWriter(16 * 1024);
            using var writer = new Utf8JsonWriter(bufferWriter, new JsonWriterOptions
            {
                Indented = false,
                SkipValidation = true
            });
            var serializer = new JsonSerializer(writer);
            proxy.Serialize(s, serializer);
            writer.Flush();
            return Encoding.UTF8.GetString(bufferWriter.WrittenMemory.Span);
        }

        public static string Serialize<T, TProvider>(T s)
            where TProvider : ISerialize<T>, ISerializeProvider<T>
            => Serialize(s, TProvider.SerializeInstance);

        public static T Deserialize<T>(string source)
            where T : IDeserializeProvider<T>
            => Deserialize<T, IDeserialize<T>>(source, default(T).GetDeserialize());

        public static List<T> DeserializeList<T>(string source)
            where T : IDeserializeProvider<T>
            => Deserialize<List<T>, ListProxy.Deserialize<T, T>>(source, default(List<T>).GetDeserialize());

        public static T Deserialize<T, TProvider>(string source)
            where TProvider : IDeserializeProvider<T>
            => Deserialize<T, IDeserialize<T>>(source, TProvider.DeserializeInstance);

        public static T Deserialize<T, D>(string source, D d)
            where D : IDeserialize<T>
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            return Deserialize_Unsafe<T, D>(bytes, d);
        }

        /// <summary>
        /// Deserialize from an array of UTF-8 bytes.
        /// </summary>
        public static T Deserialize<T, D>(byte[] utf8Bytes, D d)
            where D : IDeserialize<T>
        {
            try
            {
                // Checks for invalid UTF-8 as a side effect.
                _ = Encoding.UTF8.GetCharCount(utf8Bytes);
            }
            catch
            {
                throw new ArgumentException("Array is not valid UTF-8", nameof(utf8Bytes));
            }
            return Deserialize_Unsafe<T, D>(utf8Bytes, d);
        }

        /// <summary>
        /// Assumes the input is valid UTF-8.
        /// </summary>
        private static T Deserialize_Unsafe<T, D>(byte[] utf8Bytes, D d)
            where D : IDeserialize<T>
        {
#if DEBUG
            var reader = new System.Text.Json.Utf8JsonReader(utf8Bytes);
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
    }
}