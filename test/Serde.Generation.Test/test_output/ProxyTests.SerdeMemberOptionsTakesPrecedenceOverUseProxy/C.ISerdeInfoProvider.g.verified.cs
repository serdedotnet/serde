//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
    typeof(C).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("s", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Runtime.InteropServices.ComTypes.BIND_OPTS, Proxy2>(), typeof(C).GetField("S"))
    }
    );
}
