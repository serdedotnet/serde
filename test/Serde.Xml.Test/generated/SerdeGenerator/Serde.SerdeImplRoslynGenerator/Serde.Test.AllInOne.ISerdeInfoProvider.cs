
#nullable enable

namespace Serde.Test;

partial record AllInOne
{
    private static global::Serde.ISerdeInfo s_serdeInfo = Serde.SerdeInfo.MakeCustom(
        "AllInOne",
    typeof(Serde.Test.AllInOne).GetCustomAttributesData(),
    new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo?)[] {
        ("boolField", global::Serde.SerdeInfoProvider.GetSerializeInfo<bool, global::Serde.BoolProxy>(), typeof(Serde.Test.AllInOne).GetField("BoolField")),
        ("charField", global::Serde.SerdeInfoProvider.GetSerializeInfo<char, global::Serde.CharProxy>(), typeof(Serde.Test.AllInOne).GetField("CharField")),
        ("byteField", global::Serde.SerdeInfoProvider.GetSerializeInfo<byte, global::Serde.U8Proxy>(), typeof(Serde.Test.AllInOne).GetField("ByteField")),
        ("uShortField", global::Serde.SerdeInfoProvider.GetSerializeInfo<ushort, global::Serde.U16Proxy>(), typeof(Serde.Test.AllInOne).GetField("UShortField")),
        ("uIntField", global::Serde.SerdeInfoProvider.GetSerializeInfo<uint, global::Serde.U32Proxy>(), typeof(Serde.Test.AllInOne).GetField("UIntField")),
        ("uLongField", global::Serde.SerdeInfoProvider.GetSerializeInfo<ulong, global::Serde.U64Proxy>(), typeof(Serde.Test.AllInOne).GetField("ULongField")),
        ("sByteField", global::Serde.SerdeInfoProvider.GetSerializeInfo<sbyte, global::Serde.I8Proxy>(), typeof(Serde.Test.AllInOne).GetField("SByteField")),
        ("shortField", global::Serde.SerdeInfoProvider.GetSerializeInfo<short, global::Serde.I16Proxy>(), typeof(Serde.Test.AllInOne).GetField("ShortField")),
        ("intField", global::Serde.SerdeInfoProvider.GetSerializeInfo<int, global::Serde.I32Proxy>(), typeof(Serde.Test.AllInOne).GetField("IntField")),
        ("longField", global::Serde.SerdeInfoProvider.GetSerializeInfo<long, global::Serde.I64Proxy>(), typeof(Serde.Test.AllInOne).GetField("LongField")),
        ("stringField", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.AllInOne).GetField("StringField")),
        ("escapedStringField", global::Serde.SerdeInfoProvider.GetSerializeInfo<string, global::Serde.StringProxy>(), typeof(Serde.Test.AllInOne).GetField("EscapedStringField")),
        ("nullStringField", global::Serde.SerdeInfoProvider.GetSerializeInfo<string?, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>(), typeof(Serde.Test.AllInOne).GetField("NullStringField")),
        ("uIntArr", global::Serde.SerdeInfoProvider.GetSerializeInfo<uint[], Serde.ArrayProxy.Ser<uint, global::Serde.U32Proxy>>(), typeof(Serde.Test.AllInOne).GetField("UIntArr")),
        ("nestedArr", global::Serde.SerdeInfoProvider.GetSerializeInfo<int[][], Serde.ArrayProxy.Ser<int[], Serde.ArrayProxy.Ser<int, global::Serde.I32Proxy>>>(), typeof(Serde.Test.AllInOne).GetField("NestedArr")),
        ("intImm", global::Serde.SerdeInfoProvider.GetSerializeInfo<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayProxy.Ser<int, global::Serde.I32Proxy>>(), typeof(Serde.Test.AllInOne).GetField("IntImm")),
        ("color", global::Serde.SerdeInfoProvider.GetSerializeInfo<Serde.Test.AllInOne.ColorEnum, Serde.Test.AllInOne.ColorEnumProxy>(), typeof(Serde.Test.AllInOne).GetField("Color"))
    }
    );
}