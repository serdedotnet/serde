//HintName: Serde.AllInOneColorEnumWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Serde
{
    partial record struct AllInOneColorEnumWrap : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var name = Value switch
            {
                Serde.Test.AllInOne.ColorEnum.Red => "red",
                Serde.Test.AllInOne.ColorEnum.Blue => "blue",
                Serde.Test.AllInOne.ColorEnum.Green => "green",
                _ => null
            };
            serializer.SerializeEnumValue("ColorEnum", name, new Int32Wrap((int)Value));
        }
    }
}