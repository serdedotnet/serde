
using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test
{
    public class GeneratorWrapperTests
    {
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
            // /0/Test0.cs(3,18): error ERR_CantWrapSpecialType: The type 'string' can't be automatically wrapped because it is a built-in type.
            DiagnosticResult.CompilerError("ERR_CantWrapSpecialType").WithSpan(3, 18, 3, 28));

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
            // /0/Test0.cs(3,18): error ERR_CantWrapSpecialType: The type 'string' can't be automatically wrapped because it is a built-in type.
            DiagnosticResult.CompilerError("ERR_CantWrapSpecialType").WithSpan(3, 18, 3, 27));
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
            return VerifyGeneratedCode(src, new[] {
                ("PointWrap.ISerialize", @"
#nullable enable
using Serde;

partial struct PointWrap : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""Point"", 2);
        type.SerializeField(""x"", new Int32Wrap(_point.X));
        type.SerializeField(""y"", new Int32Wrap(_point.Y));
        type.End();
    }
}"),
                ("PointWrap.IDeserialize", @"
#nullable enable
using Serde;

partial struct PointWrap : Serde.IDeserialize<Point>
{
    static System.Threading.Tasks.ValueTask<Point> Serde.IDeserialize<Point>.Deserialize<D>(D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{""X"", ""Y""};
        return deserializer.DeserializeType<Point, SerdeVisitor>(""Point"", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Point>
    {
        public string ExpectedTypeName => ""Point"";
        async System.Threading.Tasks.ValueTask<Point> Serde.IDeserializeVisitor<Point>.VisitDictionary<D>(D d)
        {
            Serde.Option<int> x = default;
            Serde.Option<int> y = default;
            while (await d.TryGetNextKey<string, StringWrap>()is var nextOpt && nextOpt.HasValue)
            {
                switch (nextOpt.GetValueOrDefault())
                {
                    case ""x"":
                        x = await d.GetNextValue<int, Int32Wrap>();
                        break;
                    case ""y"":
                        y = await d.GetNextValue<int, Int32Wrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new Point()
            {X = x.GetValueOrThrow(""X""), Y = y.GetValueOrThrow(""Y""), };
            return newType;
        }
    }
}")
            });
        }

        [Fact]
        public Task NestedSerializeWrap()
        {
            var src = @"
using System.Collections.Specialized;
[Serde.GenerateSerialize]
partial class C
{
    public BitVector32.Section S = new BitVector32.Section();
}";
            return VerifyGeneratedCode(src, new[] {
                ("Serde.BitVector32SectionWrap", @"
namespace Serde
{
    internal readonly partial record struct BitVector32SectionWrap(System.Collections.Specialized.BitVector32.Section Value);
}"),
                ("Serde.BitVector32SectionWrap.ISerialize", @"
#nullable enable
using Serde;

namespace Serde
{
    partial record struct BitVector32SectionWrap : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType(""Section"", 2);
            type.SerializeField(""mask"", new Int16Wrap(Value.Mask));
            type.SerializeField(""offset"", new Int16Wrap(Value.Offset));
            type.End();
        }
    }
}"),
                ("C.ISerialize", @"
#nullable enable
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""C"", 1);
        type.SerializeField(""s"", new BitVector32SectionWrap(this.S));
        type.End();
    }
}")
            });
        }

        [Fact]
        public Task NestedDeserializeWrap()
        {
            var src = @"
using System.Collections.Specialized;
[Serde.GenerateDeserialize]
partial class C
{
    public BitVector32.Section S = new BitVector32.Section();
}";
            return VerifyGeneratedCode(src, new[] {
                ("Serde.BitVector32SectionWrap", @"
namespace Serde
{
    internal readonly partial record struct BitVector32SectionWrap(System.Collections.Specialized.BitVector32.Section Value);
}"),
                ("Serde.BitVector32SectionWrap.IDeserialize", @"
#nullable enable
using Serde;

namespace Serde
{
    partial record struct BitVector32SectionWrap : Serde.IDeserialize<System.Collections.Specialized.BitVector32.Section>
    {
        static System.Threading.Tasks.ValueTask<System.Collections.Specialized.BitVector32.Section> Serde.IDeserialize<System.Collections.Specialized.BitVector32.Section>.Deserialize<D>(D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]{""Mask"", ""Offset""};
            return deserializer.DeserializeType<System.Collections.Specialized.BitVector32.Section, SerdeVisitor>(""Section"", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<System.Collections.Specialized.BitVector32.Section>
        {
            public string ExpectedTypeName => ""System.Collections.Specialized.BitVector32.Section"";
            async System.Threading.Tasks.ValueTask<System.Collections.Specialized.BitVector32.Section> Serde.IDeserializeVisitor<System.Collections.Specialized.BitVector32.Section>.VisitDictionary<D>(D d)
            {
                Serde.Option<short> mask = default;
                Serde.Option<short> offset = default;
                while (await d.TryGetNextKey<string, StringWrap>()is var nextOpt && nextOpt.HasValue)
                {
                    switch (nextOpt.GetValueOrDefault())
                    {
                        case ""mask"":
                            mask = await d.GetNextValue<short, Int16Wrap>();
                            break;
                        case ""offset"":
                            offset = await d.GetNextValue<short, Int16Wrap>();
                            break;
                        default:
                            break;
                    }
                }

                var newType = new System.Collections.Specialized.BitVector32.Section()
                {Mask = mask.GetValueOrThrow(""Mask""), Offset = offset.GetValueOrThrow(""Offset""), };
                return newType;
            }
        }
    }
}"),
                ("C.IDeserialize", @"
#nullable enable
using Serde;

partial class C : Serde.IDeserialize<C>
{
    static System.Threading.Tasks.ValueTask<C> Serde.IDeserialize<C>.Deserialize<D>(D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{""S""};
        return deserializer.DeserializeType<C, SerdeVisitor>(""C"", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<C>
    {
        public string ExpectedTypeName => ""C"";
        async System.Threading.Tasks.ValueTask<C> Serde.IDeserializeVisitor<C>.VisitDictionary<D>(D d)
        {
            Serde.Option<System.Collections.Specialized.BitVector32.Section> s = default;
            while (await d.TryGetNextKey<string, StringWrap>()is var nextOpt && nextOpt.HasValue)
            {
                switch (nextOpt.GetValueOrDefault())
                {
                    case ""s"":
                        s = await d.GetNextValue<System.Collections.Specialized.BitVector32.Section, BitVector32SectionWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new C()
            {S = s.GetValueOrThrow(""S""), };
            return newType;
        }
    }
}") },
    // SerdeGenerator/Serde.Generator/Serde.BitVector32SectionWrap.IDeserialize.cs(39,18): error CS0200: Property or indexer 'BitVector32.Section.Mask' cannot be assigned to -- it is read only
    DiagnosticResult.CompilerError("CS0200").WithSpan("SerdeGenerator/Serde.Generator/Serde.BitVector32SectionWrap.IDeserialize.cs", 39, 18, 39, 22).WithArguments("System.Collections.Specialized.BitVector32.Section.Mask"),
    // SerdeGenerator/Serde.Generator/Serde.BitVector32SectionWrap.IDeserialize.cs(39,55): error CS0200: Property or indexer 'BitVector32.Section.Offset' cannot be assigned to -- it is read only
    DiagnosticResult.CompilerError("CS0200").WithSpan("SerdeGenerator/Serde.Generator/Serde.BitVector32SectionWrap.IDeserialize.cs", 39, 55, 39, 61).WithArguments("System.Collections.Specialized.BitVector32.Section.Offset"));

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
    static System.Threading.Tasks.ValueTask<R> Serde.IDeserialize<R>.Deserialize<D>(D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{"A", "B"};
        return deserializer.DeserializeType<R, SerdeVisitor>("R", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<R>
    {
        public string ExpectedTypeName => "R";
        async System.Threading.Tasks.ValueTask<R> Serde.IDeserializeVisitor<R>.VisitDictionary<D>(D d)
        {
            Serde.Option<int> a = default;
            Serde.Option<string> b = default;
            while (await d.TryGetNextKey<string, StringWrap>()is var nextOpt && nextOpt.HasValue)
            {
                switch (nextOpt.GetValueOrDefault())
                {
                    case "a":
                        a = await d.GetNextValue<int, Int32Wrap>();
                        break;
                    case "b":
                        b = await d.GetNextValue<string, StringWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new R(a.GetValueOrThrow("A"), b.GetValueOrThrow("B"))
            {};
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
            return VerifyGeneratedCode(src, new[] { ("Address.ISerialize", """

#nullable enable
using Serde;

partial class Address : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("Address", 5);
        type.SerializeField("name", new StringWrap(this.Name), new System.Attribute[]{new System.Xml.Serialization.XmlAttributeAttribute()
        {}, new Serde.SerdeMemberOptions()
        {ProvideAttributes = true}});
        type.SerializeField("line1", new StringWrap(this.Line1));
        type.SerializeField("city", new StringWrap(this.City));
        type.SerializeField("state", new StringWrap(this.State));
        type.SerializeField("zip", new StringWrap(this.Zip));
        type.End();
    }
}
""") });
        }
    }
}