using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Serde.IO;
using static Serde.Json.ThrowHelpers;

namespace Serde.Json;

internal static class JsonDeserializer
{
    public static JsonDeserializer<ArrayReader> FromString(string s)
    {
        return FromUtf8_Unsafe(Encoding.UTF8.GetBytes(s));
    }

    /// <summary>
    /// Assumes that the input is valid UTF-8.
    /// </summary>
    internal static JsonDeserializer<ArrayReader> FromUtf8_Unsafe(byte[] utf8Bytes)
    {
        var byteReader = new ArrayReader(utf8Bytes);
        return new JsonDeserializer<ArrayReader>(byteReader);
    }
}

internal abstract class BaseJsonDeserializer
{
    public abstract JsonValue ReadJsonValue();
}

internal sealed partial class JsonDeserializer<TReader> : BaseJsonDeserializer, IDeserializer
    where TReader : IByteReader
{
    private Utf8JsonLexer<TReader> Reader;
    private ScratchBuffer _scratch;

    /// <summary>
    /// Tracks whether we are before the first element of an object.
    /// </summary>
    private bool _first = false;

    internal JsonDeserializer(TReader byteReader)
    {
        Reader = new Utf8JsonLexer<TReader>(byteReader);
        _scratch = new ScratchBuffer();
    }

    public async Task<T?> ReadNullableRef<T>(IDeserialize<T> proxy)
        where T : class
    {
        var peek = Reader.SkipWhitespace();
        switch (ThrowIfEos(peek))
        {
            case (byte)'n' when Reader.StartsWith("null"u8):
                Reader.Advance(4);
                return null;
            default:
                return await proxy.Deserialize(this);
        }
    }

    public bool ReadBool()
    {
        _ = ThrowIfEos(Reader.SkipWhitespace());
        return Reader.GetBoolean();
    }

    public float ReadF32() => Convert.ToSingle(ReadF64());

    public double ReadF64()
    {
        _ = ThrowIfEos(Reader.SkipWhitespace());
        _scratch.Clear();
        return Reader.GetDouble(_scratch);
    }

    public decimal ReadDecimal()
    {
        _ = ThrowIfEos(Reader.SkipWhitespace());
        _scratch.Clear();
        return Reader.GetDecimal(_scratch);
    }

    public sbyte ReadI8() => Convert.ToSByte(ReadI64());

    public short ReadI16() => Convert.ToInt16(ReadI64());

    public int ReadI32() => Convert.ToInt32(ReadI64());

    public long ReadI64()
    {
        _ = ThrowIfEos(Reader.SkipWhitespace());
        _scratch.Clear();
        return Reader.GetInt64(_scratch);
    }

    public string ReadString()
    {
        return Encoding.UTF8.GetString(ReadUtf8Span());
    }

    private Utf8Span ReadUtf8Span()
    {
        var peek = ThrowIfEos(Reader.SkipWhitespace());
        if (peek != (byte)'"')
        {
            throw new JsonException($"Expected '\"', found: '{(char)peek}'");
        }
        Reader.Advance();
        _scratch.Clear();
        return Reader.LexUtf8Span(_scratch);
    }

    public ITypeDeserializer ReadType(ISerdeInfo info)
    {
        switch (info.Kind)
        {
            case InfoKind.CustomType:
            case InfoKind.Union:
            {
                // Custom types look like dictionaries, enums are inline strings
                var peek = Reader.SkipWhitespace();
                if (peek != (short)'{')
                {
                    throw new JsonException("Expected object start");
                }
                Reader.Advance();
                peek = Reader.SkipWhitespace();
                if (peek == (byte)',')
                {
                    throw new JsonException("Unexpected ',' before first element");
                }
                _first = true;
                goto case InfoKind.Enum;
            }
            case InfoKind.Enum:
                return this;
            case InfoKind.List:
            {
                var peek = Reader.SkipWhitespace();
                if (peek != (byte)'[')
                {
                    throw new JsonException("Expected array start");
                }
                Reader.Advance();
                peek = Reader.SkipWhitespace();
                if (peek == (byte)',')
                {
                    throw new JsonException("Unexpected ',' before first element");
                }
                return new DeCollection(this);
            }
            case InfoKind.Dictionary:
            {
                var peek = Reader.SkipWhitespace();
                if (peek != (byte)'{')
                {
                    throw new JsonException("Expected object start");
                }
                Reader.Advance();
                peek = Reader.SkipWhitespace();
                if (peek == (byte)',')
                {
                    throw new JsonException("Unexpected ',' before first element");
                }
                return new DeCollection(this);
            }
            default:
                throw new ArgumentException($"Expected CustomType or Enum, found {info.Kind}");
        }
    }

    public byte ReadU8() => Convert.ToByte(ReadU64());

    public ushort ReadU16() => Convert.ToUInt16(ReadU64());

    public uint ReadU32() => Convert.ToUInt32(ReadU64());

    public ulong ReadU64()
    {
        Reader.SkipWhitespace();
        _scratch.Clear();
        var u64 = Reader.GetUInt64(_scratch);
        return u64;
    }

    public char ReadChar() => ReadString().Single();

    public DateTime ReadDateTime()
    {
        var s = ReadString();
        return DateTime.Parse(s, styles: DateTimeStyles.RoundtripKind);
    }

    public void Eof()
    {
        if (Reader.SkipWhitespace() != IByteReader.EndOfStream)
        {
            throw new JsonException("Expected end of stream");
        }
    }

    void IDisposable.Dispose()
    {
        _scratch.Dispose();
        _scratch = null!;
    }

    public override JsonValue ReadJsonValue()
    {
        // Spec: https://www.json.org/json-en.html
        var peek = Reader.SkipWhitespace();
        switch (peek)
        {
            case (byte)'n':
                if (Reader.StartsWith("null"u8))
                {
                    Reader.Advance(4);
                    return JsonValue.Null.Instance;
                }
                break;
            case (byte)'t':
                if (Reader.StartsWith("true"u8))
                {
                    Reader.Advance(4);
                    return new JsonValue.Bool(true);
                }
                break;
            case (byte)'f':
                if (Reader.StartsWith("false"u8))
                {
                    Reader.Advance(5);
                    return new JsonValue.Bool(false);
                }
                break;
            case (byte)'"':
                return new JsonValue.String(ReadString());
            case (byte)'-' or (>= (byte)'0' and <= (byte)'9'):
                return new JsonValue.Number(ReadF64());
            case (byte)'{':
            {
                // Spec:
                // object =
                //   '{' ws '}'
                //   '{' members '}'
                // members =
                //   member
                //   member ',' members
                // member =
                //   ws string ws ':' element
                Reader.Advance();
                var next = Reader.SkipWhitespace();
                if (next == (byte)',')
                {
                    throw new JsonException("Unexpected ',' before first element");
                }
                if (next == (byte)'}')
                {
                    Reader.Advance();
                    return new JsonValue.Object(ImmutableDictionary<string, JsonValue>.Empty);
                }
                var builder = ImmutableDictionary.CreateBuilder<string, JsonValue>();
                while (true)
                {
                    var key = ReadString();
                    if (Reader.SkipWhitespace() != (byte)':')
                    {
                        throw new JsonException($"Expected ':', found {(char)Reader.Peek()}");
                    }
                    Reader.Advance();
                    var value = ReadJsonValue();
                    builder.Add(key, value);
                    next = Reader.SkipWhitespace();
                    switch (next)
                    {
                        case (byte)',':
                            Reader.Advance();
                            continue;
                        case (byte)'}':
                            Reader.Advance();
                            break;
                        default:
                            throw new JsonException($"Expected '}}' or ',', found {(char)next}");
                    }
                    break;
                }
                return new JsonValue.Object(builder.ToImmutable());
            }
            case (byte)'[':
            {
                // Spec:
                // array =
                //   '[' ws ']'
                //   '[' elements ']'
                // elements =
                //   element
                //   element ',' elements
                Reader.Advance();
                var next = Reader.SkipWhitespace();
                if (next == (byte)',')
                {
                    throw new JsonException("Unexpected ',' before first element");
                }
                if (next == (byte)']')
                {
                    Reader.Advance();
                    return new JsonValue.Array(ImmutableArray<JsonValue>.Empty);
                }
                var arrayBuilder = ImmutableArray.CreateBuilder<JsonValue>();
                while (true)
                {
                    var value = ReadJsonValue();
                    arrayBuilder.Add(value);
                    next = Reader.SkipWhitespace();
                    switch (next)
                    {
                        case (byte)',':
                            Reader.Advance();
                            continue;
                        case (byte)']':
                            Reader.Advance();
                            break;
                        default:
                            throw new JsonException($"Expected ']' or ',', found {(char)next}");
                    }
                    break;
                }
                return new JsonValue.Array(arrayBuilder.ToImmutable());
            }
        }
        throw new JsonException($"Unexpected token: {(char)peek}");
    }

    public void ReadBytes(IBufferWriter<byte> writer)
    {
        var span = ReadUtf8Span();
        if (span.Length == 0)
        {
            return;
        }
        // This should be a base64 string, calculate needed size
        if (span.Length % 4 != 0)
        {
            throw new JsonException($"Invalid base64 string length: {span.Length}");
        }
        var size = span.Length / 4 * 3;
        var buffer = writer.GetSpan(size);
        var status = Base64.DecodeFromUtf8(span, buffer, out _, out var written);
        writer.Advance(written);
        if (status != OperationStatus.Done)
        {
            throw new JsonException($"Invalid base64 string: {status}");
        }
    }

    Task<bool> IDeserializer.ReadBool() => Task.FromResult(ReadBool());
    Task<float> IDeserializer.ReadF32() => Task.FromResult(ReadF32());
    Task<double> IDeserializer.ReadF64() => Task.FromResult(ReadF64());
    Task<decimal> IDeserializer.ReadDecimal() => Task.FromResult(ReadDecimal());
    Task<sbyte> IDeserializer.ReadI8() => Task.FromResult(ReadI8());
    Task<short> IDeserializer.ReadI16() => Task.FromResult(ReadI16());
    Task<int> IDeserializer.ReadI32() => Task.FromResult(ReadI32());
    Task<long> IDeserializer.ReadI64() => Task.FromResult(ReadI64());
    Task<byte> IDeserializer.ReadU8() => Task.FromResult(ReadU8());
    Task<ushort> IDeserializer.ReadU16() => Task.FromResult(ReadU16());
    Task<uint> IDeserializer.ReadU32() => Task.FromResult(ReadU32());
    Task<ulong> IDeserializer.ReadU64() => Task.FromResult(ReadU64());
    Task<char> IDeserializer.ReadChar() => Task.FromResult(ReadChar());
    Task<DateTime> IDeserializer.ReadDateTime() => Task.FromResult(ReadDateTime());
    Task<string> IDeserializer.ReadString() => Task.FromResult(ReadString());
    ITypeDeserializer IDeserializer.ReadType(ISerdeInfo info) => ReadType(info);
    Task IDeserializer.ReadBytes(IBufferWriter<byte> writer)
    {
        ReadBytes(writer);
        return Task.CompletedTask;
    }
}