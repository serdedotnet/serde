//HintName: Mine.C.ISerdeInfoProvider.g.cs

#nullable enable

namespace Mine;

partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(Mine.C).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("field", global::Serde.SerdeInfoProvider.GetDeserializeInfo<Other.Color, Other.ColorProxy>()),
            new("y", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>())
        }
    );
}
