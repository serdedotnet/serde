
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial struct MaxSizeType : Serde.ISerialize<Serde.Test.MaxSizeType>
    {
        void ISerialize<Serde.Test.MaxSizeType>.Serialize(Serde.Test.MaxSizeType value, ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<MaxSizeType>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 0, value.Field1);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 1, value.Field2);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 2, value.Field3);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 3, value.Field4);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 4, value.Field5);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 5, value.Field6);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 6, value.Field7);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 7, value.Field8);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 8, value.Field9);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 9, value.Field10);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 10, value.Field11);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 11, value.Field12);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 12, value.Field13);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 13, value.Field14);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 14, value.Field15);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 15, value.Field16);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 16, value.Field17);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 17, value.Field18);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 18, value.Field19);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 19, value.Field20);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 20, value.Field21);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 21, value.Field22);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 22, value.Field23);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 23, value.Field24);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 24, value.Field25);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 25, value.Field26);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 26, value.Field27);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 27, value.Field28);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 28, value.Field29);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 29, value.Field30);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 30, value.Field31);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 31, value.Field32);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 32, value.Field33);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 33, value.Field34);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 34, value.Field35);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 35, value.Field36);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 36, value.Field37);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 37, value.Field38);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 38, value.Field39);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 39, value.Field40);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 40, value.Field41);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 41, value.Field42);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 42, value.Field43);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 43, value.Field44);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 44, value.Field45);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 45, value.Field46);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 46, value.Field47);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 47, value.Field48);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 48, value.Field49);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 49, value.Field50);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 50, value.Field51);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 51, value.Field52);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 52, value.Field53);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 53, value.Field54);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 54, value.Field55);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 55, value.Field56);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 56, value.Field57);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 57, value.Field58);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 58, value.Field59);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 59, value.Field60);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 60, value.Field61);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 61, value.Field62);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 62, value.Field63);
            type.SerializeField<byte, global::Serde.ByteWrap>(_l_serdeInfo, 63, value.Field64);
            type.End();
        }
    }
}