
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial struct MaxSizeType : Serde.ISerialize<Serde.Test.MaxSizeType>
    {
        void ISerialize<Serde.Test.MaxSizeType>.Serialize(Serde.Test.MaxSizeType value, ISerializer serializer)
        {
            var type = serializer.SerializeType("MaxSizeType", 64);
            type.SerializeField<byte, ByteWrap>("field1", value.Field1);
            type.SerializeField<byte, ByteWrap>("field2", value.Field2);
            type.SerializeField<byte, ByteWrap>("field3", value.Field3);
            type.SerializeField<byte, ByteWrap>("field4", value.Field4);
            type.SerializeField<byte, ByteWrap>("field5", value.Field5);
            type.SerializeField<byte, ByteWrap>("field6", value.Field6);
            type.SerializeField<byte, ByteWrap>("field7", value.Field7);
            type.SerializeField<byte, ByteWrap>("field8", value.Field8);
            type.SerializeField<byte, ByteWrap>("field9", value.Field9);
            type.SerializeField<byte, ByteWrap>("field10", value.Field10);
            type.SerializeField<byte, ByteWrap>("field11", value.Field11);
            type.SerializeField<byte, ByteWrap>("field12", value.Field12);
            type.SerializeField<byte, ByteWrap>("field13", value.Field13);
            type.SerializeField<byte, ByteWrap>("field14", value.Field14);
            type.SerializeField<byte, ByteWrap>("field15", value.Field15);
            type.SerializeField<byte, ByteWrap>("field16", value.Field16);
            type.SerializeField<byte, ByteWrap>("field17", value.Field17);
            type.SerializeField<byte, ByteWrap>("field18", value.Field18);
            type.SerializeField<byte, ByteWrap>("field19", value.Field19);
            type.SerializeField<byte, ByteWrap>("field20", value.Field20);
            type.SerializeField<byte, ByteWrap>("field21", value.Field21);
            type.SerializeField<byte, ByteWrap>("field22", value.Field22);
            type.SerializeField<byte, ByteWrap>("field23", value.Field23);
            type.SerializeField<byte, ByteWrap>("field24", value.Field24);
            type.SerializeField<byte, ByteWrap>("field25", value.Field25);
            type.SerializeField<byte, ByteWrap>("field26", value.Field26);
            type.SerializeField<byte, ByteWrap>("field27", value.Field27);
            type.SerializeField<byte, ByteWrap>("field28", value.Field28);
            type.SerializeField<byte, ByteWrap>("field29", value.Field29);
            type.SerializeField<byte, ByteWrap>("field30", value.Field30);
            type.SerializeField<byte, ByteWrap>("field31", value.Field31);
            type.SerializeField<byte, ByteWrap>("field32", value.Field32);
            type.SerializeField<byte, ByteWrap>("field33", value.Field33);
            type.SerializeField<byte, ByteWrap>("field34", value.Field34);
            type.SerializeField<byte, ByteWrap>("field35", value.Field35);
            type.SerializeField<byte, ByteWrap>("field36", value.Field36);
            type.SerializeField<byte, ByteWrap>("field37", value.Field37);
            type.SerializeField<byte, ByteWrap>("field38", value.Field38);
            type.SerializeField<byte, ByteWrap>("field39", value.Field39);
            type.SerializeField<byte, ByteWrap>("field40", value.Field40);
            type.SerializeField<byte, ByteWrap>("field41", value.Field41);
            type.SerializeField<byte, ByteWrap>("field42", value.Field42);
            type.SerializeField<byte, ByteWrap>("field43", value.Field43);
            type.SerializeField<byte, ByteWrap>("field44", value.Field44);
            type.SerializeField<byte, ByteWrap>("field45", value.Field45);
            type.SerializeField<byte, ByteWrap>("field46", value.Field46);
            type.SerializeField<byte, ByteWrap>("field47", value.Field47);
            type.SerializeField<byte, ByteWrap>("field48", value.Field48);
            type.SerializeField<byte, ByteWrap>("field49", value.Field49);
            type.SerializeField<byte, ByteWrap>("field50", value.Field50);
            type.SerializeField<byte, ByteWrap>("field51", value.Field51);
            type.SerializeField<byte, ByteWrap>("field52", value.Field52);
            type.SerializeField<byte, ByteWrap>("field53", value.Field53);
            type.SerializeField<byte, ByteWrap>("field54", value.Field54);
            type.SerializeField<byte, ByteWrap>("field55", value.Field55);
            type.SerializeField<byte, ByteWrap>("field56", value.Field56);
            type.SerializeField<byte, ByteWrap>("field57", value.Field57);
            type.SerializeField<byte, ByteWrap>("field58", value.Field58);
            type.SerializeField<byte, ByteWrap>("field59", value.Field59);
            type.SerializeField<byte, ByteWrap>("field60", value.Field60);
            type.SerializeField<byte, ByteWrap>("field61", value.Field61);
            type.SerializeField<byte, ByteWrap>("field62", value.Field62);
            type.SerializeField<byte, ByteWrap>("field63", value.Field63);
            type.SerializeField<byte, ByteWrap>("field64", value.Field64);
            type.End();
        }
    }
}