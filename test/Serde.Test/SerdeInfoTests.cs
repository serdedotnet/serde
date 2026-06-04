
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

    [Fact]
    public void WithName_PreservesFieldsAndKind()
    {
        var original = SerdeInfoProvider.GetDeserializeInfo<Rgb, RgbProxy>();
        var renamed = original.WithName("NewRgb");

        Assert.Equal("NewRgb", renamed.Name);
        Assert.Equal(original.Kind, renamed.Kind);
        Assert.Equal(original.FieldCount, renamed.FieldCount);
        Assert.Equal(original.Attributes, renamed.Attributes);

        for (int i = 0; i < original.FieldCount; i++)
        {
            Assert.Equal(original.GetFieldStringName(i), renamed.GetFieldStringName(i));
            Assert.Equal(original.GetFieldAttributes(i), renamed.GetFieldAttributes(i));
            Assert.Equal(original.GetFieldInfo(i), renamed.GetFieldInfo(i));
        }

        Assert.Equal(0, renamed.TryGetIndex("r"u8));
        Assert.Equal(1, renamed.TryGetIndex("g"u8));
        Assert.Equal(2, renamed.TryGetIndex("b"u8));
    }

    [Fact]
    public void MakeCustom_NoMemberInfo_SimpleOverload()
    {
        var info = SerdeInfo.MakeCustom(
            "SimpleType",
            System.Array.Empty<System.Reflection.CustomAttributeData>(),
            new (string, ISerdeInfo)[] {
                ("x", I32Proxy.SerdeInfo),
                ("y", StringProxy.SerdeInfo)
            });

        Assert.Equal("SimpleType", info.Name);
        Assert.Equal(InfoKind.CustomType, info.Kind);
        Assert.Equal(2, info.FieldCount);
        Assert.Equal("x", info.GetFieldStringName(0));
        Assert.Equal("y", info.GetFieldStringName(1));
        Assert.Equal(0, info.TryGetIndex("x"u8));
        Assert.Equal(1, info.TryGetIndex("y"u8));
        Assert.Empty(info.GetFieldAttributes(0));
        Assert.Empty(info.GetFieldAttributes(1));
        Assert.Equal(I32Proxy.SerdeInfo, info.GetFieldInfo(0));
        Assert.Equal(StringProxy.SerdeInfo, info.GetFieldInfo(1));
    }

    [Fact]
    public void MakeCustom_ExplicitFieldAttributes()
    {
        var info = SerdeInfo.MakeCustom(
            "AttrType",
            System.Array.Empty<System.Reflection.CustomAttributeData>(),
            new (string, ISerdeInfo, IList<System.Reflection.CustomAttributeData>)[] {
                ("a", I32Proxy.SerdeInfo, System.Array.Empty<System.Reflection.CustomAttributeData>()),
                ("b", StringProxy.SerdeInfo, System.Array.Empty<System.Reflection.CustomAttributeData>())
            });

        Assert.Equal("AttrType", info.Name);
        Assert.Equal(InfoKind.CustomType, info.Kind);
        Assert.Equal(2, info.FieldCount);
        Assert.Equal("a", info.GetFieldStringName(0));
        Assert.Equal("b", info.GetFieldStringName(1));
        Assert.Equal(0, info.TryGetIndex("a"u8));
        Assert.Equal(1, info.TryGetIndex("b"u8));
        Assert.Equal(I32Proxy.SerdeInfo, info.GetFieldInfo(0));
        Assert.Equal(StringProxy.SerdeInfo, info.GetFieldInfo(1));
    }

    [Fact]
    public void MakeCustom_SyntheticCustomAttributeData()
    {
        // Verify that CustomAttributeData can be subclassed to synthesize
        // attribute metadata for types that aren't actually loaded.
        var syntheticAttr = new SyntheticCustomAttributeData(
            typeof(DefaultValueAttribute),
            [new CustomAttributeTypedArgument(typeof(string), "synthetic")],
            []);

        var info = SerdeInfo.MakeCustom(
            "SyntheticType",
            new CustomAttributeData[] { syntheticAttr },
            new (string, ISerdeInfo, IList<CustomAttributeData>)[] {
                ("field1", I32Proxy.SerdeInfo, new CustomAttributeData[] { syntheticAttr }),
                ("field2", StringProxy.SerdeInfo, System.Array.Empty<CustomAttributeData>())
            });

        Assert.Equal("SyntheticType", info.Name);
        Assert.Equal(InfoKind.CustomType, info.Kind);

        // Type-level synthetic attribute
        var typeAttr = Assert.Single(info.Attributes);
        Assert.Equal(typeof(DefaultValueAttribute), typeAttr.AttributeType);
        Assert.Equal("synthetic", typeAttr.ConstructorArguments[0].Value);

        // Field-level synthetic attribute
        var fieldAttr = Assert.Single(info.GetFieldAttributes(0));
        Assert.Equal(typeof(DefaultValueAttribute), fieldAttr.AttributeType);
        Assert.Equal("synthetic", fieldAttr.ConstructorArguments[0].Value);

        // Second field has no attributes
        Assert.Empty(info.GetFieldAttributes(1));
    }

    /// <summary>
    /// A subclass of CustomAttributeData that can be constructed without
    /// a real metadata-backed type, for synthesizing ISerdeInfo.
    /// </summary>
    private sealed class SyntheticCustomAttributeData : CustomAttributeData
    {
        private readonly Type _attributeType;
        private readonly IList<CustomAttributeTypedArgument> _constructorArguments;
        private readonly IList<CustomAttributeNamedArgument> _namedArguments;

        public SyntheticCustomAttributeData(
            Type attributeType,
            IList<CustomAttributeTypedArgument> constructorArguments,
            IList<CustomAttributeNamedArgument> namedArguments)
        {
            _attributeType = attributeType;
            _constructorArguments = constructorArguments;
            _namedArguments = namedArguments;
        }

        public override Type AttributeType => _attributeType;
        public override IList<CustomAttributeTypedArgument> ConstructorArguments => _constructorArguments;
        public override IList<CustomAttributeNamedArgument> NamedArguments => _namedArguments;
    }

#pragma warning restore SerdeExperimentalFieldInfo // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}