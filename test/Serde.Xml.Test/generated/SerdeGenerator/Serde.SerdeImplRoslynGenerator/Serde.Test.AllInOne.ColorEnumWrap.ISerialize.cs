
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial record AllInOne
    {
        partial struct ColorEnumWrap : Serde.ISerialize<Serde.Test.AllInOne.ColorEnum>
        {
            void ISerialize<Serde.Test.AllInOne.ColorEnum>.Serialize(Serde.Test.AllInOne.ColorEnum value, ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ColorEnumWrap>();
                var index = value switch
                {
                    Serde.Test.AllInOne.ColorEnum.Red => 0,
                    Serde.Test.AllInOne.ColorEnum.Blue => 1,
                    Serde.Test.AllInOne.ColorEnum.Green => 2,
                    var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorEnum'"),
                };
                serializer.SerializeEnumValue(_l_serdeInfo, index, (int)value, default(global::Serde.Int32Wrap));
            }
        }
    }
}