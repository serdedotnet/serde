
using System;
using System.Diagnostics.CodeAnalysis;
using static Serde.Json.ThrowHelpers;

namespace Serde.Json;

partial class JsonDeserializer<TReader>
{
    public ITypeDeserializer ReadCollection(ISerdeInfo typeInfo)
    {
        var kind = typeInfo.Kind;
        if (kind is not (InfoKind.Enumerable or InfoKind.Dictionary))
        {
            throw new ArgumentException($"TypeKind is {typeInfo.Kind}, expected Enumerable or Dictionary");
        }
        switch ((ThrowIfEos(Reader.SkipWhitespace()), kind))
        {
            case ((byte)'[', InfoKind.Enumerable):
            case ((byte)'{', InfoKind.Dictionary):
                Reader.Advance();
                break;
            case (_, InfoKind.Enumerable):
                throw new JsonException("Expected array start");
            case (_, InfoKind.Dictionary):
                throw new JsonException("Expected object start");
        }

        return new DeCollection(this);
    }

    private sealed class DeCollection : ITypeDeserializer
    {
        private readonly JsonDeserializer<TReader> _deserializer;
        private int _index = 0;

        public DeCollection(JsonDeserializer<TReader> de)
        {
            _deserializer = de;
        }

        public int? SizeOpt => null;

        public int TryReadIndex(ISerdeInfo info, out string? errorName)
        {
            switch (info.Kind)
            {
                case InfoKind.Enumerable:
                    return TryReadIndexEnumerable(out errorName);
                case InfoKind.Dictionary:
                    return TryReadIndexDictionary(out errorName);
                default:
                    throw new ArgumentException($"TypeKind is {info.Kind}, expected Enumerable or Dictionary");
            }
        }

        private int TryReadIndexEnumerable(out string? errorName)
        {
            var peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            if (peek == (short)',')
            {
                if (_index == 0)
                {
                    throw new JsonException("Unexpected comma before first element");
                }
                _deserializer.Reader.Advance();
                peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            }

            switch (peek)
            {
                case (byte)']':
                    _deserializer.Reader.Advance();
                    errorName = null;
                    return ITypeDeserializer.EndOfType;

                case (byte)'}':
                case (byte)':':
                    throw new JsonException($"Unexpected character {peek} in array");

                default:
                    errorName = null;
                    return _index++;
            }
        }

        private int TryReadIndexDictionary(out string? errorName)
        {
            bool first = _index == 0;
            bool afterKey = _index % 2 == 1;
            var peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            if (peek == (short)',')
            {
                if (first)
                {
                    throw new JsonException("Unexpected comma before first element");
                }
                if (afterKey)
                {
                    throw new JsonException("Unexpected comma after key");
                }
                _deserializer.Reader.Advance();
                peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            }

            if (afterKey && peek != (short)':')
            {
                throw new JsonException("Expected ':' after key");
            }

            if (peek == (short)':')
            {
                if (first || !afterKey)
                {
                    throw new JsonException("Unexpected ':' before key");
                }
                _deserializer.Reader.Advance();
                peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            }

            switch (peek)
            {
                case (byte)']':
                    throw new JsonException($"Unexpected '{peek}' in dictionary");

                case (byte)'}':
                    if (afterKey)
                    {
                        throw new JsonException("Expected object value, found '}'");
                    }
                    _deserializer.Reader.Advance();
                    errorName = null;
                    return ITypeDeserializer.EndOfType;

                default:
                    errorName = null;
                    return _index;
            }
        }

        public T ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> d) where T : class?
        {
            var next = d.Deserialize(_deserializer);
            _index++;
            return next;
        }

        public void SkipValue(ISerdeInfo info, int index)
        {
            _deserializer.Reader.Skip();
            _index++;
        }

        public bool ReadBool(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadBool();
            _index++;
            return v;
        }

        public char ReadChar(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadChar();
            _index++;
            return v;
        }

        public byte ReadU8(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadU8();
            _index++;
            return v;
        }

        public ushort ReadU16(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadU16();
            _index++;
            return v;
        }

        public uint ReadU32(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadU32();
            _index++;
            return v;
        }

        public ulong ReadU64(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadU64();
            _index++;
            return v;
        }

        public sbyte ReadI8(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadI8();
            _index++;
            return v;
        }

        public short ReadI16(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadI16();
            _index++;
            return v;
        }

        public int ReadI32(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadI32();
            _index++;
            return v;
        }

        public long ReadI64(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadI64();
            _index++;
            return v;
        }

        public float ReadF32(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadF32();
            _index++;
            return v;
        }

        public double ReadF64(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadF64();
            _index++;
            return v;
        }

        public decimal ReadDecimal(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadDecimal();
            _index++;
            return v;
        }

        public string ReadString(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadString();
            _index++;
            return v;
        }
    }
}