using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Serde.Json
{
    // definition
    public abstract partial record JsonValue
    {
        public sealed partial record Number(double Value) : JsonValue;
        public sealed partial record Bool(bool Value) : JsonValue;
        public sealed partial record String(string Value) : JsonValue;
        public sealed partial record Object(ImmutableDictionary<string, JsonValue> Members) : JsonValue;
        public sealed partial record Array(ImmutableArray<JsonValue> Elements) : JsonValue;
        public sealed partial record Null : JsonValue;
    }

    // helpers
    partial record JsonValue
    {
        static ISerdeInfo ISerdeInfoProvider.SerdeInfo => UnionInfo.Instance;

        private sealed class UnionInfo : ISerdeInfo, IUnionSerdeInfo
        {
            public static readonly UnionInfo Instance = new UnionInfo();
            public string Name => nameof(JsonValue);

            public int FieldCount => 0;

            public IList<CustomAttributeData> Attributes => [];

            public IEnumerable<ISerdeInfo> CaseInfos { get; } = [
                SerdeInfoProvider.GetInfo<JsonValue.Number>(),
                SerdeInfoProvider.GetInfo<JsonValue.Bool>(),
                SerdeInfoProvider.GetInfo<JsonValue.String>(),
                SerdeInfoProvider.GetInfo<JsonValue.Object>(),
                SerdeInfoProvider.GetInfo<JsonValue.Array>(),
                SerdeInfoProvider.GetInfo<JsonValue.Null>(),
            ];

            public IList<CustomAttributeData> GetFieldAttributes(int index) => throw GetOOR(index);

            public ISerdeInfo GetFieldInfo(int index) => throw GetOOR(index);

            public Utf8Span GetFieldName(int index) => throw GetOOR(index);

            public string GetFieldStringName(int index) => throw GetOOR(index);

            public int TryGetIndex(Utf8Span fieldName) => IDeserializeType.IndexNotFound;

            private ArgumentOutOfRangeException GetOOR(int index)
            {
                return new ArgumentOutOfRangeException(nameof(index), index, $"{Name} has no fields or properties.");
            }
        }

        private JsonValue() { }

        public static implicit operator JsonValue(int i) => new Number(i);
        public static implicit operator JsonValue(string s) => new String(s);

        partial record Number : ISerdeInfoProvider
        {
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(nameof(Number));
            public override string ToString() => Value.ToString();
        }

        partial record Bool : ISerdeInfoProvider
        {
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(nameof(Bool));
            public override string ToString() => Value.ToString();
        }
        partial record String : ISerdeInfoProvider
        {
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(nameof(String));
            public override string ToString() => Value;
        }

        partial record Array : ISerdeInfoProvider
        {
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnumerable(nameof(Array));
            public static readonly Array Empty = new Array(ImmutableArray<JsonValue>.Empty);

            public bool Equals(Array? other)
            {
                if (other is null)
                {
                    return false;
                }

                return Elements.AsSpan().SequenceEqual(other.Elements.AsSpan());
            }

            public override int GetHashCode()
            {
                int hash = 0;
                foreach (var e in Elements)
                {
                    hash = HashCode.Combine(hash, e.GetHashCode());
                }
                return hash;
            }

            public override string ToString()
            {
                var builder = new StringBuilder();
                builder.Append("[ ");
                bool start = true;
                foreach (var e in Elements)
                {
                    if (!start)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(e.ToString());
                    start = false;
                }
                builder.Append(" ]");
                return builder.ToString();
            }
        }

        partial record Object : ISerdeInfoProvider
        {
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeDictionary(nameof(Object));
            public bool Equals(Object? other)
            {
                if (other is null || Members.Count != other.Members.Count)
                {
                    return false;
                }

                foreach (var (k, v) in other.Members)
                {
                    if (!Members.TryGetValue(k, out var otherVal) || !v.Equals(otherVal))
                    {
                        return false;
                    }
                }
                return true;
            }

            public override int GetHashCode()
            {
                int hash = 0;
                foreach (var (k, v) in Members)
                {
                    hash = HashCode.Combine(hash, k, v);
                }
                return hash;
            }

            public override string ToString()
            {
                var builder = new StringBuilder();
                builder.Append("{ ");
                bool start = true;
                foreach (var (k, v) in Members)
                {
                    if (!start)
                    {
                        builder.Append(", ");
                    }
                    builder.Append($"\"{k}\": {v}");
                    start = false;
                }
                builder.Append(" }");
                return builder.ToString();
            }
        }

        partial record Null : ISerdeInfoProvider
        {
            static ISerdeInfo ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakePrimitive(nameof(Null));
            public static readonly Null Instance = new Null();
            private Null() { }
        }
    }
}