//HintName: Serde.Test.AllInOne.ColorEnumProxy.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial record AllInOne
    {
        sealed partial class ColorEnumProxy : Serde.ISerialize<Serde.Test.AllInOne.ColorEnum>, Serde.ISerializeProvider<Serde.Test.AllInOne.ColorEnum>
        {
            void ISerialize<Serde.Test.AllInOne.ColorEnum>.Serialize(Serde.Test.AllInOne.ColorEnum value, ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorEnumProxy>();
                var index = value switch
                {
                    Serde.Test.AllInOne.ColorEnum.Red => 0,
                    Serde.Test.AllInOne.ColorEnum.Blue => 1,
                    Serde.Test.AllInOne.ColorEnum.Green => 2,
                    var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorEnum'"),
                };
                serializer.SerializeEnumValue(_l_serdeInfo, index, (int)value, global::Serde.Int32Proxy.Instance);
            }

            static ISerialize<Serde.Test.AllInOne.ColorEnum> ISerializeProvider<Serde.Test.AllInOne.ColorEnum>.SerializeInstance => Serde.Test.AllInOne.ColorEnumProxy.Instance;
        }
    }
}