//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(C).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("s", global::Serde.SerdeInfoProvider.GetSerializeInfo<S<int>, SWrap.Ser<int, global::Serde.I32Proxy>>())
            {
                MemberInfo = typeof(C).GetField("S"),
            }
        }
    );
}
