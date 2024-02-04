//HintName: A.B.C.D.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial class A
{
    partial class B
    {
        partial class C
        {
            partial class D : Serde.ISerialize<A.B.C.D>
            {
                void ISerialize<A.B.C.D>.Serialize(A.B.C.D value, ISerializer serializer)
                {
                    var type = serializer.SerializeType("D", 1);
                    type.SerializeField<int, Int32Wrap>("field", value.Field);
                    type.End();
                }
            }
        }
    }
}