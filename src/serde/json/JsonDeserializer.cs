
using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Serde.Json
{
    internal partial struct JsonDeserializer : IDeserializer
    {
        // Need to use a class so it can be referenced from the Enumerable and
        // Dictionary implementations
        private sealed class DeserializerState
        {
            public Utf8JsonReader Reader;
        }
        private readonly DeserializerState _state;

        public static JsonDeserializer FromString(string s)
        {
            return new JsonDeserializer(Encoding.UTF8.GetBytes(s));
        }

        private JsonDeserializer(byte[] bytes)
        {
            _state = new DeserializerState
            {
                Reader = new Utf8JsonReader(bytes, default)
            };
        }

        private void SaveState(in Utf8JsonReader reader)
        {
            _state.Reader = reader;
        }

        private ref Utf8JsonReader GetReader()
        {
            return ref _state.Reader;
        }

        public T DeserializeAny<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            var reader = GetReader();
            reader.ReadOrThrow();
            T result;
            switch (reader.TokenType)
            {
                case JsonTokenType.StartArray:
                    result = DeserializeEnumerable<T, V>(v);
                    break;

                case JsonTokenType.Number:
                    result = DeserializeDouble<T, V>(v);
                    break;

                case JsonTokenType.StartObject:
                    result = DeserializeDictionary<T, V>(v);
                    break;

                case JsonTokenType.String:
                    result = DeserializeString<T, V>(v);
                    break;

                case JsonTokenType.True:
                case JsonTokenType.False:
                    result = DeserializeBool<T, V>(v);
                    break;

                default:
                    throw new InvalidDeserializeValueException($"Could not deserialize '{reader.TokenType}");
            }
            return result;
        }

        public T DeserializeBool<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();
            bool b = reader.GetBoolean();
            return v.VisitBool(b);
        }

        public T DeserializeDictionary<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new InvalidDeserializeValueException("Expected object start");
            }

            var map = new DeDictionary(this);
            return v.VisitDictionary(ref map);
        }

        public T DeserializeFloat<T, V>(V v) where V : IDeserializeVisitor<T>
            => DeserializeDouble<T, V>(v);

        public T DeserializeDouble<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();
            var d = reader.GetDouble();
            return v.VisitDouble(d);
        }

        public T DeserializeDecimal<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();
            var d = reader.GetDecimal();
            return v.VisitDecimal(d);
        }

        public T DeserializeEnumerable<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new InvalidDeserializeValueException("Expected array start");
            }

            var enumerable = new DeEnumerable(this);
            return v.VisitEnumerable(ref enumerable);
        }

        private struct DeEnumerable : IDeserializeEnumerable
        {
            private JsonDeserializer _deserializer;
            public DeEnumerable(JsonDeserializer de)
            {
                _deserializer = de;
            }
            public int? SizeOpt => null;

            public bool TryGetNext<T, D>([MaybeNullWhen(false)] out T next)
                where D : IDeserialize<T>
            {
                var reader = _deserializer.GetReader();
                // Check if the next token is the end of the array, but don't advance the stream if not
                reader.ReadOrThrow();
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    _deserializer.SaveState(reader);
                    next = default;
                    return false;
                }
                // Don't save state
                next = D.Deserialize(ref _deserializer);
                return true;
            }
        }

        private struct DeDictionary : IDeserializeDictionary
        {
            private JsonDeserializer _deserializer;
            public DeDictionary(JsonDeserializer de)
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
                    var reader = _deserializer.GetReader();
                    reader.ReadOrThrow();
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.EndObject:
                            // Check if the next token is the end of the object, but don't advance the stream if not
                            _deserializer.SaveState(reader);
                            next = default;
                            return false;
                        case JsonTokenType.PropertyName:
                            next = D.Deserialize(ref _deserializer);
                            return true;
                        default:
                            // If we aren't at a property name, we must be at a value and intending to skip it
                            // Call Skip in case we are starting a new array or object. Doesn't do
                            // anything for bare tokens, but we've already read one token forward above,
                            // so we can simply save the state and continue
                            reader.Skip();
                            _deserializer.SaveState(reader);
                            break;
                    }
                }
            }

            public V GetNextValue<V, D>() where D : IDeserialize<V>
            {
                return D.Deserialize(ref _deserializer);
            }
        }

        public T DeserializeSByte<T, V>(V v) where V : IDeserializeVisitor<T>
            => DeserializeI64<T, V>(v);

        public T DeserializeI16<T, V>(V v) where V : IDeserializeVisitor<T>
            => DeserializeI64<T, V>(v);


        public T DeserializeI32<T, V>(V v) where V : IDeserializeVisitor<T>
            => DeserializeI64<T, V>(v);

        public T DeserializeI64<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();
            var i64 = reader.GetInt64();
            return v.VisitI64(i64);
        }

        public T DeserializeString<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();
            if (reader.HasValueSequence || reader.ValueIsEscaped)
            {
                var s = reader.GetString()!;
                return v.VisitString(s);
            }
            else
            {
                var result = v.VisitUtf8Span(reader.ValueSpan);
                return result;
            }
        }

        public T DeserializeIdentifier<T, V>(V v) where V : IDeserializeVisitor<T>
            => DeserializeString<T, V>(v);

        public T DeserializeType<T, V>(string typeName, ReadOnlySpan<string> fieldNames, V v) where V : IDeserializeVisitor<T>
        {
            // Types are identical to dictionaries
            return DeserializeDictionary<T, V>(v);
        }

        public T DeserializeByte<T, V>(V v) where V : IDeserializeVisitor<T>
            => DeserializeU64<T, V>(v);

        public T DeserializeU16<T, V>(V v) where V : IDeserializeVisitor<T>
            => DeserializeU64<T, V>(v);

        public T DeserializeU32<T, V>(V v) where V : IDeserializeVisitor<T>
            => DeserializeU64<T, V>(v);

        public T DeserializeU64<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();
            var u64 = reader.GetUInt64();
            return v.VisitU64(u64);
        }

        public T DeserializeChar<T, V>(V v) where V : IDeserializeVisitor<T>
            => DeserializeString<T, V>(v);

        public T DeserializeNullableRef<T, V>(V v)
            where V : IDeserializeVisitor<T>
        {
            var reader = GetReader();
            reader.ReadOrThrow();
            if (reader.TokenType == JsonTokenType.Null)
            {
                return v.VisitNull();
            }
            else
            {
                var deserializer = this;
                return v.VisitNotNull(ref deserializer);
            }
        }
    }

    internal static class Utf8JsonReaderExtensions
    {
        public static void ReadOrThrow(ref this Utf8JsonReader reader)
        {
            if (!reader.Read())
            {
                throw new InvalidDeserializeValueException("Unexpected end of stream");
            }
        }
    }
}