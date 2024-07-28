
#nullable enable
namespace Serde.Test;
partial class SerdeInfoTests
{
    partial record EmptyRecord : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "EmptyRecord",
        typeof(Serde.Test.SerdeInfoTests.EmptyRecord).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {

    });
}
}