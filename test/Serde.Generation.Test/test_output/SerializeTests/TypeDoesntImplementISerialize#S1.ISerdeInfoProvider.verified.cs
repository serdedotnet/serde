﻿//HintName: S1.ISerdeInfoProvider.cs

#nullable enable
partial struct S1
{
    private static readonly global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "S1",
    typeof(S1).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {

    }
    );
}