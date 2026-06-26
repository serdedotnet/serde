//HintName: CommandResponse.ISerdeInfoProvider.g.cs

#nullable enable
partial class CommandResponse<TResult, TProxy>
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "CommandResponse",
        typeof(CommandResponse<,>).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("status", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
            new("message", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>()),
            new("arguments", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Generic.List<ArgumentInfo>?, Serde.NullableRefProxy.Ser<System.Collections.Generic.List<ArgumentInfo>, Serde.ListProxy.Ser<ArgumentInfo, ArgumentInfo>>>()),
            new("results", global::Serde.SerdeInfoProvider.GetSerializeInfo<TResult, TProxy>())
            {
                MemberInfo = typeof(CommandResponse<,>).GetProperty("Results"),
            },
            new("duration", global::Serde.SerdeInfoProvider.GetSerializeInfo<long, global::Serde.I64Proxy>())
        }
    );
}
