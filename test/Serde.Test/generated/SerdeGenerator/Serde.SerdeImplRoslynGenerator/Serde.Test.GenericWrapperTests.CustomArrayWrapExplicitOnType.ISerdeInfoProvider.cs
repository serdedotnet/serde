
#nullable enable
namespace Serde.Test;
partial class GenericWrapperTests
{
    partial record struct CustomArrayWrapExplicitOnType : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "CustomArrayWrapExplicitOnType",
        typeof(Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("a", global::Serde.SerdeInfoProvider.GetInfo<Serde.Test.GenericWrapperTests.CustomImArray2Wrap.SerializeImpl<int,global::Serde.Int32Wrap>>(), typeof(Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType).GetField("A")!)
    });
}
}