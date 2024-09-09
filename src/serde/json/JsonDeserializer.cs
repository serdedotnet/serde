
using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml.Linq;
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
    /// Tracks whether we are before the first element of an array or object.
    /// </summary>
    private bool _first = false;

    internal JsonDeserializer(TReader byteReader)
    {
        Reader = new Utf8JsonLexer<TReader>(byteReader);
        _scratch = new ScratchBuffer();
    }

    public T ReadAny<T>(IDeserializeVisitor<T> v)
    {
        var peek = Reader.SkipWhitespace();
        T result;
        switch (ThrowIfEos(peek))
        {
            case (byte)'[':
                result = ReadEnumerable(v);
                break;

            case (byte)'-' or (>= (byte)'0' and <= (byte)'9'):
                var d = ReadDouble();
                result = v.VisitDouble(d);
                break;

            case (byte)'{':
                result = DeserializeDictionary(v);
                break;

            case (byte)'"':
                result = VisitString(v);
                break;

            case (byte)'n' when Reader.StartsWith("null"u8):
                Reader.Advance(4);
                result = v.VisitNull();
                break;

            case (byte)'t' when Reader.StartsWith("true"u8):
                Reader.Advance(4);
                result = v.VisitBool(true);
                break;

            case (byte)'f' when Reader.StartsWith("false"u8):
                Reader.Advance(5);
                result = v.VisitBool(false);
                break;

            default:
                throw new JsonException($"Could not deserialize '{(char)peek}");
        }
        return result;
    }

    private T VisitString<T>(IDeserializeVisitor<T> v)
    {
        var span = ReadUtf8Span();
        return v.VisitUtf8Span(span);
    }

    public T ReadNullableRef<T>(IDeserializeVisitor<T> v)
    {
        var peek = Reader.SkipWhitespace();
        switch (ThrowIfEos(peek))
        {
            case (byte)'n' when Reader.StartsWith("null"u8):
                Reader.Advance(4);
                return v.VisitNull();
            default:
                return v.VisitNotNull(this);
        }
    }


    public bool ReadBool()
    {
        return Reader.GetBoolean();
    }

    public T DeserializeDictionary<T>(IDeserializeVisitor<T> v)
    {
        var peek = Reader.SkipWhitespace();

        if (peek != (short)'{')
        {
            throw new JsonException("Expected object start");
        }

        Reader.Advance();
        var map = new DeDictionary(this);
        return v.VisitDictionary(ref map);
    }

    public IDeserializeCollection ReadCollection(ISerdeInfo typeInfo)
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

    private struct DeCollection : IDeserializeCollection
    {
        private JsonDeserializer<TReader> _deserializer;
        private bool _first = true;
        private bool _afterKey = false;

        public DeCollection(JsonDeserializer<TReader> de)
        {
            _deserializer = de;
        }

        public int? SizeOpt => null;

        public bool TryReadValue<T, D>(ISerdeInfo typeInfo, [MaybeNullWhen(false)] out T next) where D : IDeserialize<T>
        {
            var peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            if (peek == (short)',')
            {
                if (_first)
                {
                    throw new JsonException("Unexpected comma before first element");
                }
                if (_afterKey)
                {
                    throw new JsonException("Unexpected comma after key");
                }
                _deserializer.Reader.Advance();
                peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            }

            if (_afterKey && peek != (short)':' && typeInfo.Kind == InfoKind.Dictionary)
            {
                throw new JsonException("Expected ':' after key");
            }

            if (peek == (short)':')
            {
                if (typeInfo.Kind == InfoKind.Enumerable)
                {
                    throw new JsonException("Unexpected ':' in array");
                }
                if (_first || !_afterKey)
                {
                    throw new JsonException("Unexpected ':' before key");
                }
                _deserializer.Reader.Advance();
                peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            }

            switch (peek)
            {
                case (byte)']' when typeInfo.Kind == InfoKind.Enumerable:
                    _deserializer.Reader.Advance();
                    next = default;
                    return false;

                case (byte)'}':
                    if (typeInfo.Kind != InfoKind.Dictionary)
                    {
                        throw new JsonException("Unexpected '}' in array");
                    }
                    if (_afterKey)
                    {
                        throw new JsonException("Expected object value, found '}'");
                    }
                    _deserializer.Reader.Advance();
                    next = default;
                    return false;

                default:
                    next = D.Deserialize(_deserializer);
                    _first = false;
                    _afterKey = typeInfo.Kind == InfoKind.Dictionary && !_afterKey;
                    return true;
            }
        }
    }

    public float ReadFloat() => Convert.ToSingle(ReadDouble());

    public double ReadDouble()
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

    /// <summary>
    /// Expects to be one byte after '['
    /// </summary>
    private T ReadEnumerable<T>(IDeserializeVisitor<T> v)
    {
        var peek = Reader.Peek();
        if (peek != (byte)'[')
        {
            throw new JsonException("Expected array start");
        }
        Reader.Advance();

        var enumerable = new DeEnumerable(this);
        return v.VisitEnumerable(ref enumerable);
    }

    private struct DeEnumerable : IDeserializeEnumerable
    {
        private JsonDeserializer<TReader> _deserializer;
        private bool _first = true;
        public DeEnumerable(JsonDeserializer<TReader> de)
        {
            _deserializer = de;
        }
        public int? SizeOpt => null;

        public bool TryGetNext<T, D>([MaybeNullWhen(false)] out T next)
            where D : IDeserialize<T>
        {
            while (true)
            {
                var peek = _deserializer.Reader.SkipWhitespace();
                if (peek == (short)',')
                {
                    if (_first)
                    {
                        throw new JsonException("Unexpected comma before first element");
                    }
                    _deserializer.Reader.Advance();
                    peek = _deserializer.Reader.SkipWhitespace();
                }

                switch (peek)
                {
                    case IByteReader.EndOfStream:
                        throw new JsonException("Unexpected end of stream");

                    case (short)']':
                        _deserializer.Reader.Advance();
                        next = default;
                        return false;

                    default:
                        _first = false;
                        next = D.Deserialize(_deserializer);
                        return true;
                }
            }
        }
    }

    private struct DeDictionary : IDeserializeDictionary
    {
        private JsonDeserializer<TReader> _deserializer;
        private bool _first = true;
        public DeDictionary(JsonDeserializer<TReader> de)
        {
            _deserializer = de;
        }

        public int? SizeOpt => null;

        public bool TryGetNextEntry<K, DK, V, DV>([MaybeNullWhen(false)] out (K, V) next)
            where DK : IDeserialize<K>
            where DV : IDeserialize<V>
        {
            // Don't save state
            if (!TryGetNextKey<K, DK>(out K? nextKey))
            {
                next = default;
                return false;
            }
            var nextValue = GetNextValue<V, DV>();
            next = (nextKey, nextValue);
            return true;
        }

        public bool TryGetNextKey<K, D>([MaybeNullWhen(false)] out K next) where D : IDeserialize<K>
        {
            while (true)
            {
                var peek = _deserializer.Reader.SkipWhitespace();

                if (peek == (short)',')
                {
                    if (_first)
                    {
                        throw new JsonException("Unexpected comma before first element");
                    }
                    _deserializer.Reader.Advance();
                    peek = _deserializer.Reader.SkipWhitespace();
                }

                switch (peek)
                {
                    case IByteReader.EndOfStream:
                        throw new JsonException("Unexpected end of stream");

                    case (short)'}':
                        // Check if the next token is the end of the object, but don't advance the stream if not
                        _deserializer.Reader.Advance();
                        next = default;
                        _first = false;
                        return false;

                    case (short)'"':
                        next = D.Deserialize(_deserializer);
                        _first = false;
                        return true;

                    default:
                        throw new JsonException("Expected property name, found: " + (char)peek);
                }
            }
        }

        public V GetNextValue<V, D>() where D : IDeserialize<V>
        {
            var peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            if (peek != (byte)':')
            {
                throw new JsonException("Expected ':'");
            }
            _deserializer.Reader.Advance();
            return D.Deserialize(_deserializer);
        }
    }

    public sbyte ReadSByte() => Convert.ToSByte(ReadI64());

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

    public IDeserializeType ReadType(ISerdeInfo fieldMap)
    {
        // Custom types look like dictionaries, enums are inline strings
        if (fieldMap.Kind == InfoKind.CustomType)
        {
            var peek = Reader.SkipWhitespace();
            if (peek != (short)'{')
            {
                throw new JsonException("Expected object start");
            }
            Reader.Advance();
        }
        else if (fieldMap.Kind != InfoKind.Enum)
        {
            throw new ArgumentException("Expected either CustomType or Enum kind, found " + fieldMap.Kind);
        }

        _first = true;
        return this;
    }

    public byte ReadByte() => Convert.ToByte(ReadU64());

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
}

