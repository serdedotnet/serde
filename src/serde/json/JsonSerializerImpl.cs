
using System;
using System.Text;
using System.Text.Json;

namespace Serde.Json
{
    // Using a mutable struct allows for an efficient low-allocation implementation of the
    // ISerializer interface, but mutable structs are easy to misuse in C#, so hide the
    // implementation for now.
    partial class JsonSerializer
    {
        private readonly Utf8JsonWriter _writer;
        internal JsonSerializer(Utf8JsonWriter writer)
        {
            _writer = writer;
        }
    }

    // Implementations of ISerializer
    partial class JsonSerializer : ISerializer
    {
        public void SerializeBool(bool b) => _writer.WriteBooleanValue(b);

        public void SerializeChar(char c) => SerializeString(c.ToString());

        public void SerializeByte(byte b) => _writer.WriteNumberValue(b);

        public void SerializeU16(ushort u16) => _writer.WriteNumberValue(u16);

        public void SerializeU32(uint u32) => _writer.WriteNumberValue(u32);

        public void SerializeU64(ulong u64) => _writer.WriteNumberValue(u64);

        public void SerializeSByte(sbyte b) => _writer.WriteNumberValue(b);

        public void SerializeI16(short i16) => _writer.WriteNumberValue(i16);

        public void SerializeI32(int i32) => _writer.WriteNumberValue(i32);

        public void SerializeI64(long i64) => _writer.WriteNumberValue(i64);

        public void SerializeFloat(float f) => _writer.WriteNumberValue(f);

        public void SerializeDouble(double d) => _writer.WriteNumberValue(d);

        public void SerializeDecimal(decimal d) => _writer.WriteNumberValue(d);

        public void SerializeString(string s) => _writer.WriteStringValue(s);
        public void SerializeNull() => _writer.WriteNullValue();

        public void SerializeNotNull<T>(T t) where T : notnull, ISerialize
        {
            t.Serialize(this);
        }

        public void SerializeNotNull<T, U>(T t, U u)
            where T : notnull
            where U : ISerialize<T>
        {
            u.Serialize(t, this);
        }

        public void SerializeEnumValue<T>(string enumName, string? valueName, T value) where T : notnull, ISerialize
        {
            if (valueName is null)
            {
                throw new InvalidOperationException($"Cannot serialize unnamed enum value '{value}' of enum '{enumName}'");
            }
            _writer.WriteStringValue(valueName);
        }

        public void SerializeEnumValue<T, U>(string enumName, string? valueName, T value, U serialize)
            where T : struct, Enum
            where U : ISerialize<T>
        {
            if (valueName is null)
            {
                throw new InvalidOperationException($"Cannot serialize unnamed enum value '{value}' of enum '{enumName}'");
            }
            _writer.WriteStringValue(valueName);
        }

        public ISerializeType SerializeType(string name, int numFields)
        {
            _writer.WriteStartObject();
            return this;
        }

        public ISerializeEnumerable SerializeEnumerable(string typeName, int? count)
        {
            _writer.WriteStartArray();
            return new SerializeEnumerableImpl(this);
        }

        private sealed class SerializeEnumerableImpl : ISerializeEnumerable
        {
            private readonly JsonSerializer _s;
            public SerializeEnumerableImpl(JsonSerializer s) { _s = s; }
            void ISerializeEnumerable.SerializeElement<T>(T value)
            {
                value.Serialize(_s);
            }
            void ISerializeEnumerable.SerializeElement<T, U>(T value, U serialize)
                => serialize.Serialize(value, _s);

            void ISerializeEnumerable.End()
            {
                _s._writer.WriteEndArray();
            }
        }

        public ISerializeDictionary SerializeDictionary(int? count)
        {
            _writer.WriteStartObject();
            return this;
        }
    }

    partial class JsonSerializer : ISerializeType
    {
        void ISerializeType.SerializeField<T>(Utf8Span name, T value)
        {
            _writer.WritePropertyName(name);
            value.Serialize(this);
        }

        void ISerializeType.SerializeField<T, U>(string name, T value, U serialize)
        {
            _writer.WritePropertyName(name);
            serialize.Serialize(value, this);
        }

        void ISerializeType.End()
        {
            _writer.WriteEndObject();
        }
    }

    partial class JsonSerializer : ISerializeDictionary
    {
        void ISerializeDictionary.SerializeValue<T>(T value)
        {
            value.Serialize(this);
        }

        void ISerializeDictionary.SerializeValue<T, U>(T value, U serialize)
            => serialize.Serialize(value, this);

        void ISerializeDictionary.End()
        {
            _writer.WriteEndObject();
        }

        void ISerializeDictionary.SerializeKey<T>(T key)
        {
            key.Serialize(new KeySerializer(this));
        }

        void ISerializeDictionary.SerializeKey<T, U>(T key, U serialize)
        {
            serialize.Serialize(key, new KeySerializer(this));
        }

        private readonly struct KeySerializer : ISerializer
        {
            private readonly JsonSerializer _parent;
            public KeySerializer(JsonSerializer parent) => this._parent = parent;

            public void SerializeBool(bool b) => throw new KeyNotStringException();
            public void SerializeChar(char c) => throw new KeyNotStringException();
            public void SerializeByte(byte b) => throw new KeyNotStringException();
            public void SerializeU16(ushort u16) => throw new KeyNotStringException();

            public void SerializeU32(uint u32) => throw new KeyNotStringException();

            public void SerializeU64(ulong u64) => throw new KeyNotStringException();

            public void SerializeSByte(sbyte b) => throw new KeyNotStringException();

            public void SerializeI16(short i16) => throw new KeyNotStringException();

            public void SerializeI32(int i32) => throw new KeyNotStringException();

            public void SerializeI64(long i64) => throw new KeyNotStringException();

            public void SerializeFloat(float f) => throw new KeyNotStringException();

            public void SerializeDouble(double d) => throw new KeyNotStringException();

            public void SerializeDecimal(decimal d) => throw new KeyNotStringException();

            public void SerializeString(string s)
            {
                _parent._writer.WritePropertyName(s);
            }

            public void SerializeEnumValue<T>(string enumName, string? valueName, T value) where T : notnull, ISerialize
                => throw new KeyNotStringException();
            void ISerializer.SerializeEnumValue<T, U>(string enumName, string? valueName, T value, U serialize)
                => throw new KeyNotStringException();

            public ISerializeDictionary SerializeDictionary(int? length) => throw new KeyNotStringException();
            public ISerializeEnumerable SerializeEnumerable(string typeName, int? length) => throw new KeyNotStringException();
            public ISerializeType SerializeType(string name, int numFields) => throw new KeyNotStringException();
            public void SerializeNull() => throw new KeyNotStringException();
            public void SerializeNotNull<T>(T t) where T : notnull, ISerialize => throw new KeyNotStringException();
            void ISerializer.SerializeNotNull<T, U>(T t, U u) => throw new KeyNotStringException();
        }
    }
}