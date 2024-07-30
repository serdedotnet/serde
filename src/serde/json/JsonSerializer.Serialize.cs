
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Serde.Json
{
    partial class JsonSerializer
    {
        private readonly Utf8JsonWriter _writer;
        private readonly KeySerializer _keySerializer;

        // Keeps a stack of whether or not we're writing a dictionary key
        private enum DictState
        {
            NotDictionary,
            Key,
            Value
        }
        private readonly List<DictState> _dictState = [ DictState.NotDictionary ];

        internal JsonSerializer(Utf8JsonWriter writer)
        {
            _writer = writer;
            _keySerializer = new KeySerializer(this);
        }
    }

    // Implementations of ISerializer
    partial class JsonSerializer : ISerializer
    {
        void ISerializer.SerializeBool(bool b) => _writer.WriteBooleanValue(b);

        void ISerializer.SerializeChar(char c) => SerializeString(c.ToString());

        void ISerializer.SerializeByte(byte b) => _writer.WriteNumberValue(b);

        void ISerializer.SerializeU16(ushort u16) => _writer.WriteNumberValue(u16);

        void ISerializer.SerializeU32(uint u32) => _writer.WriteNumberValue(u32);

        void ISerializer.SerializeU64(ulong u64) => _writer.WriteNumberValue(u64);

        void ISerializer.SerializeSByte(sbyte b) => _writer.WriteNumberValue(b);

        void ISerializer.SerializeI16(short i16) => _writer.WriteNumberValue(i16);

        void ISerializer.SerializeI32(int i32) => _writer.WriteNumberValue(i32);

        void ISerializer.SerializeI64(long i64) => _writer.WriteNumberValue(i64);

        void ISerializer.SerializeFloat(float f) => _writer.WriteNumberValue(f);

        void ISerializer.SerializeDouble(double d) => _writer.WriteNumberValue(d);

        void ISerializer.SerializeDecimal(decimal d) => _writer.WriteNumberValue(d);

        private void SerializeString(string s) => _writer.WriteStringValue(s);

        void ISerializer.SerializeString(string s) => SerializeString(s);
        void ISerializer.SerializeNull() => _writer.WriteNullValue();

        void ISerializer.SerializeEnumValue<T, U>(ISerdeInfo typeInfo, int index, T value, U serialize)
        {
            var valueName = typeInfo.GetFieldName(index);
            _writer.WriteStringValue(valueName);
        }

        ISerializeType ISerializer.SerializeType(ISerdeInfo typeInfo)
        {
            _writer.WriteStartObject();
            return this;
        }

        ISerializeCollection ISerializer.SerializeCollection(ISerdeInfo typeInfo, int? length)
        {
            if (typeInfo.Kind == InfoKind.Dictionary)
            {
                _dictState.Add(DictState.Key);
                _writer.WriteStartObject();
            }
            else if (typeInfo.Kind == InfoKind.Enumerable)
            {
                _writer.WriteStartArray();
            }
            else
            {
                throw new InvalidOperationException("SerializeCollection called on non-collection type");
            }
            return new CollectionImpl(this, typeInfo.Kind == InfoKind.Dictionary);
        }

        partial class CollectionImpl(JsonSerializer serializer, bool isDict) : ISerializeCollection
        {
            private bool _isValue = false;
            void ISerializeCollection.SerializeElement<T, U>(T value, U serialize)
            {
                ISerializer ser = isDict && !_isValue ? serializer._keySerializer : serializer;
                serialize.Serialize(value, ser);
                _isValue = !_isValue;
            }

            void ISerializeCollection.End(ISerdeInfo typeInfo)
            {
                if (typeInfo.Kind == InfoKind.Dictionary)
                {
                    isDict = false;
                    serializer._writer.WriteEndObject();
                }
                else if (typeInfo.Kind == InfoKind.Enumerable)
                {
                    serializer._writer.WriteEndArray();
                }
                else
                {
                    throw new InvalidOperationException("ISerializeCollection.End called on non-collection type");
                }
            }

        }

        private sealed class KeySerializer(JsonSerializer _parent) : ISerializer
        {
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

            void ISerializer.SerializeEnumValue<T, U>(ISerdeInfo typeInfo, int index, T value, U serialize)
                => throw new KeyNotStringException();

            public ISerializeCollection SerializeCollection(ISerdeInfo typeInfo, int? length) => throw new KeyNotStringException();
            public ISerializeType SerializeType(ISerdeInfo typeInfo) => throw new KeyNotStringException();
            public void SerializeNull() => throw new KeyNotStringException();
        }
    }

    partial class JsonSerializer : ISerializeCollection
    {
        void ISerializeCollection.SerializeElement<T, U>(T value, U serialize)
        {
            var currentState = _dictState[^1];
            ISerializer ser = currentState == DictState.Key ? _keySerializer : this;
            serialize.Serialize(value, ser);
            if (currentState == DictState.Key)
            {
                _dictState[^1] = DictState.Value;
            }
        }

        void ISerializeCollection.End(ISerdeInfo typeInfo)
        {
            if (typeInfo.Kind == InfoKind.Dictionary)
            {
                _dictState.RemoveAt(_dictState.Count - 1);
                _writer.WriteEndObject();
            }
            else if (typeInfo.Kind == InfoKind.Enumerable)
            {
                _writer.WriteEndArray();
            }
            else
            {
                throw new InvalidOperationException("ISerializeCollection.End called on non-collection type");
            }
        }

    }

    partial class JsonSerializer : ISerializeType
    {
        void ISerializeType.SerializeField<T, U>(ISerdeInfo typeInfo, int fieldIndex, T value, U serialize)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(fieldIndex));
            serialize.Serialize(value, this);
        }

        void ISerializeType.End()
        {
            _writer.WriteEndObject();
        }
    }
}