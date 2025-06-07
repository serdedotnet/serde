//HintName: Test.ChannelProxy.ISerdeInfoProvider.g.cs

#nullable enable

namespace Test;

partial class ChannelProxy : global::Serde.ISerdeInfoProvider
{
    global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeEnum(
        "Channel",
    typeof(Test.Channel).GetCustomAttributesData(),
    global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(),
    new (string, System.Reflection.MemberInfo?)[] {
        ("a", typeof(Test.Channel).GetField("A")),
        ("b", typeof(Test.Channel).GetField("B")),
        ("c", typeof(Test.Channel).GetField("C"))
    }
    );
}
