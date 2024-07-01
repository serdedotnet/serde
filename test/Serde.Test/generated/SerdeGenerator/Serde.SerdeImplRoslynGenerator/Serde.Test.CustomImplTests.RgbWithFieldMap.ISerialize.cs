
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class CustomImplTests
    {
        partial record RgbWithFieldMap : Serde.ISerialize<Serde.Test.CustomImplTests.RgbWithFieldMap>
        {
            void ISerialize<Serde.Test.CustomImplTests.RgbWithFieldMap>.Serialize(Serde.Test.CustomImplTests.RgbWithFieldMap value, ISerializer serializer)
            {
                var _l_typeInfo = RgbWithFieldMapSerdeTypeInfo.TypeInfo;
                var type = serializer.SerializeType(_l_typeInfo);
                type.SerializeField<int, Int32Wrap>(_l_typeInfo, 0, this.Red);
                type.SerializeField<int, Int32Wrap>(_l_typeInfo, 1, this.Green);
                type.SerializeField<int, Int32Wrap>(_l_typeInfo, 2, this.Blue);
                type.End();
            }
        }
    }
}