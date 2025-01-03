
#nullable enable
namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithIntArray : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "ClassWithIntArray",
            typeof(Serde.Json.Test.InvalidJsonTests.ClassWithIntArray).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
                ("obj", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayProxy.Deserialize<int,global::Serde.Int32Proxy>>(), typeof(Serde.Json.Test.InvalidJsonTests.ClassWithIntArray).GetProperty("Obj")!)
            }
        );
    }
}