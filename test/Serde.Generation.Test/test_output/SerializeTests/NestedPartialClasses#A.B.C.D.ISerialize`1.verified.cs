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
                    var _l_typeInfo = DSerdeTypeInfo.TypeInfo;
                    var type = serializer.SerializeType(_l_typeInfo);
                    type.SerializeField<int, Int32Wrap>(_l_typeInfo, 0, value.Field);
                    type.End();
                }
            }
        }
    }
}