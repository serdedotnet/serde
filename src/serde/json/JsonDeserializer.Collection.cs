
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using static Serde.Json.ThrowHelpers;

namespace Serde.Json;

partial class JsonDeserializer<TReader>
{
    private sealed class DeCollection : ITypeDeserializer
    {
        private readonly JsonDeserializer<TReader> _deserializer;
        private int _index = 0;

        public DeCollection(JsonDeserializer<TReader> de)
        {
            _deserializer = de;
        }

        public int? SizeOpt => null;

        public int TryReadIndex(ISerdeInfo info)
            => TryReadIndexWithName(info).Item1;

        public (int, string? errorName) TryReadIndexWithName(ISerdeInfo info)
        {
            string? errorName;
            switch (info.Kind)
            {
                case InfoKind.List:
                    return (TryReadIndexEnumerable(out errorName), errorName);
                case InfoKind.Dictionary:
                    return (TryReadIndexDictionary(out errorName), errorName);
                default:
                    throw new ArgumentException($"TypeKind is {info.Kind}, expected Enumerable or Dictionary");
            }
        }

        private int TryReadIndexEnumerable(out string? errorName)
        {
            var first = _index == 0;
            var peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            if (peek == (byte)']')
            {
                errorName = null;
                _deserializer.Reader.Advance();
                return ITypeDeserializer.EndOfType;
            }
            if (!first)
            {
                if (peek != (byte)',')
                {
                    throw new JsonException($"Expected ']' or ',', found: '{(char)peek}'");
                }
                _deserializer.Reader.Advance();
            }
            errorName = null;
            return _index;
        }

        private int TryReadIndexDictionary(out string? errorName)
        {
            var first = _index == 0;
            bool afterKey = _index % 2 == 1;
            var peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            if (afterKey)
            {
                if (peek != (byte)':')
                {
                    throw new JsonException("Expected ':' after key");
                }
                _deserializer.Reader.Advance();
            }
            else
            {
                if (peek == (byte)'}')
                {
                    errorName = null;
                    _deserializer.Reader.Advance();
                    return ITypeDeserializer.EndOfType;
                }
                if (!first)
                {
                    if (peek != (byte)',')
                    {
                        throw new JsonException("Expected ',' or '}'");
                    }
                    _deserializer.Reader.Advance();
                }
            }
            errorName = null;
            return _index;
        }

        public T ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> d) where T : class?
        {
            var next = d.Deserialize(_deserializer);
            _index++;
            return next;
        }

        public UInt32 ReadValue32(ISerdeInfo info, int index, IDeserialize<UInt32> d)
        {
            var next = d.Deserialize(_deserializer);
            _index++;
            return next;
        }

        public UInt64 ReadValue64(ISerdeInfo info, int index, IDeserialize<UInt64> d)
        {
            var next = d.Deserialize(_deserializer);
            _index++;
            return next;
        }

        public UInt128 ReadValue128(ISerdeInfo info, int index, IDeserialize<UInt128> d)
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

        public UInt128 ReadU128(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadU128();
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

        public Int128 ReadI128(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadI128();
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

        public DateTime ReadDateTime(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadDateTime();
            _index++;
            return v;
        }

        public DateTimeOffset ReadDateTimeOffset(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadDateTimeOffset();
            _index++;
            return v;
        }

        public DateOnly ReadDateOnly(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadDateOnly();
            _index++;
            return v;
        }

        public TimeOnly ReadTimeOnly(ISerdeInfo info, int index)
        {
            var v = _deserializer.ReadTimeOnly();
            _index++;
            return v;
        }

        void ITypeDeserializer.ReadBytes(ISerdeInfo info, int index, IBufferWriter<byte> writer)
        {
            _deserializer.ReadBytes(writer);
            _index++;
        }
    }
}