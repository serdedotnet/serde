﻿//HintName: B.ISerdeInfoProvider.cs

#nullable enable
partial record B
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "B",
    typeof(B).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("y", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(B).GetProperty("Y")),
        ("x", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(B).GetProperty("X"))
    }
    );
}