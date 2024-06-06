//HintName: Serde.Test.AllInOneSerdeTypeInfo.cs
namespace Serde.Test;
internal static class AllInOneSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<AllInOne>(nameof(AllInOne), new (string, System.Reflection.MemberInfo)[] {
        ("boolField", typeof(AllInOne).GetField("BoolField")!),
("charField", typeof(AllInOne).GetField("CharField")!),
("byteField", typeof(AllInOne).GetField("ByteField")!),
("uShortField", typeof(AllInOne).GetField("UShortField")!),
("uIntField", typeof(AllInOne).GetField("UIntField")!),
("uLongField", typeof(AllInOne).GetField("ULongField")!),
("sByteField", typeof(AllInOne).GetField("SByteField")!),
("shortField", typeof(AllInOne).GetField("ShortField")!),
("intField", typeof(AllInOne).GetField("IntField")!),
("longField", typeof(AllInOne).GetField("LongField")!),
("stringField", typeof(AllInOne).GetField("StringField")!),
("nullStringField", typeof(AllInOne).GetField("NullStringField")!),
("uIntArr", typeof(AllInOne).GetField("UIntArr")!),
("nestedArr", typeof(AllInOne).GetField("NestedArr")!),
("intImm", typeof(AllInOne).GetField("IntImm")!),
("color", typeof(AllInOne).GetField("Color")!)
    });
}