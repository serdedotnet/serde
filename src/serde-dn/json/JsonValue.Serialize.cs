
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Serde.Json
{
    partial record JsonValue : ISerialize
    {
        public abstract void Serialize(ISerializer serializer);


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
                var type = serializer.SerializeType("", Members.Count);
                foreach (var (name, node) in Members.OrderBy(kvp => kvp.Key))
                {
                    type.SerializeField(name, node);
                }
                type.End();
            }
        }

        partial record Array
        {
            public Array(IEnumerable<JsonValue> elements)
                : this(elements.ToImmutableArray())
            { }

            public override void Serialize(ISerializer serializer)
            {
                var enumerable = serializer.SerializeEnumerable("JsonValue", Elements.Length);
                foreach (var element in Elements)
                {
                    enumerable.SerializeElement(element);
                }
                enumerable.End();
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