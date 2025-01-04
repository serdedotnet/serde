
#nullable enable

namespace Serde.Test;

partial class XmlTests
{
    partial record StructWithIntField : Serde.ISerdeInfoProvider
    {
        static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "StructWithIntField",
            typeof(Serde.Test.XmlTests.StructWithIntField).GetCustomAttributesData(),
            new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
                ("X", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Proxy>(), typeof(Serde.Test.XmlTests.StructWithIntField).GetProperty("X")!)
            }
        );
    }
}