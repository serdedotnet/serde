//HintName: C.ISerdeInfoProvider.cs

#nullable enable
partial class C<T>
{
    private static readonly global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
    typeof(C<>).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {

    }
    );
}