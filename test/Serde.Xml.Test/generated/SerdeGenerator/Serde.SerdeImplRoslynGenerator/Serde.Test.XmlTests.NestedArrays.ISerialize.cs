
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial class NestedArrays : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("NestedArrays", 1);
                type.SerializeField("A", new ArrayWrap.SerializeImpl<int[][], ArrayWrap.SerializeImpl<int[], ArrayWrap.SerializeImpl<int, Int32Wrap>>>(this.A));
                type.End();
            }
        }
    }
}