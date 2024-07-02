
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial struct MaxSizeType : Serde.ISerialize<Serde.Test.MaxSizeType>
    {
        void ISerialize<Serde.Test.MaxSizeType>.Serialize(Serde.Test.MaxSizeType value, ISerializer serializer)
        {
            var _l_typeInfo = MaxSizeTypeSerdeTypeInfo.TypeInfo;
            var type = serializer.SerializeType(_l_typeInfo);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 0, value.Field1);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 1, value.Field2);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 2, value.Field3);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 3, value.Field4);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 4, value.Field5);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 5, value.Field6);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 6, value.Field7);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 7, value.Field8);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 8, value.Field9);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 9, value.Field10);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 10, value.Field11);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 11, value.Field12);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 12, value.Field13);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 13, value.Field14);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 14, value.Field15);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 15, value.Field16);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 16, value.Field17);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 17, value.Field18);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 18, value.Field19);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 19, value.Field20);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 20, value.Field21);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 21, value.Field22);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 22, value.Field23);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 23, value.Field24);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 24, value.Field25);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 25, value.Field26);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 26, value.Field27);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 27, value.Field28);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 28, value.Field29);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 29, value.Field30);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 30, value.Field31);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 31, value.Field32);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 32, value.Field33);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 33, value.Field34);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 34, value.Field35);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 35, value.Field36);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 36, value.Field37);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 37, value.Field38);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 38, value.Field39);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 39, value.Field40);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 40, value.Field41);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 41, value.Field42);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 42, value.Field43);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 43, value.Field44);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 44, value.Field45);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 45, value.Field46);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 46, value.Field47);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 47, value.Field48);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 48, value.Field49);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 49, value.Field50);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 50, value.Field51);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 51, value.Field52);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 52, value.Field53);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 53, value.Field54);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 54, value.Field55);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 55, value.Field56);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 56, value.Field57);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 57, value.Field58);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 58, value.Field59);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 59, value.Field60);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 60, value.Field61);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 61, value.Field62);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 62, value.Field63);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 63, value.Field64);
            type.End();
        }
    }
}