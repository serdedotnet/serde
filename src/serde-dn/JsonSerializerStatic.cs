
using System;
using System.Text;
using System.Text.Json;

namespace Serde.Json
{
    // Using a mutable struct allows for an efficient low-allocation implementation of the
    // ISerializer interface, but mutable structs are easy to misuse in C#, so hide the
    // implementation for now.
    internal partial struct JsonSerializerStatic
    {
        internal readonly Utf8JsonWriter _writer;
        public JsonSerializerStatic(Utf8JsonWriter writer)
        {
            _writer = writer;
        }
    }

    // Implementations of ISerializerStatic
    partial struct JsonSerializerStatic : ISerializerStatic<SerializeTypeStatic, SerializeEnumerableStatic, SerializeDictionaryStatic>
    {
        public void Serialize(bool b) => _writer.WriteBooleanValue(b);

        public void Serialize(char c) => Serialize(c.ToString());

        public void Serialize(byte b) => _writer.WriteNumberValue(b);

        public void Serialize(ushort u16) => _writer.WriteNumberValue(u16);

        public void Serialize(uint u32) => _writer.WriteNumberValue(u32);

        public void Serialize(ulong u64) => _writer.WriteNumberValue(u64);

        public void Serialize(sbyte b) => _writer.WriteNumberValue(b);

        public void Serialize(short i16) => _writer.WriteNumberValue(i16);

        public void Serialize(int i32) => _writer.WriteNumberValue(i32);

        public void Serialize(long i64) => _writer.WriteNumberValue(i64);

        public void Serialize(float f) => _writer.WriteNumberValue(f);

        public void Serialize(double d) => _writer.WriteNumberValue(d);

        public void Serialize(string s) => _writer.WriteStringValue(s);

        public SerializeTypeStatic SerializeType(string name, int numFields)
        {
            _writer.WriteStartObject();
            return new SerializeTypeStatic(ref this);
        }

        public SerializeEnumerableStatic SerializeEnumerable(int? count)
        {
            _writer.WriteStartArray();
            return new SerializeEnumerableStatic(ref this);
        }

        public SerializeDictionaryStatic SerializeDictionary(int? count)
        {
            _writer.WriteStartObject();
            return new SerializeDictionaryStatic(ref this);
        }

        ISerializeType ISerializer.SerializeType(string name, int numFields)
            => SerializeType(name, numFields);

        ISerializeEnumerable ISerializer.SerializeEnumerable(int? length)
            => SerializeEnumerable(length);

        ISerializeDictionary ISerializer.SerializeDictionary(int? length)
            => SerializeDictionary(length);
    }

    internal struct SerializeTypeStatic : ISerializeTypeStatic
    {
        private JsonSerializerStatic _impl;
        public SerializeTypeStatic(ref JsonSerializerStatic impl)
        {
            // Copies Impl, since we can't hold a ref. This forces the persistant state to be a
            // reference type, but that works for now.
            _impl = impl;
        }

        void ISerializeTypeStatic.SerializeField<T>(string name, T value)
        {
            _impl._writer.WritePropertyName(name);
            value.Serialize<JsonSerializerStatic, SerializeTypeStatic, SerializeEnumerableStatic, SerializeDictionaryStatic>(ref _impl);
        }

        void ISerializeType.SerializeField<T>(string name, T value)
        {
            _impl._writer.WritePropertyName(name);
            value.Serialize(_impl);
        }

        public void End()
        {
            _impl._writer.WriteEndObject();
        }
    }

    struct SerializeEnumerableStatic : ISerializeEnumerableStatic
    {
        private JsonSerializerStatic _impl;
        public SerializeEnumerableStatic(ref JsonSerializerStatic impl)
        {
            // Copies Impl, since we can't hold a ref. This forces the persistant state to be a
            // reference type, but that works for now.
            _impl = impl;
        }

        void ISerializeEnumerableStatic.SerializeElement<T>(T value)
        {
            value.Serialize<JsonSerializerStatic, SerializeTypeStatic, SerializeEnumerableStatic, SerializeDictionaryStatic>(ref _impl);
        }

        void ISerializeEnumerable.SerializeElement<T>(T value)
        {
            value.Serialize(_impl);
        }

        void ISerializeEnumerable.End()
        {
            _impl._writer.WriteEndArray();
        }
    }

    struct SerializeDictionaryStatic : ISerializeDictionaryStatic
    {
        private JsonSerializerStatic _impl;
        public SerializeDictionaryStatic(ref JsonSerializerStatic impl)
        {
            // Copies Impl, since we can't hold a ref. This forces the persistant state to be a
            // reference type, but that works for now.
            _impl = impl;
        }
        void ISerializeDictionaryStatic.SerializeKey<T>(T key)
        {
            // Grab a string value
            var keySerializer = new KeySerializer();
            key.Serialize(keySerializer);
            _impl._writer.WritePropertyName(keySerializer.StringResult!);
        }
        void ISerializeDictionaryStatic.SerializeValue<T>(T value)
        {
            value.Serialize<JsonSerializerStatic, SerializeTypeStatic, SerializeEnumerableStatic, SerializeDictionaryStatic>(ref _impl);
        }
        public void End()
        {
            _impl._writer.WriteEndObject();
        }

        // Share implementation with JsonSerializer
        public static void SerializeKey<T>(ref JsonSerializerStatic impl, T key) where T : ISerialize
        {
            // Grab a string value. Box to prevent internal copying and losing side-effects
            ISerializer keySerializer = new KeySerializer();
            key.Serialize(keySerializer);
            impl._writer.WritePropertyName(((KeySerializer)keySerializer).StringResult!);
        }

        void ISerializeDictionary.SerializeKey<T>(T key)
        {
            SerializeKey(ref _impl, key);
        }

        void ISerializeDictionary.SerializeValue<T>(T value)
        {
            value.Serialize(_impl);
        }

        private struct KeySerializer : ISerializer, ISerializerStatic<ISerializeTypeStatic, ISerializeEnumerableStatic, ISerializeDictionaryStatic>
        {
            public string? StringResult;
            public KeySerializer(int dummy)
            {
                StringResult = null;
            }

            public void Serialize(bool b) => throw new KeyNotStringException();
            public void Serialize(char c) => throw new KeyNotStringException();
            public void Serialize(byte b) => throw new KeyNotStringException();
            public void Serialize(ushort u16) => throw new KeyNotStringException();

            public void Serialize(uint u32) => throw new KeyNotStringException();

            public void Serialize(ulong u64) => throw new KeyNotStringException();

            public void Serialize(sbyte b) => throw new KeyNotStringException();

            public void Serialize(short i16) => throw new KeyNotStringException();

            public void Serialize(int i32) => throw new KeyNotStringException();

            public void Serialize(long i64) => throw new KeyNotStringException();

            public void Serialize(float f) => throw new KeyNotStringException();

            public void Serialize(double d) => throw new KeyNotStringException();

            public void Serialize(string s)
            {
                StringResult = s;
            }

            public ISerializeDictionaryStatic SerializeDictionary(int? length) => throw new KeyNotStringException();

            public ISerializeEnumerableStatic SerializeEnumerable(int? length) => throw new KeyNotStringException();

            public ISerializeTypeStatic SerializeType(string name, int numFields) => throw new KeyNotStringException();

            ISerializeDictionary ISerializer.SerializeDictionary(int? length)
                => SerializeDictionary(length);

            ISerializeEnumerable ISerializer.SerializeEnumerable(int? length)
                => SerializeEnumerable(length);

            ISerializeType ISerializer.SerializeType(string name, int numFields)
                => SerializeType(name, numFields);
        }
    }
}