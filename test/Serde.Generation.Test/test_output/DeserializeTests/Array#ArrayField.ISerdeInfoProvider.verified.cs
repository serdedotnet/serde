//HintName: ArrayField.ISerdeInfoProvider.cs

#nullable enable
partial class ArrayField : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "ArrayField",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("intArr", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.DeserializeImpl<int,global::Serde.Int32Wrap>>(), typeof(ArrayField).GetField("IntArr")!)
    });
}