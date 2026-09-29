
#nullable enable

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial class EmptyInitializedProxy
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "InitializedTarget",
            typeof(Serde.Test.SerdeInfoTests.InitializedTarget).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("number", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>()),
                new("text", global::Serde.SerdeInfoProvider.GetDeserializeInfo<string, global::Serde.StringProxy>())
            }
        );
    }
}
