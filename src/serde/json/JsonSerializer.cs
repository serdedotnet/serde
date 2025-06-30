
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Serde.Json
{

    public sealed partial class JsonSerializer : ISerializer
    {
        /// <summary>
        /// Serialize the given type to a string.
        /// </summary>
        public static string Serialize<T>(T provider, ISerialize<T> ser)
        {
            using var bufferWriter = new PooledByteBufferWriter(16 * 1024);
            using var writer = new Utf8JsonWriter(bufferWriter, new JsonWriterOptions
            {
                Indented = false,
                SkipValidation = true
            });
            var serializer = new JsonSerializer(writer);
            ser.Serialize(provider, serializer);
            writer.Flush();
            return Encoding.UTF8.GetString(bufferWriter.WrittenMemory.Span);
        }

        public static string Serialize<T, TProvider>(T s)
            where TProvider : ISerializeProvider<T>
            => Serialize(s, TProvider.Instance);

        public static string Serialize<T>(T s)
            where T : ISerializeProvider<T>
            => Serialize(s, T.Instance);

        public static T Deserialize<T>(string source)
            where T : IDeserializeProvider<T>
            => Deserialize<T, T>(source);

        public static List<T> DeserializeList<T>(string source)
            where T : IDeserializeProvider<T>
            => Deserialize(source, default(List<T>).GetDeserialize());

        public static T Deserialize<T, TProvider>(string source)
            where TProvider : IDeserializeProvider<T>
            => Deserialize(source, TProvider.Instance);

        public static T Deserialize<T>(string source, IDeserialize<T> d)
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            return Deserialize_Unsafe<T>(bytes, d);
        }

        /// <summary>
        /// Deserialize from an array of UTF-8 bytes.
        /// </summary>
        public static T Deserialize<T, D>(byte[] utf8Bytes, IDeserialize<T> d)
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
            return Deserialize_Unsafe(utf8Bytes, d);
        }

        /// <summary>
        /// Assumes the input is valid UTF-8.
        /// </summary>
        private static T Deserialize_Unsafe<T>(byte[] utf8Bytes, IDeserialize<T> d)
        {
#if DEBUG
            bool expectedSuccess = ParseWithUtf8JsonReader(utf8Bytes);
#endif // DEBUG

            T result;
#if DEBUG
            try
            {
#endif // DEBUG
                using var deserializer = JsonDeserializer.FromUtf8_Unsafe(utf8Bytes);
                // The deserialize call here is async, but the input is a byte array, so there's
                // no reason to block. There should be no problem with calling GetAwaiter().GetResult().
                result = d.Deserialize(deserializer).GetAwaiter().GetResult();
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

            static bool ParseWithUtf8JsonReader(byte[] utf8Bytes)
            {
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
                return expectedSuccess;
            }
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