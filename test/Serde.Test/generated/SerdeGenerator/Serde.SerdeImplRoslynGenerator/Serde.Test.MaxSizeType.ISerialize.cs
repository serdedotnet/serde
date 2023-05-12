
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial struct MaxSizeType : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType("MaxSizeType", 64);
            type.SerializeField("field1"u8, new ByteWrap(this.Field1));
            type.SerializeField("field2"u8, new ByteWrap(this.Field2));
            type.SerializeField("field3"u8, new ByteWrap(this.Field3));
            type.SerializeField("field4"u8, new ByteWrap(this.Field4));
            type.SerializeField("field5"u8, new ByteWrap(this.Field5));
            type.SerializeField("field6"u8, new ByteWrap(this.Field6));
            type.SerializeField("field7"u8, new ByteWrap(this.Field7));
            type.SerializeField("field8"u8, new ByteWrap(this.Field8));
            type.SerializeField("field9"u8, new ByteWrap(this.Field9));
            type.SerializeField("field10"u8, new ByteWrap(this.Field10));
            type.SerializeField("field11"u8, new ByteWrap(this.Field11));
            type.SerializeField("field12"u8, new ByteWrap(this.Field12));
            type.SerializeField("field13"u8, new ByteWrap(this.Field13));
            type.SerializeField("field14"u8, new ByteWrap(this.Field14));
            type.SerializeField("field15"u8, new ByteWrap(this.Field15));
            type.SerializeField("field16"u8, new ByteWrap(this.Field16));
            type.SerializeField("field17"u8, new ByteWrap(this.Field17));
            type.SerializeField("field18"u8, new ByteWrap(this.Field18));
            type.SerializeField("field19"u8, new ByteWrap(this.Field19));
            type.SerializeField("field20"u8, new ByteWrap(this.Field20));
            type.SerializeField("field21"u8, new ByteWrap(this.Field21));
            type.SerializeField("field22"u8, new ByteWrap(this.Field22));
            type.SerializeField("field23"u8, new ByteWrap(this.Field23));
            type.SerializeField("field24"u8, new ByteWrap(this.Field24));
            type.SerializeField("field25"u8, new ByteWrap(this.Field25));
            type.SerializeField("field26"u8, new ByteWrap(this.Field26));
            type.SerializeField("field27"u8, new ByteWrap(this.Field27));
            type.SerializeField("field28"u8, new ByteWrap(this.Field28));
            type.SerializeField("field29"u8, new ByteWrap(this.Field29));
            type.SerializeField("field30"u8, new ByteWrap(this.Field30));
            type.SerializeField("field31"u8, new ByteWrap(this.Field31));
            type.SerializeField("field32"u8, new ByteWrap(this.Field32));
            type.SerializeField("field33"u8, new ByteWrap(this.Field33));
            type.SerializeField("field34"u8, new ByteWrap(this.Field34));
            type.SerializeField("field35"u8, new ByteWrap(this.Field35));
            type.SerializeField("field36"u8, new ByteWrap(this.Field36));
            type.SerializeField("field37"u8, new ByteWrap(this.Field37));
            type.SerializeField("field38"u8, new ByteWrap(this.Field38));
            type.SerializeField("field39"u8, new ByteWrap(this.Field39));
            type.SerializeField("field40"u8, new ByteWrap(this.Field40));
            type.SerializeField("field41"u8, new ByteWrap(this.Field41));
            type.SerializeField("field42"u8, new ByteWrap(this.Field42));
            type.SerializeField("field43"u8, new ByteWrap(this.Field43));
            type.SerializeField("field44"u8, new ByteWrap(this.Field44));
            type.SerializeField("field45"u8, new ByteWrap(this.Field45));
            type.SerializeField("field46"u8, new ByteWrap(this.Field46));
            type.SerializeField("field47"u8, new ByteWrap(this.Field47));
            type.SerializeField("field48"u8, new ByteWrap(this.Field48));
            type.SerializeField("field49"u8, new ByteWrap(this.Field49));
            type.SerializeField("field50"u8, new ByteWrap(this.Field50));
            type.SerializeField("field51"u8, new ByteWrap(this.Field51));
            type.SerializeField("field52"u8, new ByteWrap(this.Field52));
            type.SerializeField("field53"u8, new ByteWrap(this.Field53));
            type.SerializeField("field54"u8, new ByteWrap(this.Field54));
            type.SerializeField("field55"u8, new ByteWrap(this.Field55));
            type.SerializeField("field56"u8, new ByteWrap(this.Field56));
            type.SerializeField("field57"u8, new ByteWrap(this.Field57));
            type.SerializeField("field58"u8, new ByteWrap(this.Field58));
            type.SerializeField("field59"u8, new ByteWrap(this.Field59));
            type.SerializeField("field60"u8, new ByteWrap(this.Field60));
            type.SerializeField("field61"u8, new ByteWrap(this.Field61));
            type.SerializeField("field62"u8, new ByteWrap(this.Field62));
            type.SerializeField("field63"u8, new ByteWrap(this.Field63));
            type.SerializeField("field64"u8, new ByteWrap(this.Field64));
            type.End();
        }
    }
}