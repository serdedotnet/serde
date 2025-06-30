using System;
using System.Buffers;
using System.Buffers.Text;
using System.Threading.Tasks;
using static Serde.Json.ThrowHelpers;

namespace Serde.Json;

partial class JsonDeserializer<TReader> : ITypeDeserializer
{
    int? ITypeDeserializer.SizeOpt => null;

    async Task<int> ITypeDeserializer.TryReadIndex(ISerdeInfo info)
    {
        return (await TryReadIndexWithName(info)).Item1;
    }

    async Task<(int, string? errorName)> ITypeDeserializer.TryReadIndexWithName(ISerdeInfo serdeInfo)
        => await TryReadIndexWithName(serdeInfo);

    private async Task<(int, string? errorName)> TryReadIndexWithName(ISerdeInfo serdeInfo)
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

    async Task<T> ITypeDeserializer.ReadValue<T>(ISerdeInfo info, int index, IDeserialize<T> d)
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

    Task<bool> ITypeDeserializer.ReadBool(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadBool());
    }
    Task<char> ITypeDeserializer.ReadChar(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadChar());
    }
    Task<byte> ITypeDeserializer.ReadU8(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadU8());
    }
    Task<ushort> ITypeDeserializer.ReadU16(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadU16());
    }
    Task<uint> ITypeDeserializer.ReadU32(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadU32());
    }
    Task<ulong> ITypeDeserializer.ReadU64(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadU64());
    }
    Task<sbyte> ITypeDeserializer.ReadI8(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadI8());
    }
    Task<short> ITypeDeserializer.ReadI16(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadI16());
    }
    Task<int> ITypeDeserializer.ReadI32(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadI32());
    }
    Task<long> ITypeDeserializer.ReadI64(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadI64());
    }
    Task<float> ITypeDeserializer.ReadF32(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadF32());
    }
    Task<double> ITypeDeserializer.ReadF64(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadF64());
    }
    Task<decimal> ITypeDeserializer.ReadDecimal(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadDecimal());
    }
    Task<string> ITypeDeserializer.ReadString(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(this.ReadString());
    }
    Task<DateTime> ITypeDeserializer.ReadDateTime(ISerdeInfo info, int index)
    {
        ReadColon();
        return Task.FromResult(ReadDateTime());
    }
    Task ITypeDeserializer.ReadBytes(ISerdeInfo info, int index, IBufferWriter<byte> writer)
    {
        ReadColon();
        ReadBytes(writer);
        return Task.CompletedTask;
    }
    Task ITypeDeserializer.SkipValue(ISerdeInfo info, int index)
    {
        ReadColon();
        Reader.Skip();
        return Task.CompletedTask;
    }
}