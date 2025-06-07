//HintName: S1.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial struct S1
{
    sealed partial class _SerObj : Serde.ISerialize<S1>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => S1.s_serdeInfo;

        void global::Serde.ISerialize<S1>.Serialize(S1 value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.End(_l_info);
        }

    }
}
