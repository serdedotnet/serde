//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial class C : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(C).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("map", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictProxy.Serialize<string,int,global::Serde.StringProxy,global::Serde.Int32Proxy>>(), typeof(C).GetField("Map")!)
    });
}