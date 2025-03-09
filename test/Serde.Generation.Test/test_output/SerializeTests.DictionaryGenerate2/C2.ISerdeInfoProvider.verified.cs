//HintName: C2.ISerdeInfoProvider.cs

#nullable enable
partial class C2 : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "C2",
        typeof(C2).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("map", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictProxy.Serialize<string, C, global::Serde.StringProxy, C>>(), typeof(C2).GetField("Map"))
        }
    );
}