
#nullable enable

namespace Serde.Test;

partial class RoundtripTests
{
    partial record UnicodeNote
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "UnicodeNote",
            typeof(Serde.Test.RoundtripTests.UnicodeNote).GetCustomAttributesData(),
            new global::Serde.SerdeInfo.FieldInfo[] {
                new("text", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>())
            }
        );
    }
}
