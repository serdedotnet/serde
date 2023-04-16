
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial class TypeWithArrayField : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("TypeWithArrayField", 1);
                type.SerializeField("ArrayField", new ArrayWrap.SerializeImpl<Serde.Test.XmlTests.StructWithIntField, IdWrap<Serde.Test.XmlTests.StructWithIntField>>(this.ArrayField));
                type.End();
            }
        }
    }
}