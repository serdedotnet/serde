
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
            type.SerializeField("field1", new ByteWrap(this.Field1));
            type.SerializeField("field2", new ByteWrap(this.Field2));
            type.SerializeField("field3", new ByteWrap(this.Field3));
            type.SerializeField("field4", new ByteWrap(this.Field4));
            type.SerializeField("field5", new ByteWrap(this.Field5));
            type.SerializeField("field6", new ByteWrap(this.Field6));
            type.SerializeField("field7", new ByteWrap(this.Field7));
            type.SerializeField("field8", new ByteWrap(this.Field8));
            type.SerializeField("field9", new ByteWrap(this.Field9));
            type.SerializeField("field10", new ByteWrap(this.Field10));
            type.SerializeField("field11", new ByteWrap(this.Field11));
            type.SerializeField("field12", new ByteWrap(this.Field12));
            type.SerializeField("field13", new ByteWrap(this.Field13));
            type.SerializeField("field14", new ByteWrap(this.Field14));
            type.SerializeField("field15", new ByteWrap(this.Field15));
            type.SerializeField("field16", new ByteWrap(this.Field16));
            type.SerializeField("field17", new ByteWrap(this.Field17));
            type.SerializeField("field18", new ByteWrap(this.Field18));
            type.SerializeField("field19", new ByteWrap(this.Field19));
            type.SerializeField("field20", new ByteWrap(this.Field20));
            type.SerializeField("field21", new ByteWrap(this.Field21));
            type.SerializeField("field22", new ByteWrap(this.Field22));
            type.SerializeField("field23", new ByteWrap(this.Field23));
            type.SerializeField("field24", new ByteWrap(this.Field24));
            type.SerializeField("field25", new ByteWrap(this.Field25));
            type.SerializeField("field26", new ByteWrap(this.Field26));
            type.SerializeField("field27", new ByteWrap(this.Field27));
            type.SerializeField("field28", new ByteWrap(this.Field28));
            type.SerializeField("field29", new ByteWrap(this.Field29));
            type.SerializeField("field30", new ByteWrap(this.Field30));
            type.SerializeField("field31", new ByteWrap(this.Field31));
            type.SerializeField("field32", new ByteWrap(this.Field32));
            type.SerializeField("field33", new ByteWrap(this.Field33));
            type.SerializeField("field34", new ByteWrap(this.Field34));
            type.SerializeField("field35", new ByteWrap(this.Field35));
            type.SerializeField("field36", new ByteWrap(this.Field36));
            type.SerializeField("field37", new ByteWrap(this.Field37));
            type.SerializeField("field38", new ByteWrap(this.Field38));
            type.SerializeField("field39", new ByteWrap(this.Field39));
            type.SerializeField("field40", new ByteWrap(this.Field40));
            type.SerializeField("field41", new ByteWrap(this.Field41));
            type.SerializeField("field42", new ByteWrap(this.Field42));
            type.SerializeField("field43", new ByteWrap(this.Field43));
            type.SerializeField("field44", new ByteWrap(this.Field44));
            type.SerializeField("field45", new ByteWrap(this.Field45));
            type.SerializeField("field46", new ByteWrap(this.Field46));
            type.SerializeField("field47", new ByteWrap(this.Field47));
            type.SerializeField("field48", new ByteWrap(this.Field48));
            type.SerializeField("field49", new ByteWrap(this.Field49));
            type.SerializeField("field50", new ByteWrap(this.Field50));
            type.SerializeField("field51", new ByteWrap(this.Field51));
            type.SerializeField("field52", new ByteWrap(this.Field52));
            type.SerializeField("field53", new ByteWrap(this.Field53));
            type.SerializeField("field54", new ByteWrap(this.Field54));
            type.SerializeField("field55", new ByteWrap(this.Field55));
            type.SerializeField("field56", new ByteWrap(this.Field56));
            type.SerializeField("field57", new ByteWrap(this.Field57));
            type.SerializeField("field58", new ByteWrap(this.Field58));
            type.SerializeField("field59", new ByteWrap(this.Field59));
            type.SerializeField("field60", new ByteWrap(this.Field60));
            type.SerializeField("field61", new ByteWrap(this.Field61));
            type.SerializeField("field62", new ByteWrap(this.Field62));
            type.SerializeField("field63", new ByteWrap(this.Field63));
            type.SerializeField("field64", new ByteWrap(this.Field64));
            type.End();
        }
    }
}