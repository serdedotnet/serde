
using Xunit;

namespace Serde.Test;

public sealed partial class SerdeInfoTests
{
    [GenerateDeserialize]
    public partial record EmptyRecord;

    [Fact]
    public void TestEmptyRecord()
    {
        var info = SerdeInfoProvider.GetInfo<EmptyRecord>();
        Assert.Equal(nameof(EmptyRecord), info.Name);
        Assert.Equal(0, info.FieldCount);
    }

    public partial record Rgb
    {
        public byte R, G, B;
    }

    [GenerateDeserialize(ThroughType = typeof(Rgb))]
    public partial record RgbProxy;

    [Fact]
    public void TestProxy()
    {
        var info = SerdeInfoProvider.GetInfo<RgbProxy>();
        Assert.Equal(nameof(Rgb), info.Name);
        Assert.Equal(3, info.FieldCount);
    }
}