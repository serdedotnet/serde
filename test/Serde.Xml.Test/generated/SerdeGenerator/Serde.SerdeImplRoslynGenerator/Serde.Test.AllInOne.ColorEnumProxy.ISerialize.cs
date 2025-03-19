
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial record AllInOne
{
    sealed partial class ColorEnumProxy :Serde.ISerialize<Serde.Test.AllInOne.ColorEnum>,Serde.ISerializeProvider<Serde.Test.AllInOne.ColorEnum>
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
        static ISerialize<Serde.Test.AllInOne.ColorEnum> ISerializeProvider<Serde.Test.AllInOne.ColorEnum>.Instance
            => Serde.Test.AllInOne.ColorEnumProxy.Instance;

    }
}