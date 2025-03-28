﻿//HintName: Test.RecursiveWrap.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Test;

partial class RecursiveWrap : Serde.ISerializeProvider<Recursive>
{
    static ISerialize<Recursive> ISerializeProvider<Recursive>.Instance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<Recursive>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Test.RecursiveWrap.s_serdeInfo;

        void global::Serde.ISerialize<Recursive>.Serialize(Recursive value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteValueIfNotNull<Recursive?, Test.RecursiveWrap>(_l_info, 0, value.Next);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
