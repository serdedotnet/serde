
using System;
using System.Text;
using System.Text.Json;

namespace Serde
{
    public static partial class JsonSerializer
    {
        /// <summary>
        /// Serialize the given type to a string.
        /// </summary>
        public static string Serialize<T>(T s) where T : ISerialize
        {
            using var bufferWriter = new PooledByteBufferWriter(16 * 1024);
            using var writer = new Utf8JsonWriter(bufferWriter);
            var serializer = new Impl(writer);
            s.Serialize<Impl, SerializeType, SerializeEnumerable, SerializeDictionary>(ref serializer);
            writer.Flush();
            return Encoding.UTF8.GetString(bufferWriter.WrittenMemory.Span);
        }

        // Using a mutable struct allows for an efficient low-allocation implementation of the
        // ISerializer interface, but mutable structs are easy to misuse in C#, so hide the
        // implementation for now.
        private partial struct Impl
        {
            public readonly Utf8JsonWriter _writer;
            public Impl(Utf8JsonWriter writer)
            {
                _writer = writer;
            }
        }
    }

    // Implementations of ISerializer interfaces
    partial class JsonSerializer
    {
        partial struct Impl : ISerializer<SerializeType, SerializeEnumerable, SerializeDictionary>
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

        struct SerializeType : ISerializeType
        {
            private Impl _impl;
            public SerializeType(ref Impl impl)
            {
                // Copies Impl, since we can't hold a ref. This forces the persistant state to be a
                // reference type, but that works for now.
                _impl = impl;
            }

            public void SerializeField<T>(string name, T value)
                where T : ISerialize
            {
                _impl._writer.WritePropertyName(name);
                value.Serialize<Impl, SerializeType, SerializeEnumerable, SerializeDictionary>(ref _impl);
            }

            public void End()
            {
                _impl._writer.WriteEndObject();
            }
        }

        struct SerializeEnumerable : ISerializeEnumerable
        {
            private Impl _impl;
            public SerializeEnumerable(ref Impl impl)
            {
                // Copies Impl, since we can't hold a ref. This forces the persistant state to be a
                // reference type, but that works for now.
                _impl = impl;
            }
            void ISerializeEnumerable.SerializeElement<T>(T value)
            {
                value.Serialize<Impl, SerializeType, SerializeEnumerable, SerializeDictionary>(ref _impl);
            }

            void ISerializeEnumerable.End()
            {
                _impl._writer.WriteEndArray();
            }
        }

        struct SerializeDictionary : ISerializeDictionary
        {
            private Impl _impl;
            public SerializeDictionary(ref Impl impl)
            {
                // Copies Impl, since we can't hold a ref. This forces the persistant state to be a
                // reference type, but that works for now.
                _impl = impl;
            }
            void ISerializeDictionary.SerializeKey<T>(T key)
            {
                // Grab a string value
                var keySerializer = new KeySerializer();
                key.Serialize<KeySerializer, ISerializeType, ISerializeEnumerable, ISerializeDictionary>(ref keySerializer);
                _impl._writer.WritePropertyName(keySerializer.StringResult!);
            }
            void ISerializeDictionary.SerializeValue<T>(T value)
            {
                value.Serialize<Impl, SerializeType, SerializeEnumerable, SerializeDictionary>(ref _impl);
            }
            void ISerializeDictionary.End()
            {
                _impl._writer.WriteEndObject();
            }
        }

        public sealed class KeyNotStringException : Exception { }

        struct KeySerializer : ISerializer<ISerializeType, ISerializeEnumerable, ISerializeDictionary>
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

            public ISerializeDictionary SerializeDictionary(int? length) => throw new KeyNotStringException();

            public ISerializeEnumerable SerializeEnumerable(int? length) => throw new KeyNotStringException();

            public ISerializeType SerializeType(string name, int numFields) => throw new KeyNotStringException();
        }
    }
}