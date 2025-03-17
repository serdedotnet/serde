﻿//HintName: SetToNull.ISerdeInfoProvider.cs

#nullable enable
partial record struct SetToNull : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "SetToNull",
        typeof(SetToNull).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("present", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringProxy>(), typeof(SetToNull).GetProperty("Present")),
            ("missing", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(), typeof(SetToNull).GetProperty("Missing")),
            ("throwMissing", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefProxy.De<string, global::Serde.StringProxy>>(), typeof(SetToNull).GetProperty("ThrowMissing"))
        }
    );
}