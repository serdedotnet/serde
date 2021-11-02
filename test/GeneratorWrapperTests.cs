
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
            Point newType = new Point();
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case ""X"":
                        newType.X = d.GetNextValue<int, Int32Wrap>();
                        break;
                    case ""Y"":
                        newType.Y = d.GetNextValue<int, Int32Wrap>();
                        break;
                    default:
                        throw new InvalidDeserializeValueException(""Unexpected field or property name in type Point: '"" + key + ""'"");
                }
            }

            return newType;
        }
    }
}")
            });
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