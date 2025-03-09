﻿//HintName: OptsWrap.ISerdeInfoProvider.cs

#nullable enable
partial record struct OptsWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "BIND_OPTS",
        typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("cbStruct", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.I32Proxy>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("cbStruct")),
            ("dwTickCountDeadline", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.I32Proxy>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("dwTickCountDeadline")),
            ("grfFlags", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.I32Proxy>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("grfFlags")),
            ("grfMode", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.I32Proxy>(), typeof(System.Runtime.InteropServices.ComTypes.BIND_OPTS).GetField("grfMode"))
        }
    );
}