
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
("a", global::Serde.SerdeInfoProvider.GetInfo<Serde.Test.GenericWrapperTests.CustomImArrayWrap.SerializeImpl<int,global::Serde.Int32Wrap>>(), typeof(Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember).GetField("A")!)
    });
}
}