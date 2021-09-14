
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Serde.Json
{
    internal abstract partial record JsonValue : ISerializeStatic
    {
        public abstract void Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            where TSerializer : ISerializerStatic<TSerializeType, TSerializeEnumerable, TSerializeDictionary>
            where TSerializeType : ISerializeTypeStatic
            where TSerializeEnumerable : ISerializeEnumerableStatic
            where TSerializeDictionary : ISerializeDictionaryStatic;

        public abstract void Serialize(ISerializer serializer);

        partial record Number : ISerializeStatic
        {
            public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            {
                serializer.SerializeDouble(Value);
            }

            public override void Serialize(ISerializer serializer)
            {
                serializer.SerializeDouble(Value);
            }
        }

        partial record Bool
        {
            public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            {
                serializer.SerializeBool(Value);
            }

            public override void Serialize(ISerializer serializer)
            {
                serializer.SerializeBool(Value);
            }
        }
        partial record String
        {
            public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            {
                serializer.SerializeString(Value);
            }

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

            public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            {
                var type = serializer.SerializeType("", Members.Count);
                foreach (var (name, node) in Members)
                {
                    type.SerializeField(name, node);
                }
                type.End();
            }

            public override void Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("", Members.Count);
                foreach (var (name, node) in Members)
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

            public override void Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
            {
                var enumerable = serializer.SerializeEnumerable(Elements.Length);
                foreach (var element in Elements)
                {
                    enumerable.SerializeElement(element);
                }
                enumerable.End();
            }

            public override void Serialize(ISerializer serializer)
            {
                var enumerable = serializer.SerializeEnumerable(Elements.Length);
                foreach (var element in Elements)
                {
                    enumerable.SerializeElement(element);
                }
                enumerable.End();
            }
        }
    }
}