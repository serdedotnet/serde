//HintName: StringWrap.IDeserialize.cs

#nullable enable

using System;
using Serde;
partial record StringWrap : Serde.IDeserializeProvider<StringWrap>
{
    static IDeserialize<StringWrap> IDeserializeProvider<StringWrap>.Instance
        => _DeObj.Instance;

    sealed partial class _DeObj  : global::Serde.IDeserialize<StringWrap>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => StringWrap.s_serdeInfo;

        StringWrap global::Serde.IDeserialize<StringWrap>.Deserialize(global::Serde.IDeserializer deserializer)
        {
            var deObj = global::Serde.DeserializeProvider.GetDeserialize<string, global::Serde.StringProxy>();
            return new(deObj.Deserialize(deserializer));
        }
        public static readonly _DeObj Instance = new();
        private _DeObj() { }

    }
}
