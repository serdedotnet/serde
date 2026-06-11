//HintName: Mine.C.ISerdeInfoProvider.g.cs

#nullable enable

namespace Mine;

partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
    typeof(Mine.C).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("field", global::Serde.SerdeInfoProvider.GetDeserializeInfo<Other.Color, Other.ColorProxy>(), typeof(Mine.C).GetField("Field")),
        ("y", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(Mine.C).GetField("Y"))
    }
    );
}
