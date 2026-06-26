//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(C).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("s", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OPTSWrap>())
            {
                MemberInfo = typeof(C).GetField("S"),
            }
        }
    );
}
