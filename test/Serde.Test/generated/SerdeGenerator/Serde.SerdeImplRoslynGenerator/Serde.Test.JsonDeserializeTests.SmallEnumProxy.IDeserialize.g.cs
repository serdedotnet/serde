
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial class SmallEnumProxy : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SmallEnum>
    {
        Serde.Test.JsonDeserializeTests.SmallEnum IDeserialize<Serde.Test.JsonDeserializeTests.SmallEnum>.Deserialize(IDeserializer deserializer)
        {
            var serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var index = deserializer.ReadEnum(serdeInfo);
            Serde.Test.JsonDeserializeTests.SmallEnum _l_result = index switch {
                0 => Serde.Test.JsonDeserializeTests.SmallEnum.A,
                1 => Serde.Test.JsonDeserializeTests.SmallEnum.B,
                _ => throw new InvalidOperationException($"Unexpected index: {index}")
            };
            return _l_result;
        }
    }
}
