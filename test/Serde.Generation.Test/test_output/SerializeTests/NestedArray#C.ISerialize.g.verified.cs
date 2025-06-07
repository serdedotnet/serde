//HintName: C.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial class C
{
    sealed partial class _SerObj : Serde.ISerialize<C>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => C.s_serdeInfo;

        void global::Serde.ISerialize<C>.Serialize(C value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteValue<int[][], Serde.ArrayProxy.Ser<int[], Serde.ArrayProxy.Ser<int, global::Serde.I32Proxy>>>(_l_info, 0, value.NestedArr);
            _l_type.End(_l_info);
        }

    }
}
