
using System.Collections.Generic;
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

    [Theory]
    [InlineData(PrimitiveKind.Bool)]
    [InlineData(PrimitiveKind.Char)]
    [InlineData(PrimitiveKind.U8)]
    [InlineData(PrimitiveKind.U16)]
    [InlineData(PrimitiveKind.U32)]
    [InlineData(PrimitiveKind.U64)]
    [InlineData(PrimitiveKind.I8)]
    [InlineData(PrimitiveKind.I16)]
    [InlineData(PrimitiveKind.I32)]
    [InlineData(PrimitiveKind.I64)]
    [InlineData(PrimitiveKind.F32)]
    [InlineData(PrimitiveKind.F64)]
    [InlineData(PrimitiveKind.Decimal)]
    [InlineData(PrimitiveKind.String)]
    public void PrimitiveKind_ReturnsCorrectKind(PrimitiveKind expectedKind)
    {
        var info = expectedKind switch
        {
            PrimitiveKind.Bool => BoolProxy.SerdeInfo,
            PrimitiveKind.Char => CharProxy.SerdeInfo,
            PrimitiveKind.U8 => U8Proxy.SerdeInfo,
            PrimitiveKind.U16 => U16Proxy.SerdeInfo,
            PrimitiveKind.U32 => U32Proxy.SerdeInfo,
            PrimitiveKind.U64 => U64Proxy.SerdeInfo,
            PrimitiveKind.I8 => I8Proxy.SerdeInfo,
            PrimitiveKind.I16 => I16Proxy.SerdeInfo,
            PrimitiveKind.I32 => I32Proxy.SerdeInfo,
            PrimitiveKind.I64 => I64Proxy.SerdeInfo,
            PrimitiveKind.F32 => F32Proxy.SerdeInfo,
            PrimitiveKind.F64 => F64Proxy.SerdeInfo,
            PrimitiveKind.Decimal => DecimalProxy.SerdeInfo,
            PrimitiveKind.String => StringProxy.SerdeInfo,
            _ => throw new System.ArgumentException($"Unknown kind: {expectedKind}")
        };

        Assert.Equal(InfoKind.Primitive, info.Kind);
        Assert.Equal(expectedKind, info.PrimitiveKind);
    }

    [Fact]
    public void PrimitiveKind_NullForNonPrimitives()
    {
        // Custom types should have null PrimitiveKind
        var customInfo = SerdeInfoProvider.GetDeserializeInfo<EmptyRecord>();
        Assert.Equal(InfoKind.CustomType, customInfo.Kind);
        Assert.Null(customInfo.PrimitiveKind);

        // Nullable types should have null PrimitiveKind
        var nullableInfo = SerdeInfoProvider.GetSerializeInfo<string?, NullableRefProxy.Ser<string, StringProxy>>();
        Assert.Equal(InfoKind.Nullable, nullableInfo.Kind);
        Assert.Null(nullableInfo.PrimitiveKind);

        // List types should have null PrimitiveKind
        var listInfo = SerdeInfoProvider.GetInfo(List<int>.Serialize);
        Assert.Equal(InfoKind.List, listInfo.Kind);
        Assert.Null(listInfo.PrimitiveKind);

        // Dictionary types should have null PrimitiveKind
        var dictInfo = SerdeInfoProvider.GetInfo(Dictionary<string, int>.Serialize);
        Assert.Equal(InfoKind.Dictionary, dictInfo.Kind);
        Assert.Null(dictInfo.PrimitiveKind);
    }

    [Fact]
    public void ListInfo_GetFieldInfo_ReturnsElementInfo()
    {
        var listInfo = SerdeInfoProvider.GetInfo(List<int>.Serialize);

        Assert.Equal(InfoKind.List, listInfo.Kind);
        Assert.Equal(1, listInfo.FieldCount);

        var elementInfo = listInfo.GetFieldInfo(0);
        Assert.Equal(I32Proxy.SerdeInfo, elementInfo);
        Assert.Equal(InfoKind.Primitive, elementInfo.Kind);
        Assert.Equal(PrimitiveKind.I32, elementInfo.PrimitiveKind);
    }

    [Fact]
    public void DictionaryInfo_GetFieldInfo_ReturnsKeyAndValueInfo()
    {
        var dictInfo = SerdeInfoProvider.GetInfo(Dictionary<string, int>.Serialize);

        Assert.Equal(InfoKind.Dictionary, dictInfo.Kind);
        Assert.Equal(2, dictInfo.FieldCount);

        var keyInfo = dictInfo.GetFieldInfo(0);
        Assert.Equal(StringProxy.SerdeInfo, keyInfo);
        Assert.Equal(InfoKind.Primitive, keyInfo.Kind);
        Assert.Equal(PrimitiveKind.String, keyInfo.PrimitiveKind);

        var valueInfo = dictInfo.GetFieldInfo(1);
        Assert.Equal(I32Proxy.SerdeInfo, valueInfo);
        Assert.Equal(InfoKind.Primitive, valueInfo.Kind);
        Assert.Equal(PrimitiveKind.I32, valueInfo.PrimitiveKind);
    }

#pragma warning restore SerdeExperimentalFieldInfo // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}