partial class JsonDeserializer<TReader> : IDeserializeType
{
    V IDeserializeType.ReadValue<V, D>(int index)
    {
        ReadColon();
        return D.Deserialize(this);
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

    bool IDeserializeType.ReadBool(int index)
    {
        ReadColon();
        return ReadBool();
    }
    char IDeserializeType.ReadChar(int index)
    {
        ReadColon();
        return ReadChar();
    }
    byte IDeserializeType.ReadByte(int index)
    {
        ReadColon();
        return ReadByte();
    }
    ushort IDeserializeType.ReadU16(int index)
    {
        ReadColon();
        return ReadU16();
    }
    uint IDeserializeType.ReadU32(int index)
    {
        ReadColon();
        return ReadU32();
    }
    ulong IDeserializeType.ReadU64(int index)
    {
        ReadColon();
        return ReadU64();
    }
    sbyte IDeserializeType.ReadSByte(int index)
    {
        ReadColon();
        return ReadSByte();
    }
    short IDeserializeType.ReadI16(int index)
    {
        ReadColon();
        return ReadI16();
    }
    int IDeserializeType.ReadI32(int index)
    {
        ReadColon();
        return ReadI32();
    }
    long IDeserializeType.ReadI64(int index)
    {
        ReadColon();
        return ReadI64();
    }
    float IDeserializeType.ReadFloat(int index)
    {
        ReadColon();
        return ReadFloat();
    }
    double IDeserializeType.ReadDouble(int index)
    {
        ReadColon();
        return ReadDouble();
    }
    decimal IDeserializeType.ReadDecimal(int index)
    {
        ReadColon();
        return ReadDecimal();
    }
    string IDeserializeType.ReadString(int index)
    {
        ReadColon();
        return this.ReadString();
    }

    void IDeserializeType.SkipValue()
    {
        ReadColon();
        Reader.Skip();
    }

    int IDeserializeType.TryReadIndex(ISerdeInfo serdeInfo, out string? errorName)
    {
        if (serdeInfo.Kind == InfoKind.Enum)
        {
            switch (ThrowIfEos(Reader.SkipWhitespace()))
            {
                default:
                    throw new JsonException("Expected enum name as string");
                case (byte)'"':
                    goto ReadIndexAsString;
            };
        }
        else
        {
            while (true)
            {
                var peek = Reader.SkipWhitespace();
                if (peek == (short)',')
                {
                    if (_first)
                    {
                        throw new JsonException("Unexpected comma before first element");
                    }

                    Reader.Advance();
                    peek = Reader.SkipWhitespace();
                }

                switch (ThrowIfEos(peek))
                {
                    case (byte)'}':
                        errorName = null;
                        Reader.Advance();
                        _first = false;
                        return IDeserializeType.EndOfType;

                    case (byte)'"':
                        goto ReadIndexAsString;

                    case var x:
                        throw new JsonException($"Expected property name, got: '{(char)x}'");
                }
            }
        }

    ReadIndexAsString:
        Reader.Advance();
        _scratch.Clear();
        var span = Reader.LexUtf8Span(_scratch);
        var localIndex = serdeInfo.TryGetIndex(span);
        errorName = localIndex == IDeserializeType.IndexNotFound ? span.ToString() : null;
        _first = false;
        return localIndex;
    }
}