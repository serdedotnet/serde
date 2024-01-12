
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial record AllInOne : Serde.ISerialize<Serde.Test.AllInOne>
    {
        void ISerialize<Serde.Test.AllInOne>.Serialize(Serde.Test.AllInOne value, ISerializer serializer)
        {
            var type = serializer.SerializeType("AllInOne", 16);
            type.SerializeField<bool, BoolWrap>("boolField", value.BoolField);
            type.SerializeField<char, CharWrap>("charField", value.CharField);
            type.SerializeField<byte, ByteWrap>("byteField", value.ByteField);
            type.SerializeField<ushort, UInt16Wrap>("uShortField", value.UShortField);
            type.SerializeField<uint, UInt32Wrap>("uIntField", value.UIntField);
            type.SerializeField<ulong, UInt64Wrap>("uLongField", value.ULongField);
            type.SerializeField<sbyte, SByteWrap>("sByteField", value.SByteField);
            type.SerializeField<short, Int16Wrap>("shortField", value.ShortField);
            type.SerializeField<int, Int32Wrap>("intField", value.IntField);
            type.SerializeField<long, Int64Wrap>("longField", value.LongField);
            type.SerializeField<string, StringWrap>("stringField", value.StringField);
            type.SerializeFieldIfNotNull<string?, Serde.NullableRefWrap.SerializeImpl<string, StringWrap>>("nullStringField", value.NullStringField);
            type.SerializeField<uint[], Serde.ArrayWrap.SerializeImpl<uint, UInt32Wrap>>("uIntArr", value.UIntArr);
            type.SerializeField<int[][], Serde.ArrayWrap.SerializeImpl<int[], Serde.ArrayWrap.SerializeImpl<int, Int32Wrap>>>("nestedArr", value.NestedArr);
            type.SerializeField<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayWrap.SerializeImpl<int, Int32Wrap>>("intImm", value.IntImm);
            type.SerializeField<Serde.Test.AllInOne.ColorEnum, Serde.Test.AllInOne.ColorEnumWrap>("color", value.Color);
            type.End();
        }
    }
}