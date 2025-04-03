//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial class C
{
    private static readonly global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
    typeof(C).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("s", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OPTSWrap>(), typeof(C).GetField("S"))
    }
    );
}