
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
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 0, this.Field1);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 1, this.Field2);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 2, this.Field3);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 3, this.Field4);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 4, this.Field5);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 5, this.Field6);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 6, this.Field7);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 7, this.Field8);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 8, this.Field9);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 9, this.Field10);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 10, this.Field11);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 11, this.Field12);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 12, this.Field13);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 13, this.Field14);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 14, this.Field15);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 15, this.Field16);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 16, this.Field17);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 17, this.Field18);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 18, this.Field19);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 19, this.Field20);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 20, this.Field21);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 21, this.Field22);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 22, this.Field23);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 23, this.Field24);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 24, this.Field25);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 25, this.Field26);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 26, this.Field27);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 27, this.Field28);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 28, this.Field29);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 29, this.Field30);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 30, this.Field31);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 31, this.Field32);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 32, this.Field33);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 33, this.Field34);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 34, this.Field35);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 35, this.Field36);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 36, this.Field37);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 37, this.Field38);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 38, this.Field39);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 39, this.Field40);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 40, this.Field41);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 41, this.Field42);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 42, this.Field43);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 43, this.Field44);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 44, this.Field45);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 45, this.Field46);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 46, this.Field47);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 47, this.Field48);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 48, this.Field49);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 49, this.Field50);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 50, this.Field51);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 51, this.Field52);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 52, this.Field53);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 53, this.Field54);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 54, this.Field55);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 55, this.Field56);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 56, this.Field57);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 57, this.Field58);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 58, this.Field59);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 59, this.Field60);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 60, this.Field61);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 61, this.Field62);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 62, this.Field63);
            type.SerializeField<byte, ByteWrap>(_l_typeInfo, 63, this.Field64);
            type.End();
        }
    }
}