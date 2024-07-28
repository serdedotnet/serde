
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
        Assert.Equal(nameof(EmptyRecord), info.TypeName);
        Assert.Equal(0, info.FieldCount);
    }
}