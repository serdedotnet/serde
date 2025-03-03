//HintName: Rgb.ISerialize.cs

#nullable enable

using System;
using Serde;
partial struct Rgb : Serde.ISerializeProvider<Rgb>
{
    static ISerialize<Rgb> ISerializeProvider<Rgb>.SerializeInstance
        => RgbSerializeProxy.Instance;

    sealed partial class RgbSerializeProxy :Serde.ISerialize<Rgb>
    {
        void global::Serde.ISerialize<Rgb>.Serialize(Rgb value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<Rgb>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteByte(_l_info, 0, value.Red);
            _l_type.WriteByte(_l_info, 1, value.Green);
            _l_type.WriteByte(_l_info, 2, value.Blue);
            _l_type.End(_l_info);
        }
        public static readonly RgbSerializeProxy Instance = new();
        private RgbSerializeProxy() { }

    }
}
