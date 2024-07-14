
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial struct BoolStruct : Serde.ISerialize<Serde.Test.XmlTests.BoolStruct>
        {
            void ISerialize<Serde.Test.XmlTests.BoolStruct>.Serialize(Serde.Test.XmlTests.BoolStruct value, ISerializer serializer)
            {
                var _l_serdeInfo = BoolStructSerdeTypeInfo.TypeInfo;
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<bool, BoolWrap>(_l_serdeInfo, 0, value.BoolField);
                type.End();
            }
        }
    }
}