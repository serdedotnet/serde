
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
                var type = serializer.SerializeType("BoolStruct", 1);
                type.SerializeField<bool, BoolWrap>("BoolField", value.BoolField);
                type.End();
            }
        }
    }
}