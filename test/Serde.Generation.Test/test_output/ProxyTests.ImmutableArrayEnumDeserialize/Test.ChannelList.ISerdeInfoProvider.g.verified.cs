//HintName: Test.ChannelList.ISerdeInfoProvider.g.cs

#nullable enable

namespace Test;

partial record struct ChannelList
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "ChannelList",
    typeof(Test.ChannelList).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("channels", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Immutable.ImmutableArray<Test.Channel>, Serde.ImmutableArrayProxy.De<Test.Channel, Test.ChannelProxy>>(), typeof(Test.ChannelList).GetProperty("Channels"))
    }
    );
}
