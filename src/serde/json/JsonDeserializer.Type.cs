using System;
using System.Buffers;
using System.Buffers.Text;
using static Serde.Json.ThrowHelpers;

namespace Serde.Json;

partial class JsonDeserializer<TReader> : ITypeDeserializer
{
    int? ITypeDeserializer.SizeOpt => null;

    int ITypeDeserializer.TryReadIndex(ISerdeInfo serdeInfo, out string? errorName)
    {
        if (serdeInfo.Kind == InfoKind.Enum)
        {
            switch (ThrowIfEos(Reader.SkipWhitespace()))
            {
                case (byte)'"':
                    break;
                default:
                    throw new JsonException("Expected enum name as string");
            }
        }
        else
        {
            var peek = Reader.SkipWhitespace();
            if (peek == (byte)'}')
            {
                errorName = null;
                Reader.Advance();
                return ITypeDeserializer.EndOfType;
            }
            if (!_first)
            {
                if (peek != (byte)',')
                {
                    throw new JsonException($"Expected '}}' or ',', found: '{(char)peek}'");
                }
                Reader.Advance();
                peek = Reader.SkipWhitespace();
            }
            if (peek != (byte)'"')
            {
                throw new JsonException($"Expected property name, got: '{(char)peek}'");
            }
        }

        // Read a string
        Reader.Advance();
        _scratch.Clear();
        var span = Reader.LexUtf8Span(_scratch);
        var localIndex = serdeInfo.TryGetIndex(span);
        errorName = localIndex == ITypeDeserializer.IndexNotFound ? span.ToString() : null;
        _first = false;
        return localIndex;
    }

    T ITypeDeserializer.ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> d)
    {
        ReadColon();
        return d.Deserialize(this);
    }

    private void ReadColon()
    {
        var peek = ThrowIfEos(Reader.SkipWhitespace());
        if (peek != (byte)':')
        {
            throw new JsonException("Expected ':'");
        }
        Reader.Advance();
    }

    bool ITypeDeserializer.ReadBool(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadBool();
    }
    char ITypeDeserializer.ReadChar(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadChar();
    }
    byte ITypeDeserializer.ReadU8(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadU8();
    }
    ushort ITypeDeserializer.ReadU16(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadU16();
    }
    uint ITypeDeserializer.ReadU32(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadU32();
    }
    ulong ITypeDeserializer.ReadU64(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadU64();
    }
    sbyte ITypeDeserializer.ReadI8(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadI8();
    }
    short ITypeDeserializer.ReadI16(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadI16();
    }
    int ITypeDeserializer.ReadI32(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadI32();
    }
    long ITypeDeserializer.ReadI64(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadI64();
    }
    float ITypeDeserializer.ReadF32(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadF32();
    }
    double ITypeDeserializer.ReadF64(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadF64();
    }
    decimal ITypeDeserializer.ReadDecimal(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadDecimal();
    }
    string ITypeDeserializer.ReadString(ISerdeInfo info, int index)
    {
        ReadColon();
        return this.ReadString();
    }
    DateTime ITypeDeserializer.ReadDateTime(ISerdeInfo info, int index)
    {
        ReadColon();
        return ReadDateTime();
    }
    void ITypeDeserializer.ReadBytes(ISerdeInfo info, int index, IBufferWriter<byte> writer)
    {
        ReadColon();
        ReadBytes(writer);
    }

    void ITypeDeserializer.SkipValue(ISerdeInfo info, int index)
    {
        ReadColon();
        Reader.Skip();
    }
}