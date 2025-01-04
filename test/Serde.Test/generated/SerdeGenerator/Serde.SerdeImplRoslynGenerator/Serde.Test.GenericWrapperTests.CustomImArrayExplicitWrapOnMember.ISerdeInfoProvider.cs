
#nullable enable

namespace Serde.Test;

partial class GenericWrapperTests
{
    partial record struct CustomImArrayExplicitWrapOnMember : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "CustomImArrayExplicitWrapOnMember",
            typeof(Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
                ("a", global::Serde.SerdeInfoProvider.GetInfo<Serde.Test.GenericWrapperTests.CustomImArrayProxy.Serialize<int,global::Serde.Int32Proxy>>(), typeof(Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember).GetField("A")!)
            }
        );
    }
}