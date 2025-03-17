﻿//HintName: Container.ISerdeInfoProvider.cs

#nullable enable
partial record Container : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "Container",
        typeof(Container).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("sdkDir", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableProxy.De<Original, Proxy>>(), typeof(Container).GetProperty("SdkDir"))
        }
    );
}