//HintName: Rgb.ISerialize.cs

#nullable enable

using System;
using Serde;
partial struct Rgb : Serde.ISerializeProvider<Rgb>
{
    static ISerialize<Rgb> ISerializeProvider<Rgb>.Instance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<Rgb>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Rgb.s_serdeInfo;

        void global::Serde.ISerialize<Rgb>.Serialize(Rgb value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteU8(_l_info, 0, value.Red);
            _l_type.WriteU8(_l_info, 1, value.Blue);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
