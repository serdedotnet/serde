//HintName: RgbSerdeTypeInfo.cs
internal static class RgbSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create([
        ("red", typeof(Rgb).GetField("Red")!),
("blue", typeof(Rgb).GetField("Blue")!)
    ]);
}