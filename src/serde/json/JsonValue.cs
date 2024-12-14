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
        internal sealed class UnionInfo : ISerdeInfo, IUnionSerdeInfo
        {
            public static readonly UnionInfo Instance = new UnionInfo();
            public string Name => nameof(JsonValue);

            public int FieldCount => 0;

            public IList<CustomAttributeData> Attributes => [];

            internal static readonly ISerdeInfo ObjectInfo = SerdeInfo.MakeDictionary(nameof(Object));
            internal static readonly ISerdeInfo ArrayInfo = SerdeInfo.MakeEnumerable(nameof(Array));

            public IEnumerable<ISerdeInfo> CaseInfos { get; } = [
                SerdeInfo.MakePrimitive(nameof(Number)),
                SerdeInfo.MakePrimitive(nameof(Bool)),
                SerdeInfo.MakePrimitive(nameof(String)),
                ObjectInfo,
                ArrayInfo,
                SerdeInfo.MakePrimitive(nameof(Null)),
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

        partial record Number
        {
            public override string ToString() => Value.ToString();
        }

        partial record Bool
        {
            public override string ToString() => Value.ToString();
        }
        partial record String
        {
            public override string ToString() => Value;
        }

        partial record Array
        {
            public Array(IEnumerable<JsonValue> elements)
                : this(elements.ToImmutableArray())
            { }

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

        partial record Object
        {
            public Object(IEnumerable<KeyValuePair<string, JsonValue>> members)
                : this(members.ToImmutableDictionary())
            { }

            public Object((string FieldName, JsonValue Value)[] members)
                : this(members.ToImmutableDictionary(t => t.FieldName, t => t.Value))
            { }

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

        partial record Null
        {
            public static readonly Null Instance = new Null();
            private Null() { }
        }
    }
}