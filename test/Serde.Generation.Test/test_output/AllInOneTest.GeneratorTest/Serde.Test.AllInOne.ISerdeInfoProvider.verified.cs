//HintName: Serde.Test.AllInOne.ISerdeInfoProvider.cs

#nullable enable
namespace Serde.Test;
partial record AllInOne : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "AllInOne",
        typeof(Serde.Test.AllInOne).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
("boolField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.BoolWrap>(), typeof(Serde.Test.AllInOne).GetField("BoolField")!),
("charField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.CharWrap>(), typeof(Serde.Test.AllInOne).GetField("CharField")!),
("byteField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.ByteWrap>(), typeof(Serde.Test.AllInOne).GetField("ByteField")!),
("uShortField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.UInt16Wrap>(), typeof(Serde.Test.AllInOne).GetField("UShortField")!),
("uIntField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.UInt32Wrap>(), typeof(Serde.Test.AllInOne).GetField("UIntField")!),
("uLongField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.UInt64Wrap>(), typeof(Serde.Test.AllInOne).GetField("ULongField")!),
("sByteField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.SByteWrap>(), typeof(Serde.Test.AllInOne).GetField("SByteField")!),
("shortField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int16Wrap>(), typeof(Serde.Test.AllInOne).GetField("ShortField")!),
("intField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Wrap>(), typeof(Serde.Test.AllInOne).GetField("IntField")!),
("longField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int64Wrap>(), typeof(Serde.Test.AllInOne).GetField("LongField")!),
("stringField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.AllInOne).GetField("StringField")!),
("escapedStringField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringWrap>(), typeof(Serde.Test.AllInOne).GetField("EscapedStringField")!),
("nullStringField", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefWrap.SerializeImpl<string,global::Serde.StringWrap>>(), typeof(Serde.Test.AllInOne).GetField("NullStringField")!),
("uIntArr", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<uint,global::Serde.UInt32Wrap>>(), typeof(Serde.Test.AllInOne).GetField("UIntArr")!),
("nestedArr", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayWrap.SerializeImpl<int[],Serde.ArrayWrap.SerializeImpl<int,global::Serde.Int32Wrap>>>(), typeof(Serde.Test.AllInOne).GetField("NestedArr")!),
("intImm", global::Serde.SerdeInfoProvider.GetInfo<Serde.ImmutableArrayWrap.SerializeImpl<int,global::Serde.Int32Wrap>>(), typeof(Serde.Test.AllInOne).GetField("IntImm")!),
("color", global::Serde.SerdeInfoProvider.GetInfo<Serde.Test.AllInOne.ColorEnumWrap>(), typeof(Serde.Test.AllInOne).GetField("Color")!)
    });
}