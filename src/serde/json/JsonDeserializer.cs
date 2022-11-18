
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
                Reader = new Utf8JsonReader(bytes, isFinalBlock: true, default)
            };
        }

        private void SaveState(in Utf8JsonReader reader)
        {
            _state.Reader = reader;
        }

        private Utf8JsonReader GetReader()
        {
            return _state.Reader;
        }

        public async ValueTask<T> DeserializeAny<T, V>(V v) where V : class, IDeserializeVisitor<T>
        {
            var reader = GetReader();
            await reader.ReadOrThrow();
            ValueTask<T> result;
            switch (reader.TokenType)
            {
                case JsonTokenType.StartArray:
                    result = DeserializeEnumerable<T, V>(v);
                    break;

                case JsonTokenType.Number:
                    result = DeserializeI64<T, V>(v);
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
            return await result;
        }

        public ValueTask<T> DeserializeBool<T, V>(V v) where V : class, IDeserializeVisitor<T>
        {
            var reader = GetReader();
            reader.ReadOrThrow();
            bool b = reader.GetBoolean();
            SaveState(reader);
            return ValueTask.FromResult(v.VisitBool(b));
        }

        public async ValueTask<T> DeserializeDictionary<T, V>(V v) where V : class, IDeserializeVisitor<T>
        {
            var reader = GetReader();
            await reader.ReadOrThrow();

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new InvalidDeserializeValueException("Expected object start");
            }

            SaveState(reader);
            var map = new DeDictionary(this);
            return await v.VisitDictionary(map);
        }

        public ValueTask<T> DeserializeFloat<T, V>(V v) where V : class, IDeserializeVisitor<T>
            => DeserializeDouble<T, V>(v);

        public ValueTask<T> DeserializeDouble<T, V>(V v) where V : class, IDeserializeVisitor<T>
        {
            var reader = GetReader();
            var d = reader.GetDouble();
            SaveState(reader);
            return ValueTask.FromResult(v.VisitDouble(d));
        }

        public ValueTask<T> DeserializeDecimal<T, V>(V v) where V : class, IDeserializeVisitor<T>
        {
            var reader = GetReader();
            var d = reader.GetDecimal();
            SaveState(reader);
            return ValueTask.FromResult(v.VisitDecimal(d));
        }

        public ValueTask<T> DeserializeEnumerable<T, V>(V v) where V : class, IDeserializeVisitor<T>
        {
            var reader = GetReader();
            reader.ReadOrThrow();

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new InvalidDeserializeValueException("Expected array start");
            }

            SaveState(reader);
            var enumerable = new DeEnumerable(this);
            return v.VisitEnumerable(enumerable);
        }

        private struct DeEnumerable : IDeserializeEnumerable
        {
            private JsonDeserializer _deserializer;
            public DeEnumerable(JsonDeserializer de)
            {
                _deserializer = de;
            }
            public int? SizeOpt => null;

            public async ValueTask<Option<T>> TryGetNext<T, D>()
                where D : IDeserialize<T>
            {
                var reader = _deserializer.GetReader();
                // Check if the next token is the end of the array, but don't advance the stream if not
                await reader.ReadOrThrow();
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    _deserializer.SaveState(reader);
                    return default;
                }
                // Don't save state
                return await D.Deserialize(_deserializer);
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

            public async ValueTask<Option<(K, V)>> TryGetNextEntry<K, DK, V, DV>()
                where DK : IDeserialize<K>
                where DV : IDeserialize<V>
            {
                // Don't save state
                if (await TryGetNextKey<K, DK>() is var key && !key.HasValue)
                {
                    return default;
                }
                var nextValue = await GetNextValue<V, DV>();
                return (key.Value, nextValue);
            }

            public async ValueTask<Option<K>> TryGetNextKey<K, D>() where D : IDeserialize<K>
            {
                while (true)
                {
                    var reader = _deserializer.GetReader();
                    await reader.ReadOrThrow();
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.EndObject:
                            // Check if the next token is the end of the object, but don't advance the stream if not
                            _deserializer.SaveState(reader);
                            return Option<K>.None;
                        case JsonTokenType.PropertyName:
                            return await D.Deserialize(_deserializer);
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

            public ValueTask<V> GetNextValue<V, D>() where D : IDeserialize<V>
            {
                return D.Deserialize(_deserializer);
            }
        }

        public ValueTask<T> DeserializeSByte<T, V>(V v) where V : class, IDeserializeVisitor<T>
            => DeserializeI64<T, V>(v);

        public ValueTask<T> DeserializeI16<T, V>(V v) where V : class, IDeserializeVisitor<T>
            => DeserializeI64<T, V>(v);


        public ValueTask<T> DeserializeI32<T, V>(V v) where V : class, IDeserializeVisitor<T>
            => DeserializeI64<T, V>(v);

        public async ValueTask<T> DeserializeI64<T, V>(V v) where V : class, IDeserializeVisitor<T>
        {
            var reader = GetReader();
            await reader.ReadOrThrow();
            var i64 = reader.GetInt64();
            SaveState(reader);
            return v.VisitI64(i64);
        }

        public async ValueTask<T> DeserializeString<T, V>(V v) where V : class, IDeserializeVisitor<T>
        {
            var reader = GetReader();
            await reader.ReadOrThrow();
            var s = reader.GetString();
            SaveState(reader);
            return s is null
                ? v.VisitNull()
                : v.VisitString(s);
        }

        public ValueTask<T> DeserializeIdentifier<T, V>(V v) where V : class, IDeserializeVisitor<T>
            => DeserializeString<T, V>(v);

        public ValueTask<T> DeserializeType<T, V>(string typeName, ReadOnlySpan<string> fieldNames, V v) where V : class, IDeserializeVisitor<T>
        {
            // Types are identical to dictionaries
            return DeserializeDictionary<T, V>(v);
        }

        public ValueTask<T> DeserializeByte<T, V>(V v) where V : class, IDeserializeVisitor<T>
            => DeserializeU64<T, V>(v);

        public ValueTask<T> DeserializeU16<T, V>(V v) where V : class, IDeserializeVisitor<T>
            => DeserializeU64<T, V>(v);

        public ValueTask<T> DeserializeU32<T, V>(V v) where V : class, IDeserializeVisitor<T>
            => DeserializeU64<T, V>(v);

        public async ValueTask<T> DeserializeU64<T, V>(V v) where V : class, IDeserializeVisitor<T>
        {
            var reader = GetReader();
            await reader.ReadOrThrow();
            var u64 = reader.GetUInt64();
            SaveState(reader);
            return v.VisitU64(u64);
        }

        public ValueTask<T> DeserializeChar<T, V>(V v) where V : class, IDeserializeVisitor<T>
            => DeserializeString<T, V>(v);

        public async ValueTask<T> DeserializeNullableRef<T, V>(V v)
            where V : class, IDeserializeVisitor<T>
        {
            var reader = GetReader();
            await reader.ReadOrThrow();
            if (reader.TokenType == JsonTokenType.Null)
            {
                return v.VisitNull();
            }
            else
            {
                return await v.VisitNotNull(this);
            }
        }
    }

    internal static class Utf8JsonReaderExtensions
    {
        public static ValueTask ReadOrThrow(ref this Utf8JsonReader reader)
        {
            if (!reader.Read())
            {
                throw new InvalidDeserializeValueException("Unexpected end of stream");
            }

            return ValueTask.CompletedTask;
        }
    }
}