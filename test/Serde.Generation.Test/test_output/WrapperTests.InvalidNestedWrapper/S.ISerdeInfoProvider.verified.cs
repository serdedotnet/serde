﻿//HintName: S.ISerdeInfoProvider.cs

#nullable enable
partial struct S : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "S",
        typeof(S).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("sections", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<int,global::Serde.Int32Wrap>>(), typeof(S).GetField("Sections")!)
    });
}