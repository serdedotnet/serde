//HintName: Container.ISerdeInfoProvider.g.cs

#nullable enable
partial class Container
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Container",
    typeof(Container).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("point", global::Serde.SerdeInfoProvider.GetSerializeInfo<ForeignPoint, ForeignPointProxy>(), typeof(Container).GetProperty("Point"))
    }
    );
}
