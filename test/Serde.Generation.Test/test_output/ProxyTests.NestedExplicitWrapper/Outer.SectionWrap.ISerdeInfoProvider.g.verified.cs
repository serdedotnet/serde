//HintName: Outer.SectionWrap.ISerdeInfoProvider.g.cs

#nullable enable
partial class Outer
{
    partial record struct SectionWrap
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "Section",
        typeof(System.Collections.Specialized.BitVector32.Section).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("mask", global::Serde.SerdeInfoProvider.GetSerializeInfo<short, global::Serde.I16Proxy>(), typeof(System.Collections.Specialized.BitVector32.Section).GetProperty("Mask")),
            ("offset", global::Serde.SerdeInfoProvider.GetSerializeInfo<short, global::Serde.I16Proxy>(), typeof(System.Collections.Specialized.BitVector32.Section).GetProperty("Offset"))
        }
        );
    }
}
