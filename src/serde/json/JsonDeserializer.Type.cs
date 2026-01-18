using System;
using System.Buffers;
using System.Buffers.Text;
using static Serde.Json.ThrowHelpers;

namespace Serde.Json;

partial class JsonDeserializer<TReader>
{
    private sealed class DeType : ITypeDeserializer
    {
        private readonly JsonDeserializer<TReader> _deserializer;

        public DeType(JsonDeserializer<TReader> deserializer)
        {
            _deserializer = deserializer;
        }

        int? ITypeDeserializer.SizeOpt => null;

        int ITypeDeserializer.TryReadIndex(ISerdeInfo info)
        {
            return TryReadIndexWithName(info).Item1;
        }

        (int, string? errorName) ITypeDeserializer.TryReadIndexWithName(ISerdeInfo serdeInfo)
            => TryReadIndexWithName(serdeInfo);

        private (int, string? errorName) TryReadIndexWithName(ISerdeInfo serdeInfo)
        {
            if (serdeInfo.Kind == InfoKind.Enum)
            {
                switch (ThrowIfEos(_deserializer.Reader.SkipWhitespace()))
                {
                    case (byte)'"':
                        break;
                    default:
                        throw new JsonException("Expected enum name as string");
                }
            }
            else
            {
                var peek = _deserializer.Reader.SkipWhitespace();
                if (peek == (byte)'}')
                {
                    _deserializer.Reader.Advance();
                    return (ITypeDeserializer.EndOfType, null);
                }
                if (!_deserializer._first)
                {
                    if (peek != (byte)',')
                    {
                        throw new JsonException($"Expected '}}' or ',', found: '{(char)peek}'");
                    }
                    _deserializer.Reader.Advance();
                    peek = _deserializer.Reader.SkipWhitespace();
                }
                if (peek != (byte)'"')
                {
                    throw new JsonException($"Expected property name, got: '{(char)peek}'");
                }
            }

            // Read a string
            _deserializer.Reader.Advance();
            _deserializer._scratch.Clear();
            var span = _deserializer.Reader.LexUtf8Span(_deserializer._scratch);
            var localIndex = serdeInfo.TryGetIndex(span);
            var errorName = localIndex == ITypeDeserializer.IndexNotFound ? span.ToString() : null;
            _deserializer._first = false;
            return (localIndex, errorName);
        }

        T ITypeDeserializer.ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> d)
        {
            ReadColon();
            return d.Deserialize(_deserializer);
        }

        uint ITypeDeserializer.ReadValue32(ISerdeInfo info, int index, IDeserialize<uint> d)
        {
            ReadColon();
            return d.Deserialize(_deserializer);
        }

        ulong ITypeDeserializer.ReadValue64(ISerdeInfo info, int index, IDeserialize<ulong> d)
        {
            ReadColon();
            return d.Deserialize(_deserializer);
        }

        UInt128 ITypeDeserializer.ReadValue128(ISerdeInfo info, int index, IDeserialize<UInt128> d)
        {
            ReadColon();
            return d.Deserialize(_deserializer);
        }

        private void ReadColon()
        {
            var peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            if (peek != (byte)':')
            {
                throw new JsonException("Expected ':'");
            }
            _deserializer.Reader.Advance();
        }

        bool ITypeDeserializer.ReadBool(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadBool();
        }
        char ITypeDeserializer.ReadChar(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadChar();
        }
        byte ITypeDeserializer.ReadU8(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadU8();
        }
        ushort ITypeDeserializer.ReadU16(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadU16();
        }
        uint ITypeDeserializer.ReadU32(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadU32();
        }
        ulong ITypeDeserializer.ReadU64(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadU64();
        }
        UInt128 ITypeDeserializer.ReadU128(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadU128();
        }
        sbyte ITypeDeserializer.ReadI8(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadI8();
        }
        short ITypeDeserializer.ReadI16(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadI16();
        }
        int ITypeDeserializer.ReadI32(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadI32();
        }
        long ITypeDeserializer.ReadI64(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadI64();
        }
        Int128 ITypeDeserializer.ReadI128(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadI128();
        }
        float ITypeDeserializer.ReadF32(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadF32();
        }
        double ITypeDeserializer.ReadF64(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadF64();
        }
        decimal ITypeDeserializer.ReadDecimal(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadDecimal();
        }
        string ITypeDeserializer.ReadString(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadString();
        }
        DateTime ITypeDeserializer.ReadDateTime(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadDateTime();
        }
        DateTimeOffset ITypeDeserializer.ReadDateTimeOffset(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadDateTimeOffset();
        }
        DateOnly ITypeDeserializer.ReadDateOnly(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadDateOnly();
        }
        TimeOnly ITypeDeserializer.ReadTimeOnly(ISerdeInfo info, int index)
        {
            ReadColon();
            return _deserializer.ReadTimeOnly();
        }
        void ITypeDeserializer.ReadBytes(ISerdeInfo info, int index, IBufferWriter<byte> writer)
        {
            ReadColon();
            _deserializer.ReadBytes(writer);
        }

        void ITypeDeserializer.SkipValue(ISerdeInfo info, int index)
        {
            ReadColon();
            _deserializer.Reader.Skip();
        }

        void IDisposable.Dispose()
        {
            // Nothing to dispose
        }
    }
}