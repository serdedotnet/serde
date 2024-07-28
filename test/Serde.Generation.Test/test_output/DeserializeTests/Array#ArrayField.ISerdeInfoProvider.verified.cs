//HintName: ArrayField.ISerdeInfoProvider.cs

#nullable enable
partial class ArrayField : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "ArrayField",
        typeof(ArrayField).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("intArr", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.DeserializeImpl<int,global::Serde.Int32Wrap>>(), typeof(ArrayField).GetField("IntArr")!)
    });
}