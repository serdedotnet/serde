﻿//HintName: Some.Nested.Namespace.ColorIntProxy.ISerdeInfoProvider.cs

#nullable enable

namespace Some.Nested.Namespace;

partial class ColorIntProxy : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "ColorInt",
        typeof(Some.Nested.Namespace.ColorInt).GetCustomAttributesData(),
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.I32Proxy>(),
        new (string, System.Reflection.MemberInfo?)[] {
            ("red", typeof(Some.Nested.Namespace.ColorInt).GetField("Red")),
            ("green", typeof(Some.Nested.Namespace.ColorInt).GetField("Green")),
            ("blue", typeof(Some.Nested.Namespace.ColorInt).GetField("Blue"))
        }
    );
}