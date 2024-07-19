//HintName: Test.ChannelWrap.ISerdeInfoProvider.cs

#nullable enable
namespace Test;
partial struct ChannelWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.SerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.Create(
        "Channel",
        Serde.SerdeInfo.TypeKind.Enum,
        new (string, global::Serde.SerdeInfo, System.Reflection.MemberInfo)[] {
("a", global::Serde.SerdeInfoProvider.GetInfo<ChannelWrap>(), typeof(Test.Channel).GetField("A")!),
("b", global::Serde.SerdeInfoProvider.GetInfo<ChannelWrap>(), typeof(Test.Channel).GetField("B")!),
("c", global::Serde.SerdeInfoProvider.GetInfo<ChannelWrap>(), typeof(Test.Channel).GetField("C")!)
    });
}