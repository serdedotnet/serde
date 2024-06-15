namespace Serde.Test;
internal static class AllInOneSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("boolField", typeof(Serde.Test.AllInOne).GetField("BoolField")!),
("charField", typeof(Serde.Test.AllInOne).GetField("CharField")!),
("byteField", typeof(Serde.Test.AllInOne).GetField("ByteField")!),
("uShortField", typeof(Serde.Test.AllInOne).GetField("UShortField")!),
("uIntField", typeof(Serde.Test.AllInOne).GetField("UIntField")!),
("uLongField", typeof(Serde.Test.AllInOne).GetField("ULongField")!),
("sByteField", typeof(Serde.Test.AllInOne).GetField("SByteField")!),
("shortField", typeof(Serde.Test.AllInOne).GetField("ShortField")!),
("intField", typeof(Serde.Test.AllInOne).GetField("IntField")!),
("longField", typeof(Serde.Test.AllInOne).GetField("LongField")!),
("stringField", typeof(Serde.Test.AllInOne).GetField("StringField")!),
("nullStringField", typeof(Serde.Test.AllInOne).GetField("NullStringField")!),
("uIntArr", typeof(Serde.Test.AllInOne).GetField("UIntArr")!),
("nestedArr", typeof(Serde.Test.AllInOne).GetField("NestedArr")!),
("intImm", typeof(Serde.Test.AllInOne).GetField("IntImm")!),
("color", typeof(Serde.Test.AllInOne).GetField("Color")!)
    });
}