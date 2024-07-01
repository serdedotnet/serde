//HintName: Serde.Test.AllInOne.ColorEnumWrap.ISerialize.cs

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
                var name = value switch
                {
                    Serde.Test.AllInOne.ColorEnum.Red => "red",
                    Serde.Test.AllInOne.ColorEnum.Blue => "blue",
                    Serde.Test.AllInOne.ColorEnum.Green => "green",
                    _ => null
                };
                serializer.SerializeEnumValue("ColorEnum", name, (int)value, default(Int32Wrap));
            }
        }
    }
}