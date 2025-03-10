//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial class C : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(C).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("nestedArr", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayProxy.Serialize<int[], Serde.ArrayProxy.Serialize<int, global::Serde.I32Proxy>>>(), typeof(C).GetField("NestedArr"))
        }
    );
}