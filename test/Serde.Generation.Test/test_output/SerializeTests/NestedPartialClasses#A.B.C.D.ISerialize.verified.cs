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
            partial class D : Serde.ISerializeProvider<A.B.C.D>
            {
                static ISerialize<A.B.C.D> ISerializeProvider<A.B.C.D>.SerializeInstance
                    => DSerializeProxy.Instance;

                sealed partial class DSerializeProxy :Serde.ISerialize<A.B.C.D>
                {
                    void global::Serde.ISerialize<A.B.C.D>.Serialize(A.B.C.D value, global::Serde.ISerializer serializer)
                    {
                        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<D>();
                        var type = serializer.SerializeType(_l_serdeInfo);
                        type.SerializeField<int,global::Serde.Int32Proxy>(_l_serdeInfo,0,value.Field);
                        type.End();
                    }
                    public static readonly DSerializeProxy Instance = new();
                    private DSerializeProxy() { }

                }
            }
        }
    }
}
