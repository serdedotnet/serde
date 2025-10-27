
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
            var _l_type = serializer.WriteType(_l_info);
            var index = value switch
            {
                Serde.Test.AllInOne.ColorEnum.Red => 0,
                Serde.Test.AllInOne.ColorEnum.Blue => 1,
                Serde.Test.AllInOne.ColorEnum.Green => 2,
                var v => throw new InvalidOperationException($"Cannot serialize unnamed enum value '{v}' of enum 'ColorEnum'"),
            };
            _l_type.WriteI32(_l_info, index, (int)value);
            _l_type.End(_l_info);
        }
        Serde.Test.AllInOne.ColorEnum IDeserialize<Serde.Test.AllInOne.ColorEnum>.Deserialize(IDeserializer deserializer)
        {
            var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            using var de = deserializer.ReadType(serdeInfo);
            var (index, errorName) = de.TryReadIndexWithName(serdeInfo);
            if (index == ITypeDeserializer.IndexNotFound)
            {
                throw Serde.DeserializeException.UnknownMember(errorName!, serdeInfo);
            }
            if (index == ITypeDeserializer.EndOfType)
            {
                // Assume we want to read the underlying value
                return (Serde.Test.AllInOne.ColorEnum)de.ReadI32(serdeInfo, index);
            }
            return index switch {
                0 => Serde.Test.AllInOne.ColorEnum.Red,
                1 => Serde.Test.AllInOne.ColorEnum.Blue,
                2 => Serde.Test.AllInOne.ColorEnum.Green,
                _ => throw new InvalidOperationException($"Unexpected index: {index}")
            };
        }
    }
}
