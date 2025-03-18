
#nullable enable

namespace Serde.Test;

partial class XmlTests
{
    partial class NestedArrays : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "NestedArrays",
            typeof(Serde.Test.XmlTests.NestedArrays).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
                ("A", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayProxy.Ser<int[][], Serde.ArrayProxy.Ser<int[], Serde.ArrayProxy.Ser<int, global::Serde.I32Proxy>>>>(), typeof(Serde.Test.XmlTests.NestedArrays).GetField("A"))
            }
        );
    }
}