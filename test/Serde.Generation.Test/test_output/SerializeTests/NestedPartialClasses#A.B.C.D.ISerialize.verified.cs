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
            partial class D : Serde.ISerialize<A.B.C.D>
            {
                void ISerialize<A.B.C.D>.Serialize(A.B.C.D value, ISerializer serializer)
                {
                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<D>();
                    var type = serializer.SerializeType(_l_serdeInfo);
                    type.SerializeField<int, global::Serde.Int32Wrap>(_l_serdeInfo, 0, value.Field);
                    type.End();
                }
            }
        }
    }
}