//HintName: Test.Parent.ISerdeInfoProvider.cs

#nullable enable

namespace Test;

partial record Parent
{
    private static readonly global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Parent",
    typeof(Test.Parent).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("r", global::Serde.SerdeInfoProvider.GetSerializeInfo<Recursive, Test.RecursiveWrap>(), typeof(Test.Parent).GetProperty("R"))
    }
    );
}