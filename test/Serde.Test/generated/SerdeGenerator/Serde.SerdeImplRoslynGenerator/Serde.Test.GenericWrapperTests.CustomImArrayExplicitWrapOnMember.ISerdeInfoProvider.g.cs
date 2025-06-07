
#nullable enable

namespace Serde.Test;

partial class GenericWrapperTests
{
    partial record struct CustomImArrayExplicitWrapOnMember
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "CustomImArrayExplicitWrapOnMember",
        typeof(Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("a", global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.GenericWrapperTests.CustomImArray<int>, Serde.Test.GenericWrapperTests.CustomImArrayProxy.Ser<int, global::Serde.I32Proxy>>(), typeof(Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember).GetField("A"))
        }
        );
    }
}
