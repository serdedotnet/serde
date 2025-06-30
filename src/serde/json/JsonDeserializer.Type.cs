using System;
using System.Buffers;
using System.Buffers.Text;
using System.Threading.Tasks;
using static Serde.Json.ThrowHelpers;

namespace Serde.Json;

partial class JsonDeserializer<TReader> : ITypeDeserializer
{
    int? ITypeDeserializer.SizeOpt => null;

    async ValueTask<int> ITypeDeserializer.TryReadIndex(ISerdeInfo info)
    {
        return (await TryReadIndexWithName(info)).Item1;
    }

    async ValueTask<(int, string? errorName)> ITypeDeserializer.TryReadIndexWithName(ISerdeInfo serdeInfo)
        => await TryReadIndexWithName(serdeInfo);

    private async ValueTask<(int, string? errorName)> TryReadIndexWithName(ISerdeInfo serdeInfo)
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
                Reader.Advance();
                return (ITypeDeserializer.EndOfType, null);
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
        var errorName = localIndex == ITypeDeserializer.IndexNotFound ? span.ToString() : null;
        _first = false;
        return (localIndex, errorName);
    }

    async ValueTask<T> ITypeDeserializer.ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> d)
    {
        ReadColon();
        return await d.Deserialize(this).ConfigureAwait(false);
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

    ValueTask<bool> ITypeDeserializer.ReadBool(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadBool());
    }
    ValueTask<char> ITypeDeserializer.ReadChar(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadChar());
    }
    ValueTask<byte> ITypeDeserializer.ReadU8(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadU8());
    }
    ValueTask<ushort> ITypeDeserializer.ReadU16(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadU16());
    }
    ValueTask<uint> ITypeDeserializer.ReadU32(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadU32());
    }
    ValueTask<ulong> ITypeDeserializer.ReadU64(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadU64());
    }
    ValueTask<sbyte> ITypeDeserializer.ReadI8(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadI8());
    }
    ValueTask<short> ITypeDeserializer.ReadI16(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadI16());
    }
    ValueTask<int> ITypeDeserializer.ReadI32(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadI32());
    }
    ValueTask<long> ITypeDeserializer.ReadI64(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadI64());
    }
    ValueTask<float> ITypeDeserializer.ReadF32(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadF32());
    }
    ValueTask<double> ITypeDeserializer.ReadF64(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadF64());
    }
    ValueTask<decimal> ITypeDeserializer.ReadDecimal(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadDecimal());
    }
    ValueTask<string> ITypeDeserializer.ReadString(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(this.ReadString());
    }
    ValueTask<DateTime> ITypeDeserializer.ReadDateTime(ISerdeInfo info, int index)
    {
        ReadColon();
        return ValueTask.FromResult(ReadDateTime());
    }
    ValueTask ITypeDeserializer.ReadBytes(ISerdeInfo info, int index, IBufferWriter<byte> writer)
    {
        ReadColon();
        ReadBytes(writer);
        return ValueTask.CompletedTask;
    }
    ValueTask ITypeDeserializer.SkipValue(ISerdeInfo info, int index)
    {
        ReadColon();
        Reader.Skip();
        return ValueTask.CompletedTask;
    }
}