﻿//HintName: Some.Nested.Namespace.ColorULongSerdeTypeInfo.cs
namespace Some.Nested.Namespace;
internal static class ColorULongSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("red", typeof(ColorULong).GetField("Red")!),
("green", typeof(ColorULong).GetField("Green")!),
("blue", typeof(ColorULong).GetField("Blue")!)
    });
}