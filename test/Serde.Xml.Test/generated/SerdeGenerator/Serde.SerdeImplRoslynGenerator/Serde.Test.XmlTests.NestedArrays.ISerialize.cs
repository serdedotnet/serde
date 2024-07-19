
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial class NestedArrays : Serde.ISerialize<Serde.Test.XmlTests.NestedArrays>
        {
            void ISerialize<Serde.Test.XmlTests.NestedArrays>.Serialize(Serde.Test.XmlTests.NestedArrays value, ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<NestedArrays>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<int[][][], Serde.ArrayWrap.SerializeImpl<int[][], Serde.ArrayWrap.SerializeImpl<int[], Serde.ArrayWrap.SerializeImpl<int, global::Serde.Int32Wrap>>>>(_l_serdeInfo, 0, value.A);
                type.End();
            }
        }
    }
}