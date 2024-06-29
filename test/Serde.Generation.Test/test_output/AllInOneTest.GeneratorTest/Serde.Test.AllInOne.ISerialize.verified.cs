//HintName: Serde.Test.AllInOne.ISerialize.cs

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
            type.SerializeField<bool, BoolWrap>(_l_typeInfo, 0, this.BoolField);
            type.SerializeField<char, CharWrap>(_l_typeInfo, 1, this.CharField);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 2, this.ByteField);
            type.SerializeField<ushort, UInt16Wrap>(_l_typeInfo, 3, this.UShortField);
            type.SerializeField<uint, UInt32Wrap>(_l_typeInfo, 4, this.UIntField);
            type.SerializeField<ulong, UInt64Wrap>(_l_typeInfo, 5, this.ULongField);
            type.SerializeField<sbyte, SByteWrap>(_l_typeInfo, 6, this.SByteField);
            type.SerializeField<short, Int16Wrap>(_l_typeInfo, 7, this.ShortField);
            type.SerializeField<int, Int32Wrap>(_l_typeInfo, 8, this.IntField);
            type.SerializeField<long, Int64Wrap>(_l_typeInfo, 9, this.LongField);
            type.SerializeField<string, StringWrap>(_l_typeInfo, 10, this.StringField);
            type.SerializeFieldIfNotNull<string?, Serde.NullableRefWrap.SerializeImpl<string, StringWrap>>(_l_typeInfo, 11, this.NullStringField);
            type.SerializeField<uint[], Serde.ArrayWrap.SerializeImpl<uint, UInt32Wrap>>(_l_typeInfo, 12, this.UIntArr);
            type.SerializeField<int[][], Serde.ArrayWrap.SerializeImpl<int[], Serde.ArrayWrap.SerializeImpl<int, Int32Wrap>>>(_l_typeInfo, 13, this.NestedArr);
            type.SerializeField<System.Collections.Immutable.ImmutableArray<int>, Serde.ImmutableArrayWrap.SerializeImpl<int, Int32Wrap>>(_l_typeInfo, 14, this.IntImm);
            type.SerializeField<Serde.Test.AllInOne.ColorEnum, Serde.Test.AllInOne.ColorEnumWrap>(_l_typeInfo, 15, this.Color);
            type.End();
        }
    }
}