﻿//HintName: ColorLongSerdeTypeInfo.cs
internal static class ColorLongSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(
        "ColorLong",
        Serde.TypeInfo.TypeKind.Enum,
        new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorLong).GetField("Red")!),
("green", typeof(ColorLong).GetField("Green")!),
("blue", typeof(ColorLong).GetField("Blue")!)
    });
}