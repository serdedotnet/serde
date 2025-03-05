//HintName: C2.ISerialize.cs

#nullable enable

using System;
using Serde;
partial class C2 : Serde.ISerializeProvider<C2>
{
    static ISerialize<C2> ISerializeProvider<C2>.SerializeInstance
        => C2SerializeProxy.Instance;

    sealed partial class C2SerializeProxy :Serde.ISerialize<C2>
    {
        void global::Serde.ISerialize<C2>.Serialize(C2 value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<C2>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteField<System.Collections.Generic.Dictionary<string, C>, Serde.DictProxy.Serialize<string,C,global::Serde.StringProxy,C>>(_l_info, 0, value.Map);
            _l_type.End(_l_info);
        }
        public static readonly C2SerializeProxy Instance = new();
        private C2SerializeProxy() { }

    }
}
