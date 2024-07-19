
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
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<RgbWithFieldMap>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<int, global::Serde.Int32Wrap>(_l_serdeInfo, 0, value.Red);
                type.SerializeField<int, global::Serde.Int32Wrap>(_l_serdeInfo, 1, value.Green);
                type.SerializeField<int, global::Serde.Int32Wrap>(_l_serdeInfo, 2, value.Blue);
                type.End();
            }
        }
    }
}