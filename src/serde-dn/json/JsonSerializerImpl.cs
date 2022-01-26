
using System;
using System.Text;
using System.Text.Json;

namespace Serde.Json
{
    // Using a mutable struct allows for an efficient low-allocation implementation of the
    // ISerializer interface, but mutable structs are easy to misuse in C#, so hide the
    // implementation for now.
    internal partial struct JsonSerializerImpl
    {
        internal readonly Utf8JsonWriter _writer;
        public JsonSerializerImpl(Utf8JsonWriter writer)
        {
            _writer = writer;
        }
    }

    // Implementations of ISerializer
    partial struct JsonSerializerImpl : ISerializer<SerializeType, SerializeEnumerable, SerializeDictionary>
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

        public void SerializeString(string s) => _writer.WriteStringValue(s);
        public void SerializeNull() => _writer.WriteNullValue();

        public void SerializeNotNull<T>(T t) where T : notnull, ISerialize
        {
            t.Serialize<JsonSerializerImpl, SerializeType, SerializeEnumerable, SerializeDictionary>(ref this);
        }

        public SerializeType SerializeType(string name, int numFields)
        {
            _writer.WriteStartObject();
            return new SerializeType(ref this);
        }

        public SerializeEnumerable SerializeEnumerable(int? count)
        {
            _writer.WriteStartArray();
            return new SerializeEnumerable(ref this);
        }

        public SerializeDictionary SerializeDictionary(int? count)
        {
            _writer.WriteStartObject();
            return new SerializeDictionary(ref this);
        }
    }

    internal struct SerializeType : ISerializeType
    {
        private JsonSerializerImpl _impl;
        public SerializeType(ref JsonSerializerImpl impl)
        {
            // Copies Impl, since we can't hold a ref. This forces the persistant state to be a
            // reference type, but that works for now.
            _impl = impl;
        }

        void ISerializeType.SerializeField<T>(string name, T value)
        {
            _impl._writer.WritePropertyName(name);
            value.Serialize<JsonSerializerImpl, SerializeType, SerializeEnumerable, SerializeDictionary>(ref _impl);
        }

        public void End()
        {
            _impl._writer.WriteEndObject();
        }
    }

    struct SerializeEnumerable : ISerializeEnumerable
    {
        private JsonSerializerImpl _impl;
        public SerializeEnumerable(ref JsonSerializerImpl impl)
        {
            // Copies Impl, since we can't hold a ref. This forces the persistant state to be a
            // reference type, but that works for now.
            _impl = impl;
        }

        void ISerializeEnumerable.SerializeElement<T>(T value)
        {
            value.Serialize<JsonSerializerImpl, SerializeType, SerializeEnumerable, SerializeDictionary>(ref _impl);
        }

        void ISerializeEnumerable.End()
        {
            _impl._writer.WriteEndArray();
        }
    }

    struct SerializeDictionary : ISerializeDictionary
    {
        private JsonSerializerImpl _impl;
        public SerializeDictionary(ref JsonSerializerImpl impl)
        {
            // Copies Impl, since we can't hold a ref. This forces the persistant state to be a
            // reference type, but that works for now.
            _impl = impl;
        }
        void ISerializeDictionary.SerializeValue<T>(T value)
        {
            value.Serialize<JsonSerializerImpl, SerializeType, SerializeEnumerable, SerializeDictionary>(ref _impl);
        }
        public void End()
        {
            _impl._writer.WriteEndObject();
        }

        void ISerializeDictionary.SerializeKey<T>(T key)
        {
            // Grab a string value. Box to prevent internal copying and losing side-effects
            ISerializer<ISerializeType, ISerializeEnumerable, ISerializeDictionary> keySerializer = new KeySerializer();
            key.Serialize<
                ISerializer<ISerializeType, ISerializeEnumerable, ISerializeDictionary>,
                ISerializeType,
                ISerializeEnumerable,
                ISerializeDictionary>(ref keySerializer);
            _impl._writer.WritePropertyName(((KeySerializer)keySerializer).StringResult!);
        }

        private struct KeySerializer : ISerializer<ISerializeType, ISerializeEnumerable, ISerializeDictionary>
        {
            public string? StringResult;
            public KeySerializer(int dummy)
            {
                StringResult = null;
            }

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

            public void SerializeString(string s)
            {
                StringResult = s;
            }

            public ISerializeDictionary SerializeDictionary(int? length) => throw new KeyNotStringException();

            public ISerializeEnumerable SerializeEnumerable(int? length) => throw new KeyNotStringException();

            public ISerializeType SerializeType(string name, int numFields) => throw new KeyNotStringException();

            public void SerializeNull() => throw new KeyNotStringException();

            public void SerializeNotNull<T>(T t) where T : notnull, ISerialize => throw new KeyNotStringException();
        }
    }
}