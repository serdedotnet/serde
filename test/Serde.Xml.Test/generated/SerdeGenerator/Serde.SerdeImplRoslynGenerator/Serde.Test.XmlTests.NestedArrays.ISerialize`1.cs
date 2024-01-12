
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
                var type = serializer.SerializeType("NestedArrays", 1);
                type.SerializeField<int[][][], Serde.ArrayWrap.SerializeImpl<int[][], Serde.ArrayWrap.SerializeImpl<int[], Serde.ArrayWrap.SerializeImpl<int, Int32Wrap>>>>("A", value.A);
                type.End();
            }
        }
    }
}