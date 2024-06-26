﻿
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial record AllInOne
    {
        partial struct ColorEnumWrap : Serde.IDeserialize<Serde.Test.AllInOne.ColorEnum>
        {
            static Serde.Test.AllInOne.ColorEnum IDeserialize<Serde.Test.AllInOne.ColorEnum>.Deserialize(IDeserializer deserializer)
            {
                var typeInfo = Serde.Test.AllInOne.ColorEnumSerdeTypeInfo.TypeInfo;
                var de = deserializer.DeserializeType(typeInfo);
                int index;
                if ((index = de.TryReadIndex(typeInfo, out var errorName)) == IDeserializeType.IndexNotFound)
                {
                    throw new InvalidDeserializeValueException($"Unexpected value: {errorName}");
                }

                return index switch
                {
                    0 => Serde.Test.AllInOne.ColorEnum.Red,
                    1 => Serde.Test.AllInOne.ColorEnum.Blue,
                    2 => Serde.Test.AllInOne.ColorEnum.Green,
                    _ => throw new InvalidDeserializeValueException($"Unexpected index: {index}")};
            }
        }
    }
}