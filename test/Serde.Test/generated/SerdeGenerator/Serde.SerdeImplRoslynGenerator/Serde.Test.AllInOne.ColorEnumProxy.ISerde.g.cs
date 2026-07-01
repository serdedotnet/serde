
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial record AllInOne
{
    partial class ColorEnumProxy : Serde.ISerde<Serde.Test.AllInOne.ColorEnum>
    {
        void global::Serde.ISerialize<Serde.Test.AllInOne.ColorEnum>.Serialize(Serde.Test.AllInOne.ColorEnum value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_index = value switch
            {
                Serde.Test.AllInOne.ColorEnum.Red => 0,
                Serde.Test.AllInOne.ColorEnum.Blue => 1,
                Serde.Test.AllInOne.ColorEnum.Green => 2,
                var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorEnum'"),
            };
            serializer.WriteEnum(_l_info, _l_index);

        }
        Serde.Test.AllInOne.ColorEnum IDeserialize<Serde.Test.AllInOne.ColorEnum>.Deserialize(IDeserializer deserializer)
        {
            var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var index = deserializer.ReadEnum(serdeInfo);
            Serde.Test.AllInOne.ColorEnum _l_result = index switch {
                0 => Serde.Test.AllInOne.ColorEnum.Red,
                1 => Serde.Test.AllInOne.ColorEnum.Blue,
                2 => Serde.Test.AllInOne.ColorEnum.Green,
                _ => throw new InvalidOperationException($"Unexpected index: {index}")
            };
            return _l_result;
        }
    }
}
