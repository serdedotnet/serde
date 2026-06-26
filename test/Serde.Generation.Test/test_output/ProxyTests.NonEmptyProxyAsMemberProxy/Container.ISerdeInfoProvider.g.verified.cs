//HintName: Container.ISerdeInfoProvider.g.cs

#nullable enable
partial class Container
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "Container",
        typeof(Container).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("point", global::Serde.SerdeInfoProvider.GetSerializeInfo<ForeignPoint, ForeignPointProxy>())
            {
                MemberInfo = typeof(Container).GetProperty("Point"),
            }
        }
    );
}
