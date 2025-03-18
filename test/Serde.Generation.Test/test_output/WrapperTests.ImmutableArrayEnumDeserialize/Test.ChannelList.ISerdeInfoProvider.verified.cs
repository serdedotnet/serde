﻿//HintName: Test.ChannelList.ISerdeInfoProvider.cs

#nullable enable

namespace Test;

partial record struct ChannelList : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "ChannelList",
        typeof(Test.ChannelList).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("channels", global::Serde.SerdeInfoProvider.GetInfo<Serde.ImmutableArrayProxy.De<Test.Channel, Test.ChannelProxy>>(), typeof(Test.ChannelList).GetProperty("Channels"))
        }
    );
}