//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial class C : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "C",
        typeof(C).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("map", global::Serde.SerdeInfoProvider.GetInfo<Serde.DictWrap.SerializeImpl<string,global::Serde.StringWrap,int,global::Serde.Int32Wrap>>(), typeof(C).GetField("Map")!)
    });
}