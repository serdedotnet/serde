using Xunit;

namespace Serde.Test
{
    public class AllInOneJsonTest
    {

        [Fact]
        public void Rgb()
        {
            var allInOne = new AllInOne();
            var expected = "";
            Assert.Equal(expected, JsonSerializer.SerializeToString(allInOne));
        }
    }

    partial class AllInOne : ISerialize
    {
        void ISerialize.Serialize<TSerializer, TSerializeType>(TSerializer serializer)
        {
            var rgb = serializer.SerializeType("Rgb", 3);
            rgb.SerializeField($"{nameof(ByteField)}", new ByteWrap(ByteField));
            rgb.SerializeField($"{nameof(UShortField)}", new UInt16Wrap(UShortField));
            rgb.SerializeField($"{nameof(UIntField)}", new UInt32Wrap(UIntField));
            rgb.SerializeField($"{nameof(ULongField)}", new UInt64Wrap(ULongField));
            rgb.End();
        }
    }
}
