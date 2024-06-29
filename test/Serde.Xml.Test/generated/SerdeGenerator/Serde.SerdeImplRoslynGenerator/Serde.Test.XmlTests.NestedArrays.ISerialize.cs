
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
                var _l_typeInfo = NestedArraysSerdeTypeInfo.TypeInfo;
                var type = serializer.SerializeType(_l_typeInfo);
                type.SerializeField<int[][][], Serde.ArrayWrap.SerializeImpl<int[][], Serde.ArrayWrap.SerializeImpl<int[], Serde.ArrayWrap.SerializeImpl<int, Int32Wrap>>>>(_l_typeInfo, 0, this.A);
                type.End();
            }
        }
    }
}