
using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Serde.Json
{
    internal sealed partial class JsonDeserializer : IDeserializer
    {
        private Utf8JsonReader Reader;

        public static JsonDeserializer FromString(string s)
        {
            return new JsonDeserializer(Encoding.UTF8.GetBytes(s));
        }

        private JsonDeserializer(byte[] bytes)
        {
            Reader = new Utf8JsonReader(bytes, default);
        }

        private void SaveState(in Utf8JsonReader reader)
        {
            Reader = reader;
        }

        private ref Utf8JsonReader GetReader()
        {
            return ref Reader;
        }

        public T DeserializeAny<T>(IDeserializeVisitor<T> v)
        {
            var reader = GetReader();
            reader.ReadOrThrow();
            T result;
            switch (reader.TokenType)
            {
                case JsonTokenType.StartArray:
                    result = DeserializeEnumerable(v);
                    break;

                case JsonTokenType.Number:
                    result = DeserializeDouble(v);
                    break;

                case JsonTokenType.StartObject:
                    result = DeserializeDictionary(v);
                    break;

                case JsonTokenType.String:
                    result = DeserializeString(v);
                    break;

                case JsonTokenType.True:
                case JsonTokenType.False:
                    result = DeserializeBool(v);
                    break;

                default:
                    throw new InvalidDeserializeValueException($"Could not deserialize '{reader.TokenType}");
            }
            return result;
        }

        public T DeserializeBool<T>(IDeserializeVisitor<T> v)
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();
            bool b = reader.GetBoolean();
            return v.VisitBool(b);
        }

        public T DeserializeDictionary<T>(IDeserializeVisitor<T> v)
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

        public IDeserializeCollection DeserializeCollection(TypeInfo typeInfo)
        {
            if (typeInfo.Kind is not (TypeInfo.TypeKind.Enumerable or TypeInfo.TypeKind.Dictionary))
            {
                throw new ArgumentException($"TypeKind is {typeInfo.Kind}, expected Enumerable or Dictionary");
            }

            ref var reader = ref GetReader();
            reader.ReadOrThrow();

            if (typeInfo.Kind == TypeInfo.TypeKind.Dictionary && reader.TokenType != JsonTokenType.StartObject
                || typeInfo.Kind == TypeInfo.TypeKind.Enumerable && reader.TokenType != JsonTokenType.StartArray)
            {
                throw new InvalidDeserializeValueException("Expected object start");
            }

            return new DeCollection(this);
        }

        private struct DeCollection : IDeserializeCollection
        {
            private JsonDeserializer _deserializer;
            public DeCollection(JsonDeserializer de)
            {
                _deserializer = de;
            }

            public int? SizeOpt => null;

            public bool TryReadValue<T, D>(TypeInfo typeInfo, [MaybeNullWhen(false)] out T next) where D : IDeserialize<T>
            {
                var reader = _deserializer.GetReader();
                reader.ReadOrThrow();
                switch (reader.TokenType)
                {
                    case JsonTokenType.EndArray:
                        if (typeInfo.Kind != TypeInfo.TypeKind.Enumerable)
                        {
                            throw new InvalidDeserializeValueException($"Unexpected end of array in type kind: {typeInfo.Kind}");
                        }
                        break;
                    case JsonTokenType.EndObject:
                        if (typeInfo.Kind != TypeInfo.TypeKind.Dictionary)
                        {
                            throw new InvalidDeserializeValueException($"Unexpected end of object in type kind: {typeInfo.Kind}");
                        }
                        break;
                    default:
                        next = D.Deserialize(_deserializer);
                        return true;
                }
                _deserializer.SaveState(reader);
                next = default;
                _deserializer = null!;
                return false;
            }
        }

        public T DeserializeFloat<T>(IDeserializeVisitor<T> v)
            => DeserializeDouble(v);

        public T DeserializeDouble<T>(IDeserializeVisitor<T> v)
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();
            var d = reader.GetDouble();
            return v.VisitDouble(d);
        }

        public T DeserializeDecimal<T>(IDeserializeVisitor<T> v)
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();
            var d = reader.GetDecimal();
            return v.VisitDecimal(d);
        }

        public T DeserializeEnumerable<T>(IDeserializeVisitor<T> v)
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
                next = D.Deserialize(_deserializer);
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
                            next = D.Deserialize(_deserializer);
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
                return D.Deserialize(_deserializer);
            }
        }

        public T DeserializeSByte<T>(IDeserializeVisitor<T> v)
            => DeserializeI64(v);

        public T DeserializeI16<T>(IDeserializeVisitor<T> v)
            => DeserializeI64(v);


        public T DeserializeI32<T>(IDeserializeVisitor<T> v)
            => DeserializeI64(v);

        public T DeserializeI64<T>(IDeserializeVisitor<T> v)
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();
            var i64 = reader.GetInt64();
            return v.VisitI64(i64);
        }

        public T DeserializeString<T>(IDeserializeVisitor<T> v)
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

        public T DeserializeIdentifier<T>(IDeserializeVisitor<T> v)
            => DeserializeString(v);

        public IDeserializeType DeserializeType(TypeInfo fieldMap)
        {
            // Custom types look like dictionaries, enums are inline strings
            if (fieldMap.Kind == TypeInfo.TypeKind.CustomType)
            {
                ref var reader = ref GetReader();
                reader.ReadOrThrow();

                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new InvalidDeserializeValueException("Expected object start");
                }
            }
            else if (fieldMap.Kind != TypeInfo.TypeKind.Enum)
            {
                throw new ArgumentException("Expected either CustomType or Enum kind, found " + fieldMap.Kind);
            }

            return this;
        }

        public T DeserializeByte<T>(IDeserializeVisitor<T> v)
            => DeserializeU64(v);

        public T DeserializeU16<T>(IDeserializeVisitor<T> v)
            => DeserializeU64(v);

        public T DeserializeU32<T>(IDeserializeVisitor<T> v)
            => DeserializeU64(v);

        public T DeserializeU64<T>(IDeserializeVisitor<T> v)
        {
            ref var reader = ref GetReader();
            reader.ReadOrThrow();
            var u64 = reader.GetUInt64();
            return v.VisitU64(u64);
        }

        public T DeserializeChar<T>(IDeserializeVisitor<T> v)
            => DeserializeString(v);

        public T DeserializeNullableRef<T>(IDeserializeVisitor<T> v)
        {
            var reader = GetReader();
            reader.ReadOrThrow();
            if (reader.TokenType == JsonTokenType.Null)
            {
                SaveState(reader);
                return v.VisitNull();
            }
            else
            {
                return v.VisitNotNull(this);
            }
        }
    }

    partial class JsonDeserializer : IDeserializeType
    {
        V IDeserializeType.ReadValue<V, D>(int index)
        {
            return D.Deserialize(this);
        }

        int IDeserializeType.TryReadIndex(TypeInfo map, out string? errorName)
        {
            ref var reader = ref GetReader();
            if (map.Kind == TypeInfo.TypeKind.Enum)
            {
                // Enums are just treated as strings
                reader.ReadOrThrow();
                Debug.Assert(reader.TokenType == JsonTokenType.String);
            }
            else
            {
                bool foundProperty = false;
                while (!foundProperty)
                {
                    reader.ReadOrThrow();
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.EndObject:
                            errorName = null;
                            return IDeserializeType.EndOfType;
                        case JsonTokenType.PropertyName:
                            foundProperty = true;
                            break;
                        default:
                            // If we aren't at a property name, we must be at a value and intending to skip it
                            // Call Skip in case we are starting a new array or object. Doesn't do
                            // anything for bare tokens, but we've already read one token forward above,
                            // so we can simply continue
                            reader.Skip();
                            break;
                    }
                }
                Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);
            }

            Utf8Span span;
            if (reader.HasValueSequence || reader.ValueIsEscaped)
            {
                var s = reader.GetString()!;
                span = Encoding.UTF8.GetBytes(s);
            }
            else
            {
                span = reader.ValueSpan;
            }
            var index = map.TryGetIndex(span);
            errorName = index == IDeserializeType.IndexNotFound ? span.ToString() : null;
            return index;
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