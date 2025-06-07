
#nullable enable

namespace Serde.Test;

partial class GenericWrapperTests
{
    partial record struct CustomArrayWrapExplicitOnType
    {
        private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
            "CustomArrayWrapExplicitOnType",
        typeof(Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
            ("a", global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.GenericWrapperTests.CustomImArray2<int>, Serde.Test.GenericWrapperTests.CustomImArray2Proxy.Ser<int, global::Serde.I32Proxy>>(), typeof(Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType).GetField("A"))
        }
        );
    }
}
