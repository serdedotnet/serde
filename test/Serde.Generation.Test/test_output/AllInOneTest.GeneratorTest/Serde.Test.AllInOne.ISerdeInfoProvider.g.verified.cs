//HintName: Serde.Test.AllInOne.ISerdeInfoProvider.g.cs

#nullable enable

namespace Serde.Test;

partial record AllInOne
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "AllInOne",
        typeof(Serde.Test.AllInOne).GetCustomAttributesData(),
        new global::Serde.SerdeInfo.FieldInfo[] {
            new("boolField", global::Serde.SerdeInfoProvider.GetSerializeInfo<bool, global::Serde.BoolProxy>()),
            new("charField", global::Serde.SerdeInfoProvider.GetSerializeInfo<char, global::Serde.CharProxy>()),
            new("byteField", global::Serde.SerdeInfoProvider.GetSerializeInfo<byte, global::Serde.U8Proxy>()),
            new("uShortField", global::Serde.SerdeInfoProvider.GetSerializeInfo<ushort, global::Serde.U16Proxy>()),
            new("uIntField", global::Serde.SerdeInfoProvider.GetSerializeInfo<uint, global::Serde.U32Proxy>()),
            new("uLongField", global::Serde.SerdeInfoProvider.GetSerializeInfo<ulong, global::Serde.U64Proxy>()),
            new("uInt128Field", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.UInt128, global::Serde.U128Proxy>()),
            new("sByteField", global::Serde.SerdeInfoProvider.GetSerializeInfo<sbyte, global::Serde.I8Proxy>()),
            new("shortField", global::Serde.SerdeInfoProvider.GetSerializeInfo<short, global::Serde.I16Proxy>()),
            new("intField", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>()),
            new("longField", global::Serde.SerdeInfoProvider.GetSerializeInfo<long, global::Serde.I64Proxy>()),
            new("int128Field", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Int128, global::Serde.I128Proxy>()),
            new("stringField", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>()),
            new("dateTimeOffsetField", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.DateTimeOffset, global::Serde.DateTimeOffsetProxy>()),
            new("dateTimeField", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.DateTime, global::Serde.DateTimeProxy>()),
            new("dateOnlyField", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.DateOnly, global::Serde.DateOnlyProxy>()),
            new("timeOnlyField", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.TimeOnly, global::Serde.TimeOnlyProxy>()),
            new("guidField", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Guid, global::Serde.GuidProxy>()),
            new("escapedStringField", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>()),
            new("nullStringField", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>()),
            new("uIntArr", global::Serde.SerdeInfoProvider.GetSerializeInfo<uint[], Serde.ArrayProxy.Ser<uint, global::Serde.U32Proxy>>()),
            new("nestedArr", global::Serde.SerdeInfoProvider.GetSerializeInfo<int[][], Serde.ArrayProxy.Ser<int[], Serde.ArrayProxy.Ser<int, global::Serde.I32Proxy>>>()),
            new("byteArr", global::Serde.SerdeInfoProvider.GetSerializeInfo<byte[], global::Serde.ByteArrayProxy>()),
            new("intImm", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayProxy.Ser<int, global::Serde.I32Proxy>>()),
            new("color", global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.AllInOne.ColorEnum, Serde.Test.AllInOne.ColorEnumProxy>())
        }
    );
}
