//HintName: A.B.C.D.ISerdeInfoProvider.g.cs

#nullable enable
partial class A
{
    partial class B
    {
        partial class C
        {
            partial class D
            {
                private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
                    "D",
                typeof(A.B.C.D).GetCustomAttributesData(),
                new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
                    ("field", global::Serde.SerdeInfoProvider.GetDeserializeInfo<int, global::Serde.I32Proxy>(), typeof(A.B.C.D).GetField("Field"))
                }
                );
            }
        }
    }
}
