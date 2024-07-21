//HintName: Test.ChannelWrap.ISerdeInfoProvider.cs

#nullable enable
namespace Test;
partial struct ChannelWrap : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "Channel",
        global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(),
        new (string, System.Reflection.MemberInfo)[] {
("a", typeof(Test.Channel).GetField("A")!),
("b", typeof(Test.Channel).GetField("B")!),
("c", typeof(Test.Channel).GetField("C")!)
    });
}