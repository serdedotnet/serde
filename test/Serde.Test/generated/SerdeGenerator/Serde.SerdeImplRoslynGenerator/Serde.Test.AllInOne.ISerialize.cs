
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
            type.SerializeField("boolField", new BoolWrap(this.BoolField));
            type.SerializeField("charField", new CharWrap(this.CharField));
            type.SerializeField("byteField", new ByteWrap(this.ByteField));
            type.SerializeField("uShortField", new UInt16Wrap(this.UShortField));
            type.SerializeField("uIntField", new UInt32Wrap(this.UIntField));
            type.SerializeField("uLongField", new UInt64Wrap(this.ULongField));
            type.SerializeField("sByteField", new SByteWrap(this.SByteField));
            type.SerializeField("shortField", new Int16Wrap(this.ShortField));
            type.SerializeField("intField", new Int32Wrap(this.IntField));
            type.SerializeField("longField", new Int64Wrap(this.LongField));
            type.SerializeField("stringField", new StringWrap(this.StringField));
            type.SerializeFieldIfNotNull("nullStringField", new NullableRefWrap.SerializeImpl<string, StringWrap>(this.NullStringField), this.NullStringField);
            type.SerializeField("uIntArr", new ArrayWrap.SerializeImpl<uint, UInt32Wrap>(this.UIntArr));
            type.SerializeField("nestedArr", new ArrayWrap.SerializeImpl<int[], ArrayWrap.SerializeImpl<int, Int32Wrap>>(this.NestedArr));
            type.SerializeField("intImm", new ImmutableArrayWrap.SerializeImpl<int, Int32Wrap>(this.IntImm));
            type.SerializeField("color", new AllInOneColorEnumWrap(this.Color));
            type.End();
        }
    }
}