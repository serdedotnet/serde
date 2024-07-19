
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
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<TypeWithArrayField>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<Serde.Test.XmlTests.StructWithIntField[], Serde.ArrayWrap.SerializeImpl<Serde.Test.XmlTests.StructWithIntField, global::Serde.IdWrap<Serde.Test.XmlTests.StructWithIntField>>>(_l_serdeInfo, 0, value.ArrayField);
                type.End();
            }
        }
    }
}