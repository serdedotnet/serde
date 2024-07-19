//HintName: Test.ChannelList.ISerdeInfoProvider.cs

#nullable enable
namespace Test;
partial record struct ChannelList : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "ChannelList",
        Serde.SerdeInfo.TypeKind.CustomType,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("channels", global::Serde.SerdeInfoProvider.GetInfo<Serde.ImmutableArrayWrap.DeserializeImpl<Test.Channel,Test.ChannelWrap>>(), typeof(Test.ChannelList).GetProperty("Channels")!)
    });
}