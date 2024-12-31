
#nullable enable
namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithIntList : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "ClassWithIntList",
            typeof(Serde.Json.Test.InvalidJsonTests.ClassWithIntList).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
                ("obj", global::Serde.SerdeInfoProvider.GetInfo<Serde.ListProxy.Deserialize<int,global::Serde.Int32Proxy>>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithIntList).GetProperty("Obj")!)
            }
        );
    }
}