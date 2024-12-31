//HintName: Serde.Test.AllInOne.ISerdeInfoProvider.cs

#nullable enable
namespace Serde.Test;

partial record AllInOne : Serde.ISerdeInfoProvider
{
    static global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
        "AllInOne",
        typeof(Serde.Test.AllInOne).GetCustomAttributesData(),
        new (string, global::Serde.ISerdeInfo, System.Reflection.MemberInfo)[] {
            ("boolField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.BoolProxy>(), typeof(Serde.Test.AllInOne).GetField("BoolField")!),
            ("charField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.CharProxy>(), typeof(Serde.Test.AllInOne).GetField("CharField")!),
            ("byteField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.ByteProxy>(), typeof(Serde.Test.AllInOne).GetField("ByteField")!),
            ("uShortField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.UInt16Proxy>(), typeof(Serde.Test.AllInOne).GetField("UShortField")!),
            ("uIntField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.UInt32Proxy>(), typeof(Serde.Test.AllInOne).GetField("UIntField")!),
            ("uLongField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.UInt64Proxy>(), typeof(Serde.Test.AllInOne).GetField("ULongField")!),
            ("sByteField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.SByteProxy>(), typeof(Serde.Test.AllInOne).GetField("SByteField")!),
            ("shortField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int16Proxy>(), typeof(Serde.Test.AllInOne).GetField("ShortField")!),
            ("intField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int32Proxy>(), typeof(Serde.Test.AllInOne).GetField("IntField")!),
            ("longField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.Int64Proxy>(), typeof(Serde.Test.AllInOne).GetField("LongField")!),
            ("stringField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringProxy>(), typeof(Serde.Test.AllInOne).GetField("StringField")!),
            ("escapedStringField", global::Serde.SerdeInfoProvider.GetInfo<global::Serde.StringProxy>(), typeof(Serde.Test.AllInOne).GetField("EscapedStringField")!),
            ("nullStringField", global::Serde.SerdeInfoProvider.GetInfo<Serde.NullableRefProxy.Serialize<string,global::Serde.StringProxy>>(), typeof(Serde.Test.AllInOne).GetField("NullStringField")!),
            ("uIntArr", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayProxy.Serialize<uint,global::Serde.UInt32Proxy>>(), typeof(Serde.Test.AllInOne).GetField("UIntArr")!),
            ("nestedArr", global::Serde.SerdeInfoProvider.GetInfo<Serde.ArrayProxy.Serialize<int[],Serde.ArrayProxy.Serialize<int,global::Serde.Int32Proxy>>>(), typeof(Serde.Test.AllInOne).GetField("NestedArr")!),
            ("intImm", global::Serde.SerdeInfoProvider.GetInfo<Serde.ImmutableArrayProxy.Serialize<int,global::Serde.Int32Proxy>>(), typeof(Serde.Test.AllInOne).GetField("IntImm")!),
            ("color", global::Serde.SerdeInfoProvider.GetInfo<Serde.Test.AllInOne.ColorEnumProxy>(), typeof(Serde.Test.AllInOne).GetField("Color")!)
        }
    );
}