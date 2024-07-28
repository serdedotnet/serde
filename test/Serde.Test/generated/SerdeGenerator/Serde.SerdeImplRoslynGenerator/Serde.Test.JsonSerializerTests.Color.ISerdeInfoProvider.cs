
#nullable enable
namespace Serde.Test;
partial class JsonSerializerTests
{
    partial struct Color : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "Color",
        typeof(Serde.Test.JsonSerializerTests.Color).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("red", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Test.JsonSerializerTests.Color).GetField("Red")!),
("green", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Test.JsonSerializerTests.Color).GetField("Green")!),
("blue", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Test.JsonSerializerTests.Color).GetField("Blue")!)
    });
}
}