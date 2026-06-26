//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(C).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("s", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Runtime.InteropServices.ComTypes.BIND_OPTS, Proxy2>())
            {
                MemberInfo = typeof(C).GetField("S"),
            }
        }
    );
}
