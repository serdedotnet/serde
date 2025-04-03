//HintName: StringWrap.ISerialize.cs

#nullable enable

using System;
using Serde;
partial record StringWrap : Serde.ISerializeProvider<StringWrap>
{
    static ISerialize<StringWrap> ISerializeProvider<StringWrap>.Instance
        => _SerObj.Instance;

    sealed partial class _SerObj  : Serde.ISerialize<StringWrap>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => StringWrap.s_serdeInfo;

        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
