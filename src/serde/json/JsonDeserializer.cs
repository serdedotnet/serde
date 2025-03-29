
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
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

internal sealed partial class JsonDeserializer<TReader> : IDeserializer
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

    public T? ReadNullableRef<T>(IDeserialize<T> proxy)
        where T : class
    {
        var peek = Reader.SkipWhitespace();
        switch (ThrowIfEos(peek))
        {
            case (byte)'n' when Reader.StartsWith("null"u8):
                Reader.Advance(4);
                return null;
            default:
                return proxy.Deserialize(this);
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

    public JsonValue ReadJsonValue()
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
}