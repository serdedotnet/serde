//HintName: RgbSerdeTypeInfo.cs
internal static class RgbSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<Rgb>(nameof(Rgb), new (string, System.Reflection.MemberInfo)[] {
        ("Red", typeof(Rgb).GetField("Red")!),
("Green", typeof(Rgb).GetField("Green")!),
("Blue", typeof(Rgb).GetField("Blue")!)
    });
}