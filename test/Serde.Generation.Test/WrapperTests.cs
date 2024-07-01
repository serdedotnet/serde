
using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using VerifyXunit;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test
{
    public class WrapperTests
    {
        [Fact]
        public Task NestedExplicitWrapper()
        {
            var src = """

using Serde;
using System.Collections.Immutable;
using System.Collections.Specialized;

partial class Outer
{
    [GenerateSerialize(Through = nameof(Value))]
    public readonly partial record struct SectionWrap(BitVector32.Section Value);
}

[GenerateSerialize]
partial struct S
{
    [SerdeMemberOptions(
        WrapperSerialize = typeof(ImmutableArrayWrap.SerializeImpl<BitVector32.Section, Outer.SectionWrap>))]
    public ImmutableArray<BitVector32.Section> Sections;
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

[GenerateSerde(Through = nameof(Value))]
readonly partial record struct OPTSWrap(BIND_OPTS Value);

""";
            return VerifyMultiFile(src);
        }

        [Fact]
        public Task StringWrap()
        {
            var src = @"
using Serde;
[GenerateWrapper(nameof(_s))]
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
[GenerateWrapper(""Wrapped"")]
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
[GenerateWrapper(nameof(_point))]
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

[Serde.GenerateWrapper(nameof(Value))]
internal readonly partial record struct OPTSWrap(BIND_OPTS Value);

[Serde.GenerateDeserialize]
partial class C
{
    [Serde.SerdeWrap(typeof(OPTSWrap))]
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
[SerdeTypeOptions(ConstructorSignature = typeof((int, string)))]
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
   public string Name;
   public string Line1;

   /* Setting the IsNullable property to false instructs the
      XmlSerializer that the XML attribute will not appear if
      the City field is set to a null reference. */
   [XmlElementAttribute(IsNullable = false)]
   public string City;
   public string State;
   public string Zip;
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

[GenerateWrapper(nameof(Value))]
internal partial record struct RecursiveWrap(Recursive Value);

[GenerateSerde]
public partial record Parent
{
    [SerdeWrap(typeof(RecursiveWrap))]
    public Recursive R { get; init; }
}
""";
            await VerifyMultiFile(src, new[] { comp.EmitToImageReference() });
        }
    }
}