
#nullable enable
namespace Serde.Test;
partial class SampleTest
{
    partial record Address : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "Address",
        typeof(Serde.Test.SampleTest.Address).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("Name", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.SampleTest.Address).GetField("Name")!),
("Line1", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.SampleTest.Address).GetField("Line1")!),
("City", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefWrap.SerializeImpl<string,global::Serde.StringWrap>>(), typeof(Serde.Test.SampleTest.Address).GetField("City")!),
("State", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.SampleTest.Address).GetField("State")!),
("Zip", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.SampleTest.Address).GetField("Zip")!)
    });
}
}