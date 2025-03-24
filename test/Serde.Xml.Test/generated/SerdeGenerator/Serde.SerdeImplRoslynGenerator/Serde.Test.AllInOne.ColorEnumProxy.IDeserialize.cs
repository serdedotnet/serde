
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial record AllInOne
{
    sealed partial class ColorEnumProxy :Serde.IDeserialize<Serde.Test.AllInOne.ColorEnum>,Serde.IDeserializeProvider<Serde.Test.AllInOne.ColorEnum>
    {
        Serde.Test.AllInOne.ColorEnum IDeserialize<Serde.Test.AllInOne.ColorEnum>.Deserialize(IDeserializer deserializer)
        {
            var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var de = deserializer.ReadType(serdeInfo);
            int index;
            if ((index = de.TryReadIndex(serdeInfo, out var errorName)) == ITypeDeserializer.IndexNotFound)
            {
                throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
            }
            return index switch {
                0 => Serde.Test.AllInOne.ColorEnum.Red,
                1 => Serde.Test.AllInOne.ColorEnum.Blue,
                2 => Serde.Test.AllInOne.ColorEnum.Green,
                _ => throw new InvalidOperationException($"Unexpected index: {index}")
            };
        }
        static IDeserialize<Serde.Test.AllInOne.ColorEnum> IDeserializeProvider<Serde.Test.AllInOne.ColorEnum>.Instance
            => Serde.Test.AllInOne.ColorEnumProxy.Instance;

    }
}