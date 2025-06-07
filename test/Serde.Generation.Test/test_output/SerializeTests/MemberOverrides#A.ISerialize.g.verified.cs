//HintName: A.ISerialize.g.cs

#nullable enable

using System;
using Serde;
partial record A
{
    sealed partial class _SerObj : Serde.ISerialize<A>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => A.s_serdeInfo;

        void global::Serde.ISerialize<A>.Serialize(A value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteI32(_l_info, 0, value.X);
            _l_type.End(_l_info);
        }

    }
}
