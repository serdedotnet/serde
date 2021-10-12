
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace Serde.Json
{
    public sealed partial class JsonDeserializer : IDeserializer
    {
        private byte[] _utf8Bytes;
        private JsonReaderState _readerState;
        private int _offset;

        public static JsonDeserializer FromString(string s)
        {
            return new JsonDeserializer(Encoding.UTF8.GetBytes(s));
        }

        private JsonDeserializer(byte[] bytes)
        {
            _utf8Bytes = bytes;
            _readerState = default;
            _offset = 0;
        }

        private void SaveState(in Utf8JsonReader reader)
        {
            _readerState = reader.CurrentState;
            _offset += (int)reader.BytesConsumed;
        }

        private Utf8JsonReader GetReader()
        {
            return new Utf8JsonReader(_utf8Bytes.AsSpan()[_offset..], isFinalBlock: true, _readerState);
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
            return result;
        }

        public T DeserializeBool<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            var reader = GetReader();
            reader.ReadOrThrow();
            bool b = reader.GetBoolean();
            SaveState(reader);
            return v.VisitBool(b);
        }

        public T DeserializeByte<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            var reader = GetReader();
            var b = reader.GetByte();
            SaveState(reader);
            return v.VisitByte(b);
        }

        public T DeserializeDictionary<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            var reader = GetReader();
            reader.ReadOrThrow();

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new InvalidDeserializeValueException("Expected object start");
            }

            SaveState(reader);
            var map = new DeDictionary(this);
            return v.VisitDictionary(ref map);
        }

        public T DeserializeDouble<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            var reader = GetReader();
            var d = reader.GetDouble();
            _readerState = reader.CurrentState;
            return v.VisitDouble(d);
        }

        public T DeserializeEnumerable<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            var reader = GetReader();
            reader.ReadOrThrow();

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new InvalidDeserializeValueException("Expected array start");
            }

            SaveState(reader);
            var enumerable = new DeEnumerable(this);
            return v.VisitEnumerable(ref enumerable);
        }

        private readonly struct DeEnumerable : IDeserializeEnumerable
        {
            private readonly JsonDeserializer _deserializer;
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

        private readonly struct DeDictionary : IDeserializeDictionary
        {
            private readonly JsonDeserializer _deserializer;
            public DeDictionary(JsonDeserializer de)
            {
                _deserializer = de;
            }

            public int? SizeOpt => null;

            public bool TryGetNextEntry<K, V, DK, DV>([MaybeNullWhen(false)] out (K, V) next)
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
                var reader = _deserializer.GetReader();
                // Check if the next token is the end of the object, but don't advance the stream if not
                reader.ReadOrThrow();
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    _deserializer.SaveState(reader);
                    next = default;
                    return false;
                }
                next = D.Deserialize(_deserializer);
                return true;
            }

            public V GetNextValue<V, D>() where D : IDeserialize<V>
            {
                return D.Deserialize(_deserializer);
            }
        }

        public T DeserializeFloat<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            throw new System.NotImplementedException();
        }

        public T DeserializeI16<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            throw new System.NotImplementedException();
        }

        public T DeserializeI32<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            throw new System.NotImplementedException();
        }

        public T DeserializeI64<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            var reader = GetReader();
            reader.ReadOrThrow();
            var i64 = reader.GetInt64();
            SaveState(reader);
            return v.VisitI64(i64);
        }

        public T DeserializeSByte<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            throw new System.NotImplementedException();
        }

        public T DeserializeString<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            var reader = GetReader();
            reader.ReadOrThrow();
            var s = reader.GetString();
            SaveState(reader);
            return v.VisitString(s!);
        }

        public T DeserializeType<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            // Types are identical to dictionaries
            return DeserializeDictionary<T, V>(v);
        }

        public T DeserializeU16<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            throw new System.NotImplementedException();
        }

        public T DeserializeU32<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            throw new System.NotImplementedException();
        }

        public T DeserializeU64<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            throw new System.NotImplementedException();
        }

        public T DeserializeChar<T, V>(V v) where V : IDeserializeVisitor<T>
        {
            throw new System.NotImplementedException();
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