
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Serde.Json
{
    partial record JsonValue : ISerialize<JsonValue>
    {
        public abstract void Serialize(ISerializer serializer);

        void ISerialize<JsonValue>.Serialize(JsonValue value, ISerializer serializer)
        {
            value.Serialize(serializer);
        }

        partial record Number
        {
            public override void Serialize(ISerializer serializer)
            {
                serializer.SerializeDouble(Value);
            }
        }

        partial record Bool
        {
            public override void Serialize(ISerializer serializer)
            {
                serializer.SerializeBool(Value);
            }
        }

        partial record String
        {
            public override void Serialize(ISerializer serializer)
            {
                serializer.SerializeString(Value);
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

            public override void Serialize(ISerializer serializer)
            {
                var typeInfo = SerdeInfoProvider.GetInfo<Object>();
                var dict = serializer.SerializeCollection(typeInfo, Members.Count);
                foreach (var (name, node) in Members.OrderBy(kvp => kvp.Key))
                {
                    dict.SerializeElement(name, default(StringWrap));
                    dict.SerializeElement(node, node);
                }
                dict.End(typeInfo);
            }
        }

        partial record Array
        {
            public Array(IEnumerable<JsonValue> elements)
                : this(elements.ToImmutableArray())
            { }

            public override void Serialize(ISerializer serializer)
            {
                var typeInfo = SerdeInfoProvider.GetInfo<Array>();
                var enumerable = serializer.SerializeCollection(typeInfo, Elements.Length);
                foreach (var element in Elements)
                {
                    enumerable.SerializeElement(element, element);
                }
                enumerable.End(typeInfo);
            }
        }

        partial record Null
        {
            public override void Serialize(ISerializer serializer)
            {
                serializer.SerializeNull();
            }
        }
    }
}