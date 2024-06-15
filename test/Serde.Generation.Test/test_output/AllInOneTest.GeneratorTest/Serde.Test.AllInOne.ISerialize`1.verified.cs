//HintName: Serde.Test.AllInOne.ISerialize`1.cs

#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial record AllInOne : Serde.ISerialize<Serde.Test.AllInOne>
    {
        void ISerialize<Serde.Test.AllInOne>.Serialize(Serde.Test.AllInOne value, ISerializer serializer)
        {
            var _l_typeInfo = AllInOneSerdeTypeInfo.TypeInfo;
            var type = serializer.SerializeType(_l_typeInfo);
            type.SerializeField<bool, BoolWrap>(_l_typeInfo, 0, value.BoolField);
            type.SerializeField<char, CharWrap>(_l_typeInfo, 1, value.CharField);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 2, value.ByteField);
            type.SerializeField<ushort, UInt16Wrap>(_l_typeInfo, 3, value.UShortField);
            type.SerializeField<uint, UInt32Wrap>(_l_typeInfo, 4, value.UIntField);
            type.SerializeField<ulong, UInt64Wrap>(_l_typeInfo, 5, value.ULongField);
            type.SerializeField<sbyte, SByteWrap>(_l_typeInfo, 6, value.SByteField);
            type.SerializeField<short, Int16Wrap>(_l_typeInfo, 7, value.ShortField);
            type.SerializeField<int, Int32Wrap>(_l_typeInfo, 8, value.IntField);
            type.SerializeField<long, Int64Wrap>(_l_typeInfo, 9, value.LongField);
            type.SerializeField<string, StringWrap>(_l_typeInfo, 10, value.StringField);
            type.SerializeFieldIfNotNull<string?, Serde.NullableRefWrap.SerializeImpl<string, StringWrap>>(_l_typeInfo, 11, value.NullStringField);
            type.SerializeField<uint[], Serde.ArrayWrap.SerializeImpl<uint, UInt32Wrap>>(_l_typeInfo, 12, value.UIntArr);
            type.SerializeField<int[][], Serde.ArrayWrap.SerializeImpl<int[], Serde.ArrayWrap.SerializeImpl<int, Int32Wrap>>>(_l_typeInfo, 13, value.NestedArr);
            type.SerializeField<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayWrap.SerializeImpl<int, Int32Wrap>>(_l_typeInfo, 14, value.IntImm);
            type.SerializeField<Serde.Test.AllInOne.ColorEnum, Serde.Test.AllInOne.ColorEnumWrap>(_l_typeInfo, 15, value.Color);
            type.End();
        }
    }
}