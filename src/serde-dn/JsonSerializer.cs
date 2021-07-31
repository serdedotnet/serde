
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
            var serializer = new JsonSerializer(writer);
            s.Serialize(serializer);
            writer.Flush();
            return Encoding.UTF8.GetString(bufferWriter.WrittenMemory.Span);
        }

        /// <summary>
        /// Serialize the given type to a string.
        /// </summary>
        public static string SerializeStatic<T>(T s) where T : ISerializeStatic
        {
            using var bufferWriter = new PooledByteBufferWriter(16 * 1024);
            using var writer = new Utf8JsonWriter(bufferWriter);
            var serializer = new JsonSerializerStatic(writer);
            s.Serialize<JsonSerializerStatic, SerializeTypeStatic, SerializeEnumerableStatic, SerializeDictionaryStatic>(ref serializer);
            writer.Flush();
            return Encoding.UTF8.GetString(bufferWriter.WrittenMemory.Span);
        }
    }

    // Implementations of ISerializer interfaces
    partial class JsonSerializer : ISerializer
    {
        private JsonSerializerStatic _impl;
        internal JsonSerializer(Utf8JsonWriter writer)
        {
            _impl = new JsonSerializerStatic(writer);
        }

        public ISerializeType SerializeType(string name, int numFields)
        {
            _impl._writer.WriteStartObject();
            return this;
        }

        public ISerializeEnumerable SerializeEnumerable(int? count)
        {
            _impl._writer.WriteStartArray();
            return this;
        }

        public ISerializeDictionary SerializeDictionary(int? count)
        {
            _impl._writer.WriteStartObject();
            return this;
        }

        public void Serialize(bool b) => _impl.Serialize(b);

        public void Serialize(char c) => _impl.Serialize(c);

        public void Serialize(byte b) => _impl.Serialize(b);

        public void Serialize(ushort u16) => _impl.Serialize(u16);

        public void Serialize(uint u32) => _impl.Serialize(u32);

        public void Serialize(ulong u64) => _impl.Serialize(u64);

        public void Serialize(sbyte b) => _impl.Serialize(b);

        public void Serialize(short i16) => _impl.Serialize(i16);

        public void Serialize(int i32) => _impl.Serialize(i32);

        public void Serialize(long i64) => _impl.Serialize(i64);

        public void Serialize(float f) => _impl.Serialize(f);

        public void Serialize(double d) => _impl.Serialize(d);

        public void Serialize(string s) => _impl.Serialize(s);
    }

    partial class JsonSerializer : ISerializeType
    {
        void ISerializeType.SerializeField<T>(string name, T value)
        {
            _impl._writer.WritePropertyName(name);
            value.Serialize(this);
        }

        void ISerializeType.End()
        {
            _impl._writer.WriteEndObject();
        }
    }

    partial class JsonSerializer : ISerializeEnumerable
    {
        void ISerializeEnumerable.SerializeElement<T>(T value)
        {
            value.Serialize(this);
        }

        void ISerializeEnumerable.End()
        {
            _impl._writer.WriteEndArray();
        }
    }

    partial class JsonSerializer : ISerializeDictionary
    {
        void ISerializeDictionary.SerializeKey<T>(T key)
        {
            SerializeDictionaryStatic.SerializeKey(ref _impl, key);
        }

        void ISerializeDictionary.SerializeValue<T>(T value)
        {
            value.Serialize(this);
        }

        void ISerializeDictionary.End()
        {
            _impl._writer.WriteEndObject();
        }
    }
}