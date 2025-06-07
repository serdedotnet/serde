//HintName: CommandResponse.ISerdeInfoProvider.g.cs

#nullable enable
partial class CommandResponse<TResult, TProxy>
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "CommandResponse",
    typeof(CommandResponse<,>).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("status", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(CommandResponse<,>).GetProperty("Status")),
        ("message", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(CommandResponse<,>).GetProperty("Message")),
        ("arguments", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Generic.List<ArgumentInfo>?, Serde.NullableRefProxy.Ser<System.Collections.Generic.List<ArgumentInfo>, Serde.ListProxy.Ser<ArgumentInfo, ArgumentInfo>>>(), typeof(CommandResponse<,>).GetProperty("Arguments")),
        ("results", global::Serde.SerdeInfoProvider.GetSerializeInfo<TResult, TProxy>(), typeof(CommandResponse<,>).GetProperty("Results")),
        ("duration", global::Serde.SerdeInfoProvider.GetSerializeInfo<long, global::Serde.I64Proxy>(), typeof(CommandResponse<,>).GetProperty("Duration"))
    }
    );
}
