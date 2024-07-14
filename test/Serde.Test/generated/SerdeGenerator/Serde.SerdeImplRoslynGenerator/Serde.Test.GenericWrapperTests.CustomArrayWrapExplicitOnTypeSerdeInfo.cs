namespace Serde.Test;
partial class GenericWrapperTests
{
    internal static class CustomArrayWrapExplicitOnTypeSerdeInfo
{
    internal static readonly Serde.SerdeInfo Instance = Serde.SerdeInfo.Create(
        "CustomArrayWrapExplicitOnType",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, System.Reflection.MemberInfo)[] {
("a", typeof(Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType).GetField("A")!)
    });
}
}