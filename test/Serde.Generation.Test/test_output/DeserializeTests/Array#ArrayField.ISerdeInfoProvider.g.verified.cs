//HintName: ArrayField.ISerdeInfoProvider.g.cs

#nullable enable
partial class ArrayField
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "ArrayField",
        typeof(ArrayField).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("intArr", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int[], Serde.ArrayProxy.De<int, global::Serde.I32Proxy>>())
        }
    );
}
