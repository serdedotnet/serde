
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using StaticCs;
using Xunit;

namespace Serde.Test;

public sealed partial class SerdeInfoTests
{
#pragma warning disable SerdeExperimentalFieldInfo // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    [GenerateDeserialize]
    public partial record EmptyRecord;

    [Fact]
    public void TestEmptyRecord()
    {
        var info = SerdeInfoProvider.GetDeserializeInfo<EmptyRecord>();
        Assert.Equal(nameof(EmptyRecord), info.Name);
        Assert.Equal(0, info.FieldCount);
    }

    public partial record Rgb
    {
        public byte R, G, B;
    }

    [GenerateDeserialize(ForType = typeof(Rgb))]
    public partial record RgbProxy;

    [Fact]
    public void TestProxy()
    {
        var info = SerdeInfoProvider.GetDeserializeInfo<Rgb, RgbProxy>();
        Assert.Equal(nameof(Rgb), info.Name);
        Assert.Equal(3, info.FieldCount);
    }

    [GenerateDeserialize]
    [Closed]
    public abstract partial record UnionBase
    {
        private UnionBase() { }

        [DefaultValue("A")]
        public record A : UnionBase;
        [DefaultValue("B")]
        public record B : UnionBase;
    }

    [Fact]
    public void TestUnionInfo()
    {
        var info = SerdeInfoProvider.GetDeserializeInfo<UnionBase>();
        Assert.Equal(nameof(UnionBase), info.Name);
        Assert.Equal(2, info.FieldCount);
        Assert.Equal(0, info.TryGetIndex("A"u8));
        Assert.Equal(1, info.TryGetIndex("B"u8));

        var aInfo = info.GetFieldInfo(0);
        Assert.Equal(nameof(UnionBase.A), aInfo.Name);
        Assert.Equal(aInfo.Attributes, info.GetFieldAttributes(0));
        var attr = Assert.Single(aInfo.Attributes, a => a.AttributeType == typeof(DefaultValueAttribute));
        Assert.Equal("A", attr.ConstructorArguments[0].Value);

        var bInfo = info.GetFieldInfo(1);
        Assert.Equal(nameof(UnionBase.B), bInfo.Name);
        Assert.Equal(bInfo.Attributes, info.GetFieldAttributes(1));
        attr = Assert.Single(bInfo.Attributes, a => a.AttributeType == typeof(DefaultValueAttribute));
        Assert.Equal("B", attr.ConstructorArguments[0].Value);
    }

    [Fact]
    public void NullableInfo()
    {
        var info = SerdeInfoProvider.GetSerializeInfo<string?, NullableRefProxy.Ser<string, StringProxy>>();
        Assert.Equal(InfoKind.Nullable, info.Kind);
        Assert.Equal("string?", info.Name);
        Assert.Equal(1, info.FieldCount);
        Assert.Equal(0, info.TryGetIndex("Value"u8));
        Assert.Equal(SerdeInfoProvider.GetDeserializeInfo<string, StringProxy>(), info.GetFieldInfo(0));
    }

#pragma warning restore SerdeExperimentalFieldInfo // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}