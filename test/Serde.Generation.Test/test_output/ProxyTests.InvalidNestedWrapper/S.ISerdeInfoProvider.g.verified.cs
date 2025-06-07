//HintName: S.ISerdeInfoProvider.g.cs

#nullable enable
partial struct S
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "S",
    typeof(S).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("sections", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Generic.List<int>, Serde.ArrayProxy.Ser<System.Collections.Specialized.BitVector32.Section, Outer.SectionWrap>>(), typeof(S).GetField("Sections"))
    }
    );
}
