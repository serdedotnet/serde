//HintName: StringWrap.ISerialize.cs

#nullable enable

using System;
using Serde;
partial record StringWrap : Serde.ISerializeProvider<StringWrap>
{
    static ISerialize<StringWrap> ISerializeProvider<StringWrap>.Instance
        => _SerObj.Instance;

    sealed partial class _SerObj  : global::Serde.ISerialize<StringWrap>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => StringWrap.s_serdeInfo;

        void global::Serde.ISerialize<StringWrap>.Serialize(StringWrap value, global::Serde.ISerializer serializer)
        {
            var serObj = global::Serde.SerializeProvider.GetSerialize<string, global::Serde.StringProxy>();
            serObj.Serialize(value.Value, serializer);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
