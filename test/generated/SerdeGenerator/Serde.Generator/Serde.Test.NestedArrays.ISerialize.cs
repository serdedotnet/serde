
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class NestedArrays : Serde.ISerialize
    {
        static string ITypeName.TypeName => "NestedArrays";
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType("NestedArrays", 1);
            type.SerializeField("A", new ArrayWrap.SerializeImpl<int[][], ArrayWrap.SerializeImpl<int[], ArrayWrap.SerializeImpl<int, Int32Wrap>>>(this.A));
            type.End();
        }
    }
}