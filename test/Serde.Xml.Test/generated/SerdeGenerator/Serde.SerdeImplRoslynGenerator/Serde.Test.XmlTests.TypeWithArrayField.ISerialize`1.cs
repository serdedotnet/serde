
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial class TypeWithArrayField : Serde.ISerialize<Serde.Test.XmlTests.TypeWithArrayField>
        {
            void ISerialize<Serde.Test.XmlTests.TypeWithArrayField>.Serialize(Serde.Test.XmlTests.TypeWithArrayField value, ISerializer serializer)
            {
                var type = serializer.SerializeType("TypeWithArrayField", 1);
                type.SerializeField<Serde.Test.XmlTests.StructWithIntField[], Serde.ArrayWrap.SerializeImpl<Serde.Test.XmlTests.StructWithIntField, IdWrap<Serde.Test.XmlTests.StructWithIntField>>>("ArrayField", value.ArrayField);
                type.End();
            }
        }
    }
}