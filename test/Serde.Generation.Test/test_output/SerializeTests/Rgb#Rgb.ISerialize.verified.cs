//HintName: Rgb.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct Rgb : Serde.ISerializeProvider<Rgb>
{
    static ISerialize<Rgb> ISerializeProvider<Rgb>.SerializeInstance => RgbSerializeProxy.Instance;

    sealed class RgbSerializeProxy : Serde.ISerialize<Rgb>
    {
        void ISerialize<Rgb>.Serialize(Rgb value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Rgb>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<byte, global::Serde.ByteProxy>(_l_serdeInfo, 0, value.Red);
            type.SerializeField<byte, global::Serde.ByteProxy>(_l_serdeInfo, 1, value.Green);
            type.SerializeField<byte, global::Serde.ByteProxy>(_l_serdeInfo, 2, value.Blue);
            type.End();
        }

        public static readonly RgbSerializeProxy Instance = new();
        private RgbSerializeProxy()
        {
        }
    }
}