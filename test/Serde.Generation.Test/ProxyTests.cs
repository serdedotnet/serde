
using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using VerifyXunit;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test
{
    public class ProxyTests
    {
        [Fact]
        public Task ExplicitInvalidGenericWrapper()
        {
            var src = """
using System;
using System.Collections.Generic;
using System.Reflection;
using Serde;

[GenerateDeserialize]
partial record struct Original()
{
    public required string Name { get; init; }
}

internal sealed class Proxy : ISerialize<Original>, IDeserialize<Original>,
    ISerializeProvider<Original>, IDeserializeProvider<Original>
{
    public static Proxy Instance { get; } = new Proxy();
    static ISerialize<Original> ISerializeProvider<Original>.Instance => Instance;
    static IDeserialize<Original> IDeserializeProvider<Original>.Instance => Instance;

    public ISerdeInfo SerdeInfo { get; } = StringProxy.SerdeInfo;

    public Original Deserialize(IDeserializer deserializer)
    {
        var str = StringProxy.Instance.Deserialize(deserializer);
        return new Original { Name = str };
    }

    public void Serialize(Original value, ISerializer serializer)
    {
        serializer.WriteString(value.Name);
    }
}

[GenerateDeserialize]
partial record Container
{
    // Wrong wrapper type, should have a NullableWrapper outside
    [SerdeMemberOptions(DeserializeProxy = typeof(Proxy))]
    public Original? SdkDir { get; init; } = null;
}
""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task ExplicitNullableProxy()
        {
            var src = """
using System;
using System.Collections.Generic;
using System.Reflection;
using Serde;

[GenerateDeserialize]
partial record struct Original()
{
    public required string Name { get; init; }
}

internal class Proxy : ISerialize<Original>, IDeserialize<Original>,
    ISerializeProvider<Original>, IDeserializeProvider<Original>
{
    public static Proxy Instance { get; } = new Proxy();
    static ISerialize<Original> ISerializeProvider<Original>.Instance => Instance;
    static IDeserialize<Original> IDeserializeProvider<Original>.Instance => Instance;
    private Proxy() { }

    public ISerdeInfo SerdeInfo { get; } = StringProxy.SerdeInfo;

    public Original Deserialize(IDeserializer deserializer)
    {
        var str = StringProxy.Instance.Deserialize(deserializer);
        return new Original { Name = str };
    }

    public void Serialize(Original value, ISerializer serializer)
    {
        serializer.WriteString(value.Name);
    }
}

[GenerateDeserialize]
partial record Container
{
    [SerdeMemberOptions(DeserializeProxy = typeof(NullableProxy.De<Original, Proxy>))]
    public Original? SdkDir { get; init; } = null;
}
""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task NestedExplicitWrapper()
        {
            var src = """

using Serde;
using System.Collections.Immutable;
using System.Collections.Specialized;

partial class Outer
{
    [GenerateSerialize(ForType = typeof(BitVector32.Section))]
    public readonly partial record struct SectionWrap {}
}

[GenerateSerialize]
partial struct S
{
    [SerdeMemberOptions(
        SerializeProxy = typeof(ImmutableArrayProxy.Ser<BitVector32.Section, Outer.SectionWrap>))]
    public ImmutableArray<BitVector32.Section> Sections;
}
""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task InvalidNestedWrapper()
        {
            var src = """
using Serde;
using System.Collections.Generic;
using System.Collections.Specialized;

partial class Outer
{
    [GenerateSerialize(ForType = typeof(BitVector32.Section))]
    public sealed partial class SectionWrap {}
}

[GenerateSerialize]
partial struct S
{
    [SerdeMemberOptions(
        // Wrong outer wrapper type
        SerializeProxy = typeof(ArrayProxy.Ser<BitVector32.Section, Outer.SectionWrap>))]
    public List<int> Sections;
}
""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task GenerateSerdeWrap()
        {
            var src = """
using System.Runtime.InteropServices.ComTypes;
using Serde;

[GenerateSerde(ForType = typeof(BIND_OPTS))]
readonly partial record struct OPTSWrap {}

""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task StringWrap()
        {
            var src = @"
using Serde;
[GenerateSerde(ForType = typeof(string))]
readonly partial struct StringWrap
{
    private readonly string _s;
    public StringWrap(string s)
    {
        _s = s;
    }
}";
            return VerifyDiagnostics(src,
            // (3,18): error ERR_CantWrapSpecialType: The type 'string' can't be automatically wrapped because it is a built-in type.
            DiagnosticResult.CompilerError("ERR_CantWrapSpecialType").WithSpan("", 3, 2, 3, 29));

        }

        [Fact]
        public Task RecordStringWrap()
        {
            var src = @"
using Serde;
[GenerateSerde(ForType = typeof(string))]
partial record struct StringWrap(string Wrapped);
";
            return VerifyDiagnostics(src,
            // (3,2): error ERR_CantWrapSpecialType: The type 'string' can't be automatically wrapped because it is a built-in type.
            DiagnosticResult.CompilerError("ERR_CantWrapSpecialType").WithSpan("", 3, 2, 3, 28));
        }

        [Fact]
        public Task PointWrap()
        {
            var src = @"
using Serde;

class Point
{
    public int X;
    public int Y;
}
[GenerateSerde(ForType = typeof(Point))]
partial struct PointWrap
{
    private readonly Point _point;
    public PointWrap(Point point)
    {
        _point = point;
    }
}";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task NestedDeserializeWrap()
        {
            var src = @"
using System.Runtime.InteropServices.ComTypes;

[Serde.GenerateSerde(ForType = typeof(BIND_OPTS))]
internal sealed partial class OPTSWrap {}

[Serde.GenerateDeserialize]
partial class C
{
    [Serde.SerdeMemberOptions(Proxy = typeof(OPTSWrap))]
    public BIND_OPTS S = new BIND_OPTS();
}";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task PositionalRecordDeserialize()
        {
            var src = """
using Serde;

[GenerateDeserialize]
partial record R(int A, string B);
""";
            return VerifyGeneratedCode(src, "R.IDeserialize", """

#nullable enable
using Serde;

partial record R : Serde.IDeserialize<R>
{
    static R Serde.IDeserialize<R>.Deserialize(IDeserializer deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "A",
            "B"
        };
        return deserializer.DeserializeType<R, SerdeVisitor>("R", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<R>
    {
        public string ExpectedTypeName => "R";

        R Serde.IDeserializeVisitor<R>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<int> a = default;
            Serde.Option<string> b = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "a":
                        a = d.GetNextValue<int, Int32Wrap>();
                        break;
                    case "b":
                        b = d.GetNextValue<string, StringWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new R(a.GetValueOrThrow("A"), b.GetValueOrThrow("B"))
            {
            };
            return newType;
        }
    }
}
""");
        }

        [Fact]
        public Task AttributeWrapperTest()
        {
            var src = @"
using Serde;
using System;
using System.Xml;
using System.Xml.Serialization;

[GenerateSerialize]
public partial class Address
{
   /* The XmlAttribute instructs the XmlSerializer to serialize the Name
      field as an XML attribute instead of an XML element (the default
      behavior). */
   [XmlAttribute]
   [SerdeMemberOptions(ProvideAttributes = true)]
   public required string Name;
   public required string Line1;

   /* Setting the IsNullable property to false instructs the
      XmlSerializer that the XML attribute will not appear if
      the City field is set to a null reference. */
   [XmlElementAttribute(IsNullable = false)]
   public required string City;
   public required string State;
   public required string Zip;
}";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task ImmutableArrayEnumDeserialize()
        {
            var src = """
using System.Collections.Immutable;
namespace Test;

[Serde.GenerateDeserialize]
public enum Channel { A, B, C }

[Serde.GenerateDeserialize]
internal partial record struct ChannelList
{
    public ImmutableArray<Channel> Channels { get; init; }
}
""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public async Task RecursiveType()
        {
            var comp = await CreateCompilation("""
public partial record Recursive
{
    public Recursive? Next { get; init; }
}
""");
            var src = """
using Serde;
namespace Test;

[GenerateSerde(ForType = typeof(Recursive))]
internal sealed partial class RecursiveWrap {}

[GenerateSerde]
public partial record Parent
{
    [SerdeMemberOptions(Proxy = typeof(RecursiveWrap))]
    public required Recursive R { get; init; }
}
""";
            await VerifyMultiFile(src, new[] { comp.EmitToImageReference() });
        }
    }
}