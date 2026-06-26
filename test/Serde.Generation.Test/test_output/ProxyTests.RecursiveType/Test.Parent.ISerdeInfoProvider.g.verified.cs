//HintName: Test.Parent.ISerdeInfoProvider.g.cs

#nullable enable

namespace Test;

partial record Parent
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Parent",
        typeof(Test.Parent).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("r", global::Serde.SerdeInfoProvider.GetSerializeInfo<Recursive, Test.RecursiveWrap>())
            {
                MemberInfo = typeof(Test.Parent).GetProperty("R"),
            }
        }
    );
}
