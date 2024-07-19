
#nullable enable
namespace Serde.Test;
partial class JsonDeserializeTests
{
    partial class NullableFields : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "NullableFields",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("s", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefWrap.DeserializeImpl<string,global::Serde.StringWrap>>(), typeof(Serde.Test.JsonDeserializeTests.NullableFields).GetField("S")!),
("dict", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictWrap.DeserializeImpl<string,global::Serde.StringWrap,string?,Serde.NullableRefWrap.DeserializeImpl<string,global::Serde.StringWrap>>>(), typeof(Serde.Test.JsonDeserializeTests.NullableFields).GetField("Dict")!)
    });
}
}