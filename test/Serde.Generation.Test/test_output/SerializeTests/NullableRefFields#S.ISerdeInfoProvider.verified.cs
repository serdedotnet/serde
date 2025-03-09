﻿//HintName: S.ISerdeInfoProvider.cs

#nullable enable
partial struct S<T1, T2, T3, T4, T5> : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "S",
        typeof(S<,,,,>).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("fS", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefProxy.Serialize<string, global::Serde.StringProxy>>(), typeof(S<,,,,>).GetField("FS")),
            ("f1", global::Serde.SerdeInfoProvider.GetInfo<T1>(), typeof(S<,,,,>).GetField("F1")),
            ("f2", global::Serde.SerdeInfoProvider.GetInfo<T2>(), typeof(S<,,,,>).GetField("F2")),
            ("f3", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefProxy.Serialize<T3, T3>>(), typeof(S<,,,,>).GetField("F3")),
            ("f4", global::Serde.SerdeInfoProvider.GetInfo<T4>(), typeof(S<,,,,>).GetField("F4"))
        }
    );
}