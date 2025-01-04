//HintName: S1.ISerialize.cs

#nullable enable

using System;
using Serde;
partial struct S1 : Serde.ISerializeProvider<S1>
{
    static ISerialize<S1> ISerializeProvider<S1>.SerializeInstance
        => S1SerializeProxy.Instance;

    sealed partial class S1SerializeProxy :Serde.ISerialize<S1>
    {
        void global::Serde.ISerialize<S1>.Serialize(S1 value, global::Serde.ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<S1>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.End();
        }
        public static readonly S1SerializeProxy Instance = new();
        private S1SerializeProxy() { }

    }
}
