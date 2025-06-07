//HintName: A.ISerdeInfoProvider.g.cs

#nullable enable
partial class A
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "A",
    typeof(A).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("x", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(A).GetField("X"))
    }
    );
}
