//HintName: S.ISerdeInfoProvider.g.cs

#nullable enable
partial struct S
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "S",
        typeof(S).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("sections", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Immutable.ImmutableArray<System.Collections.Specialized.BitVector32.Section>, Serde.ImmutableArrayProxy.Ser<System.Collections.Specialized.BitVector32.Section, Outer.SectionWrap>>())
            {
                MemberInfo = typeof(S).GetField("Sections"),
            }
        }
    );
}
