
#nullable enable
namespace Serde.Test;
partial class JsonSerializerTests
{
    partial class NullableFields : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "NullableFields",
        typeof(Serde.Test.JsonSerializerTests.NullableFields).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("s", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefWrap.SerializeImpl<string,global::Serde.StringWrap>>(), typeof(Serde.Test.JsonSerializerTests.NullableFields).GetField("S")!),
("d", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictWrap.SerializeImpl<string,global::Serde.StringWrap,string?,Serde.NullableRefWrap.SerializeImpl<string,global::Serde.StringWrap>>>(), typeof(Serde.Test.JsonSerializerTests.NullableFields).GetField("D")!)
    });
}
}