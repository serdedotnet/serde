//HintName: C2.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial class C2
{
    sealed partial class _SerObj : Serde.ISerialize<C2>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => C2.s_serdeInfo;

        void global::Serde.ISerialize<C2>.Serialize(C2 value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteValue<System.Collections.Generic.Dictionary<string, C>, Serde.DictProxy.Ser<string, C, global::Serde.StringProxy, C>>(_l_info, 0, value.Map);
            _l_type.End(_l_info);
        }

    }
}
