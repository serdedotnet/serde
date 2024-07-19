
#nullable enable
namespace Serde.Test;
partial class GenericWrapperTests
{
    partial record struct CustomArrayWrapExplicitOnType : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "CustomArrayWrapExplicitOnType",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("a", global::Serde.SerdeInfoProvider.GetInfo<Serde.Test.GenericWrapperTests.CustomImArray2Wrap.SerializeImpl<int,global::Serde.Int32Wrap>>(), typeof(Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType).GetField("A")!)
    });
}
}