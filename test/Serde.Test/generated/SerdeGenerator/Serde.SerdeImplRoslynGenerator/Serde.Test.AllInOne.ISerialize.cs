
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial record AllInOne : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType("AllInOne", 16);
            type.SerializeField("boolField"u8, new BoolWrap(this.BoolField));
            type.SerializeField("charField"u8, new CharWrap(this.CharField));
            type.SerializeField("byteField"u8, new ByteWrap(this.ByteField));
            type.SerializeField("uShortField"u8, new UInt16Wrap(this.UShortField));
            type.SerializeField("uIntField"u8, new UInt32Wrap(this.UIntField));
            type.SerializeField("uLongField"u8, new UInt64Wrap(this.ULongField));
            type.SerializeField("sByteField"u8, new SByteWrap(this.SByteField));
            type.SerializeField("shortField"u8, new Int16Wrap(this.ShortField));
            type.SerializeField("intField"u8, new Int32Wrap(this.IntField));
            type.SerializeField("longField"u8, new Int64Wrap(this.LongField));
            type.SerializeField("stringField"u8, new StringWrap(this.StringField));
            type.SerializeFieldIfNotNull("nullStringField"u8, new NullableRefWrap.SerializeImpl<string, StringWrap>(this.NullStringField), this.NullStringField);
            type.SerializeField("uIntArr"u8, new ArrayWrap.SerializeImpl<uint, UInt32Wrap>(this.UIntArr));
            type.SerializeField("nestedArr"u8, new ArrayWrap.SerializeImpl<int[], ArrayWrap.SerializeImpl<int, Int32Wrap>>(this.NestedArr));
            type.SerializeField("intImm"u8, new ImmutableArrayWrap.SerializeImpl<int, Int32Wrap>(this.IntImm));
            type.SerializeField("color"u8, new Serde.Test.AllInOne.ColorEnumWrap(this.Color));
            type.End();
        }
    }
}