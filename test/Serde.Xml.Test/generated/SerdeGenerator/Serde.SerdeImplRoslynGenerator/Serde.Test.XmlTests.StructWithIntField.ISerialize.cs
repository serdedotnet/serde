
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial record StructWithIntField : Serde.ISerialize<Serde.Test.XmlTests.StructWithIntField>
        {
            void ISerialize<Serde.Test.XmlTests.StructWithIntField>.Serialize(Serde.Test.XmlTests.StructWithIntField value, ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<StructWithIntField>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<int, global::Serde.Int32Wrap>(_l_serdeInfo, 0, value.X);
                type.End();
            }
        }
    }
}