//HintName: Test.RecursiveWrap.ISerdeInfoProvider.g.cs

#nullable enable

namespace Test;

partial class RecursiveWrap
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Recursive",
    typeof(Recursive).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("next", global::Serde.SerdeInfoProvider.GetSerializeInfo<Recursive?, Test.RecursiveWrap>(), typeof(Recursive).GetProperty("Next"))
    }
    );
}
