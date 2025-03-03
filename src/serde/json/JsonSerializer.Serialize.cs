
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
        private readonly EnumSerializer _enumSerializer;

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
            _enumSerializer = new EnumSerializer(this);
        }
    }

    // Implementations of ISerializer
    partial class JsonSerializer : ISerializer
    {
        public void WriteBool(bool b) => _writer.WriteBooleanValue(b);

        public void WriteChar(char c) => WriteString(c.ToString());

        public void WriteByte(byte b) => _writer.WriteNumberValue(b);

        public void WriteU16(ushort u16) => _writer.WriteNumberValue(u16);

        public void WriteU32(uint u32) => _writer.WriteNumberValue(u32);

        public void WriteU64(ulong u64) => _writer.WriteNumberValue(u64);

        public void WriteSByte(sbyte b) => _writer.WriteNumberValue(b);

        public void WriteI16(short i16) => _writer.WriteNumberValue(i16);

        public void WriteI32(int i32) => _writer.WriteNumberValue(i32);

        public void WriteI64(long i64) => _writer.WriteNumberValue(i64);

        public void WriteFloat(float f) => _writer.WriteNumberValue(f);

        public void WriteDouble(double d) => _writer.WriteNumberValue(d);

        public void WriteDecimal(decimal d) => _writer.WriteNumberValue(d);

        public void WriteString(string s) => _writer.WriteStringValue(s);

        public void WriteNull() => _writer.WriteNullValue();

        ISerializeType ISerializer.WriteType(ISerdeInfo typeInfo)
        {
            if (typeInfo.Kind == InfoKind.Enum)
            {
                return _enumSerializer;
            }
            _writer.WriteStartObject();
            return this;
        }

        private sealed class EnumSerializer(JsonSerializer _parent) : ISerializeType
        {
            private void WriteEnumName(ISerdeInfo typeInfo, int index)
            {
                _parent._writer.WriteStringValue(typeInfo.GetFieldName(index));
            }

            public void End(ISerdeInfo info) { }
            public void WriteBool(ISerdeInfo typeInfo, int index, bool b) => ThrowInvalidEnum();
            public void WriteByte(ISerdeInfo typeInfo, int index, byte b) => WriteEnumName(typeInfo, index);
            public void WriteChar(ISerdeInfo typeInfo, int index, char c) => ThrowInvalidEnum();
            public void WriteDecimal(ISerdeInfo typeInfo, int index, decimal d) => ThrowInvalidEnum();
            public void WriteDouble(ISerdeInfo typeInfo, int index, double d) => ThrowInvalidEnum();
            public void WriteField<T>(ISerdeInfo typeInfo, int index, T value, ISerialize<T> serialize) where T : class? => ThrowInvalidEnum();
            public void WriteFloat(ISerdeInfo typeInfo, int index, float f) => ThrowInvalidEnum();
            public void WriteI16(ISerdeInfo typeInfo, int index, short i16) => WriteEnumName(typeInfo, index);
            public void WriteI32(ISerdeInfo typeInfo, int index, int i32) => WriteEnumName(typeInfo, index);
            public void WriteI64(ISerdeInfo typeInfo, int index, long i64) => WriteEnumName(typeInfo, index);
            public void WriteNull(ISerdeInfo typeInfo, int index) => ThrowInvalidEnum();
            public void WriteSByte(ISerdeInfo typeInfo, int index, sbyte b) => WriteEnumName(typeInfo, index);
            public void WriteString(ISerdeInfo typeInfo, int index, string s) => ThrowInvalidEnum();
            public void WriteU16(ISerdeInfo typeInfo, int index, ushort u16) => WriteEnumName(typeInfo, index);
            public void WriteU32(ISerdeInfo typeInfo, int index, uint u32) => WriteEnumName(typeInfo, index);
            public void WriteU64(ISerdeInfo typeInfo, int index, ulong u64) => WriteEnumName(typeInfo, index);
            private void ThrowInvalidEnum() => throw new InvalidOperationException("Invalid operation for enum serialization, expected integer value.");
        }

        ISerializeCollection ISerializer.WriteCollection(ISerdeInfo typeInfo, int? length)
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
            void ISerializeCollection.WriteElement<T, U>(T value, U serialize)
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
            public void WriteBool(bool b) => throw new KeyNotStringException();
            public void WriteChar(char c) => throw new KeyNotStringException();
            public void WriteByte(byte b) => throw new KeyNotStringException();
            public void WriteU16(ushort u16) => throw new KeyNotStringException();

            public void WriteU32(uint u32) => throw new KeyNotStringException();

            public void WriteU64(ulong u64) => throw new KeyNotStringException();

            public void WriteSByte(sbyte b) => throw new KeyNotStringException();

            public void WriteI16(short i16) => throw new KeyNotStringException();

            public void WriteI32(int i32) => throw new KeyNotStringException();

            public void WriteI64(long i64) => throw new KeyNotStringException();

            public void WriteFloat(float f) => throw new KeyNotStringException();

            public void WriteDouble(double d) => throw new KeyNotStringException();

            public void WriteDecimal(decimal d) => throw new KeyNotStringException();

            public void WriteString(string s)
            {
                _parent._writer.WritePropertyName(s);
            }

            public ISerializeCollection WriteCollection(ISerdeInfo typeInfo, int? length) => throw new KeyNotStringException();
            public ISerializeType WriteType(ISerdeInfo typeInfo) => throw new KeyNotStringException();
            public void WriteNull() => throw new KeyNotStringException();
        }
    }

    partial class JsonSerializer : ISerializeCollection
    {
        void ISerializeCollection.WriteElement<T, U>(T value, U serialize)
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
        void ISerializeType.WriteField<T>(ISerdeInfo typeInfo, int fieldIndex, T value, ISerialize<T> serialize)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(fieldIndex));
            serialize.Serialize(value, this);
        }

        void ISerializeType.End(ISerdeInfo typeInfo)
        {
            _writer.WriteEndObject();
        }
        void ISerializeType.WriteBool(ISerdeInfo typeInfo, int index, bool b)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteBool(b);
        }

        void ISerializeType.WriteChar(ISerdeInfo typeInfo, int index, char c)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteChar(c);
        }

        void ISerializeType.WriteByte(ISerdeInfo typeInfo, int index, byte b)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteByte(b);
        }

        void ISerializeType.WriteU16(ISerdeInfo typeInfo, int index, ushort u16)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteU16(u16);
        }

        void ISerializeType.WriteU32(ISerdeInfo typeInfo, int index, uint u32)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteU32(u32);
        }

        void ISerializeType.WriteU64(ISerdeInfo typeInfo, int index, ulong u64)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteU64(u64);
        }

        void ISerializeType.WriteSByte(ISerdeInfo typeInfo, int index, sbyte b)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteSByte(b);
        }

        void ISerializeType.WriteI16(ISerdeInfo typeInfo, int index, short i16)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteI16(i16);
        }

        void ISerializeType.WriteI32(ISerdeInfo typeInfo, int index, int i32)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteI32(i32);
        }

        void ISerializeType.WriteI64(ISerdeInfo typeInfo, int index, long i64)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteI64(i64);
        }

        void ISerializeType.WriteFloat(ISerdeInfo typeInfo, int index, float f)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteFloat(f);
        }

        void ISerializeType.WriteDouble(ISerdeInfo typeInfo, int index, double d)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteDouble(d);
        }

        void ISerializeType.WriteDecimal(ISerdeInfo typeInfo, int index, decimal d)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteDecimal(d);
        }

        void ISerializeType.WriteString(ISerdeInfo typeInfo, int index, string s)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteString(s);
        }

        void ISerializeType.WriteNull(ISerdeInfo typeInfo, int index)
        {
            _writer.WritePropertyName(typeInfo.GetFieldName(index));
            WriteNull();
        }
    }
}