using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Serde.FixedWidth
{
    /// <summary>
    /// Defines a serializer for Fixed Width files.
    /// </summary>
    public sealed partial class FixedWidthSerializer(StringWriter writer) : ISerializer
    {
        public static string Serialize<T>(T provider, ISerialize<T> serde)
        {
            using var writer = new StringWriter();
            var serializer = new FixedWidthSerializer(writer);
            serde.Serialize(provider, serializer);
            writer.Flush();
            return writer.GetStringBuilder().ToString();
        }

        public static string Serialize<T, TProvider>(T serde)
            where TProvider : ISerializeProvider<T>
            => Serialize(serde, TProvider.Instance);

        public static string Serialize<T>(T serde)
            where T : ISerializeProvider<T>
            => Serialize(serde, T.Instance);

        public static T Deserialize<T>(string source)
            where T : IDeserializeProvider<T>
            => Deserialize<T, T>(source);

        public static List<T> DeserializeList<T>(string source)
            where T : IDeserializeProvider<T>
#if NET10_0_OR_GREATER
            => Deserialize(source, List<T>.Deserialize);
#else
            => Deserialize(source, ListProxy.De<T, T>.Instance);
#endif

        public static T Deserialize<T, TProvider>(string source)
            where TProvider : IDeserializeProvider<T>
            => Deserialize(source, TProvider.Instance);

        public static T Deserialize<T>(string source, IDeserialize<T> d)
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            return Deserialize_Unsafe(bytes, d);
        }

        public static T Deserialize<T>(byte[] utf8Bytes, IDeserialize<T> d)
        {
            try
            {
                // Checks for invalid utf8 as a side effect.
                _ = Encoding.UTF8.GetCharCount(utf8Bytes);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Array is not valid UTF-8", nameof(utf8Bytes), ex);
            }
            return Deserialize_Unsafe(utf8Bytes, d);
        }

        private static T Deserialize_Unsafe<T>(byte[] utf8Bytes, IDeserialize<T> d)
        {
            T result;
            using var deserializer = FixedWidthDeserializer.FromUtf8_Unsafe(utf8Bytes);
            result = d.Deserialize(deserializer);
            deserializer.EoF();
            return result;
        }
    }
}
