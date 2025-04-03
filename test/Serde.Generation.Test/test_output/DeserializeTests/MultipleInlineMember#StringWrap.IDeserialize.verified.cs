//HintName: StringWrap.IDeserialize.cs

#nullable enable

using System;
using Serde;
partial record StringWrap : Serde.IDeserializeProvider<StringWrap>
{
    static IDeserialize<StringWrap> IDeserializeProvider<StringWrap>.Instance
        => _DeObj.Instance;

    sealed partial class _DeObj : global::Serde.IDeserialize<StringWrap>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => StringWrap.s_serdeInfo;

        public static readonly _DeObj Instance = new();
        private _DeObj() { }

    }
}
