//HintName: A.B.C.D.ISerialize.cs

#nullable enable
using System;
using Serde;

partial class A
{
    partial class B
    {
        partial class C
        {
            partial class D : Serde.ISerialize
            {
                void Serde.ISerialize.Serialize(ISerializer serializer)
                {
                    var type = serializer.SerializeType("D", 1);
                    type.SerializeField("field"u8, new Int32Wrap(this.Field));
                    type.End();
                }
            }
        }
    }
}