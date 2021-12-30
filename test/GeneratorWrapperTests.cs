
using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;

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
            return VerifyGeneratedCode(src,
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
            return VerifyGeneratedCode(src,
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
            return GeneratorSerializeTests.VerifyGeneratedCode(src, new[] {
                ("PointWrap.ISerialize", @"
#nullable enable
using Serde;

partial struct PointWrap : Serde.ISerialize
{
    void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
    {
        var type = serializer.SerializeType(""Point"", 2);
        type.SerializeField(""X"", new Int32Wrap(_point.X));
        type.SerializeField(""Y"", new Int32Wrap(_point.Y));
        type.End();
    }
}"),
                ("PointWrap.IDeserialize", @"
#nullable enable
using Serde;

partial struct PointWrap : Serde.IDeserialize<Point>
{
    static Point Serde.IDeserialize<Point>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{""X"", ""Y""};
        return deserializer.DeserializeType<Point, SerdeVisitor>(""Point"", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Point>
    {
        public string ExpectedTypeName => ""Point"";
        Point Serde.IDeserializeVisitor<Point>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<int> x = default;
            Serde.Option<int> y = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case ""X"":
                        x = d.GetNextValue<int, Int32Wrap>();
                        break;
                    case ""Y"":
                        y = d.GetNextValue<int, Int32Wrap>();
                        break;
                    default:
                        throw new InvalidDeserializeValueException(""Unexpected field or property name in type Point: '"" + key + ""'"");
                }
            }

            Point newType = new Point()
            {X = x.Value, Y = y.Value, };
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
            return GeneratorSerializeTests.VerifyGeneratedCode(src, new[] {
                ("Serde.BitVector32SectionWrap", @"
using Section = System.Collections.Specialized.BitVector32.Section;
namespace Serde
{
    internal readonly partial record struct BitVector32SectionWrap(Section Value);
}"),
                ("Serde.BitVector32SectionWrap.ISerialize", @"
#nullable enable
using Serde;

namespace Serde
{
    partial record struct BitVector32SectionWrap : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
        {
            var type = serializer.SerializeType(""Section"", 2);
            type.SerializeField(""Mask"", new Int16Wrap(Value.Mask));
            type.SerializeField(""Offset"", new Int16Wrap(Value.Offset));
            type.End();
        }
    }
}"),
                ("C.ISerialize", @"
#nullable enable
using Serde;

partial class C : Serde.ISerialize
{
    void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
    {
        var type = serializer.SerializeType(""C"", 1);
        type.SerializeField(""S"", new BitVector32SectionWrap(this.S));
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
            return GeneratorSerializeTests.VerifyGeneratedCode(src, new[] {
                ("Serde.BitVector32SectionWrap", @"
using Section = System.Collections.Specialized.BitVector32.Section;
namespace Serde
{
    internal readonly partial record struct BitVector32SectionWrap(Section Value);
}"),
                ("Serde.BitVector32SectionWrap.IDeserialize", @"
#nullable enable
using Serde;

namespace Serde
{
    partial record struct BitVector32SectionWrap : Serde.IDeserialize<System.Collections.Specialized.BitVector32.Section>
    {
        static System.Collections.Specialized.BitVector32.Section Serde.IDeserialize<System.Collections.Specialized.BitVector32.Section>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]{""Mask"", ""Offset""};
            return deserializer.DeserializeType<System.Collections.Specialized.BitVector32.Section, SerdeVisitor>(""Section"", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<System.Collections.Specialized.BitVector32.Section>
        {
            public string ExpectedTypeName => ""System.Collections.Specialized.BitVector32.Section"";
            System.Collections.Specialized.BitVector32.Section Serde.IDeserializeVisitor<System.Collections.Specialized.BitVector32.Section>.VisitDictionary<D>(ref D d)
            {
                Serde.Option<short> mask = default;
                Serde.Option<short> offset = default;
                while (d.TryGetNextKey<string, StringWrap>(out string? key))
                {
                    switch (key)
                    {
                        case ""Mask"":
                            mask = d.GetNextValue<short, Int16Wrap>();
                            break;
                        case ""Offset"":
                            offset = d.GetNextValue<short, Int16Wrap>();
                            break;
                        default:
                            throw new InvalidDeserializeValueException(""Unexpected field or property name in type System.Collections.Specialized.BitVector32.Section: '"" + key + ""'"");
                    }
                }

                System.Collections.Specialized.BitVector32.Section newType = new System.Collections.Specialized.BitVector32.Section()
                {Mask = mask.Value, Offset = offset.Value, };
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
    static C Serde.IDeserialize<C>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{""S""};
        return deserializer.DeserializeType<C, SerdeVisitor>(""C"", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<C>
    {
        public string ExpectedTypeName => ""C"";
        C Serde.IDeserializeVisitor<C>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<System.Collections.Specialized.BitVector32.Section> s = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case ""S"":
                        s = d.GetNextValue<System.Collections.Specialized.BitVector32.Section, BitVector32SectionWrap>();
                        break;
                    default:
                        throw new InvalidDeserializeValueException(""Unexpected field or property name in type C: '"" + key + ""'"");
                }
            }

            C newType = new C()
            {S = s.Value, };
            return newType;
        }
    }
}") },
    // SerdeGenerator/Serde.Generator/Serde.BitVector32SectionWrap.IDeserialize.cs(39,18): error CS0200: Property or indexer 'BitVector32.Section.Mask' cannot be assigned to -- it is read only
    DiagnosticResult.CompilerError("CS0200").WithSpan("SerdeGenerator/Serde.Generator/Serde.BitVector32SectionWrap.IDeserialize.cs", 39, 18, 39, 22).WithArguments("System.Collections.Specialized.BitVector32.Section.Mask"),
    // SerdeGenerator/Serde.Generator/Serde.BitVector32SectionWrap.IDeserialize.cs(39,37): error CS0200: Property or indexer 'BitVector32.Section.Offset' cannot be assigned to -- it is read only
    DiagnosticResult.CompilerError("CS0200").WithSpan("SerdeGenerator/Serde.Generator/Serde.BitVector32SectionWrap.IDeserialize.cs", 39, 37, 39, 43).WithArguments("System.Collections.Specialized.BitVector32.Section.Offset"));

        }

        private static Task VerifyGeneratedCode(
            string src,
            params DiagnosticResult[] diagnostics)
            => GeneratorSerializeTests.VerifyGeneratedCode(src, Array.Empty<(string, string)>(), diagnostics);

        private static Task VerifyGeneratedCode(
            string src,
            string typeName,
            string expected,
            params DiagnosticResult[] diagnostics)
            => GeneratorSerializeTests.VerifyGeneratedCode(src, new[] { (typeName, expected)}, diagnostics);
    }
}