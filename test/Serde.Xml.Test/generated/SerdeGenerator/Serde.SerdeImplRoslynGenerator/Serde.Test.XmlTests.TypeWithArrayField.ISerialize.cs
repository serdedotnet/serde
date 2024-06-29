
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
                var _l_typeInfo = TypeWithArrayFieldSerdeTypeInfo.TypeInfo;
                var type = serializer.SerializeType(_l_typeInfo);
                type.SerializeField<Serde.Test.XmlTests.StructWithIntField[], Serde.ArrayWrap.SerializeImpl<Serde.Test.XmlTests.StructWithIntField, IdWrap<Serde.Test.XmlTests.StructWithIntField>>>(_l_typeInfo, 0, this.ArrayField);
                type.End();
            }
        }
    }
}