//HintName: A.B.C.D.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial class A
{
    partial class B
    {
        partial class C
        {
            partial class D
            {
                sealed partial class _SerObj : Serde.ISerialize<A.B.C.D>
                {
                    global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => A.B.C.D.s_serdeInfo;

                    void global::Serde.ISerialize<A.B.C.D>.Serialize(A.B.C.D value, global::Serde.ISerializer serializer)
                    {
                        var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                        var _l_type = serializer.WriteType(_l_info);
                        _l_type.WriteI32(_l_info, 0, value.Field);
                        _l_type.End(_l_info);
                    }

                }
            }
        }
    }
}
