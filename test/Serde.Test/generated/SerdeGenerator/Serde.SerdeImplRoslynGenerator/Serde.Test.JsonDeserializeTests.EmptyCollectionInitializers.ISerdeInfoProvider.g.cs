
#nullable enable

namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial class EmptyCollectionInitializers
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "EmptyCollectionInitializers",
            typeof(Serde.Test.JsonDeserializeTests.EmptyCollectionInitializers).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("items", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Generic.List<int>, Serde.ListProxy.De<int, global::Serde.I32Proxy>>())
                {
                    IsOptional = true,
                },
                new("arr", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int[], Serde.ArrayProxy.De<int, global::Serde.I32Proxy>>())
                {
                    IsOptional = true,
                }
            }
        );
    }
}
