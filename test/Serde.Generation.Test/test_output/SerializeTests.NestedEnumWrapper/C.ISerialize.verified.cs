﻿//HintName: C.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class C : Serde.ISerializeProvider<C>
{
    static ISerialize<C> ISerializeProvider<C>.SerializeInstance
        => CSerializeProxy.Instance;

    sealed partial class CSerializeProxy :Serde.ISerialize<C>
    {
        void global::Serde.ISerialize<C>.Serialize(C value, global::Serde.ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<C>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeFieldIfNotNull<Rgb?,Serde.NullableProxy.Serialize<Rgb,RgbProxy>>(_l_serdeInfo,0,value.ColorOpt);
            type.End();
        }
        public static readonly CSerializeProxy Instance = new();
        private CSerializeProxy() { }

    }
}
