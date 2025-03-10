﻿//HintName: Rgb.ISerdeInfoProvider.cs

#nullable enable
partial struct Rgb : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "Rgb",
        typeof(Rgb).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("red", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.U8Proxy>(), typeof(Rgb).GetField("Red")),
            ("green", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.U8Proxy>(), typeof(Rgb).GetField("Green")),
            ("blue", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.U8Proxy>(), typeof(Rgb).GetField("Blue"))
        }
    );
}