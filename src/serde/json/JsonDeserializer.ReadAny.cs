
using System;
using System.Diagnostics.CodeAnalysis;
using Serde.IO;
using static Serde.Json.ThrowHelpers;

namespace Serde.Json;

internal sealed partial class JsonDeserializer<TReader> : IDeserializer
    where TReader : IByteReader
{
    public T ReadAny<T>(IDeserializeVisitor<T> v)
        where T : class
    {
        var peek = Reader.SkipWhitespace();
        T result;
        switch (ThrowIfEos(peek))
        {
            case (byte)'[':
                result = ReadEnumerable(v);
                break;

            case (byte)'-' or (>= (byte)'0' and <= (byte)'9'):
                var d = ReadF64();
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

    public T? ReadNullableRef<T, TProxy>(TProxy proxy)
        where T : class
        where TProxy : IDeserialize<T>
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

        public bool TryGetNext<T, TProxy>(TProxy proxy, [MaybeNullWhen(false)] out T next)
            where TProxy : IDeserialize<T>
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
                        next = proxy.Deserialize(_deserializer);
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

        public bool TryGetNextEntry<K, V, DK, DV>(DK dk, DV dv, [MaybeNullWhen(false)] out (K, V) next)
            where DK : IDeserialize<K>
            where DV : IDeserialize<V>
        {
            // Don't save state
            if (!TryGetNextKey<K, DK>(dk, out K? nextKey))
            {
                next = default;
                return false;
            }
            var nextValue = GetNextValue<V, DV>(dv);
            next = (nextKey, nextValue);
            return true;
        }

        public bool TryGetNextKey<K, D>(D d, [MaybeNullWhen(false)] out K next)
            where D : IDeserialize<K>
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
                        next = d.Deserialize(_deserializer);
                        _first = false;
                        return true;

                    default:
                        throw new JsonException("Expected property name, found: " + (char)peek);
                }
            }
        }

        public V GetNextValue<V, D>(D d) where D : IDeserialize<V>
        {
            var peek = ThrowIfEos(_deserializer.Reader.SkipWhitespace());
            if (peek != (byte)':')
            {
                throw new JsonException("Expected ':'");
            }
            _deserializer.Reader.Advance();
            return d.Deserialize(_deserializer);
        }
    }

}