//HintName: C.ISerdeInfoProvider.g.cs

#nullable enable
partial class C
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "C",
    typeof(C).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("arr", global::Serde.SerdeInfoProvider.GetDeserializeInfo<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayProxy.De<int, global::Serde.I32Proxy>>(), typeof(C).GetField("Arr"))
    }
    );
}
