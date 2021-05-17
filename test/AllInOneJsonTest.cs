using Xunit;

namespace Serde.Test
{
    public class AllInOneJsonTest
    {
        [Fact]
        public void Test()
        {
            var allInOne = new AllInOne();
            var expected = @"{""BoolField"":true,""CharField"":""#"",""ByteField"":255,""UShortField"":65535,""UIntField"":4294967295,""ULongField"":18446744073709551615,"
             + @"""SByteField"":127,""ShortField"":32767,""IntField"":2147483647,""LongField"":9223372036854775807,""StringField"":""StringValue""}";
            var actual = JsonSerializer.WriteToString(allInOne);
            Assert.Equal(expected, actual);
        }
    }

    internal partial class AllInOne : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize<TSerializer, TSerializeType>(ref TSerializer serializer)
        {
            var type = serializer.SerializeType("AllInOne", 10);
            type.SerializeField("BoolField", new BoolWrap(BoolField));
            type.SerializeField("CharField", new CharWrap(CharField));
            type.SerializeField("ByteField", new ByteWrap(ByteField));
            type.SerializeField("UShortField", new UInt16Wrap(UShortField));
            type.SerializeField("UIntField", new UInt32Wrap(UIntField));
            type.SerializeField("ULongField", new UInt64Wrap(ULongField));
            type.SerializeField("SByteField", new SByteWrap(SByteField));
            type.SerializeField("ShortField", new Int16Wrap(ShortField));
            type.SerializeField("IntField", new Int32Wrap(IntField));
            type.SerializeField("LongField", new Int64Wrap(LongField));
            type.SerializeField("StringField", new StringWrap(StringField));
            type.End();
        }
    }
}
