
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;

namespace Serde.Test
{
    public class GeneratorDeserializeTests
    {
        [Fact]
        public Task Rgb()
        {
            var src = @"
using Serde;
[GenerateDeserialize]
partial struct Rgb
{
    public byte Red, Green, Blue;
}";
            return VerifyGeneratedCode(src, "Rgb", @"
#nullable enable
using Serde;

partial struct Rgb : Serde.IDeserialize<Rgb>
{
    static Rgb Serde.IDeserialize<Rgb>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{""Red"", ""Green"", ""Blue""};
        return deserializer.DeserializeType<Rgb, SerdeVisitor>(""Rgb"", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Rgb>
    {
        public string ExpectedTypeName => ""Rgb"";
        Rgb Serde.IDeserializeVisitor<Rgb>.VisitDictionary<D>(ref D d)
        {
            Rgb newType = new Rgb();
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case ""Red"":
                        newType.Red = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case ""Green"":
                        newType.Green = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case ""Blue"":
                        newType.Blue = d.GetNextValue<byte, ByteWrap>();
                        break;
                    default:
                        throw new InvalidDeserializeValueException(""Unexpected field or property name in type Rgb: '"" + key + ""'"");
                }
            }

            return newType;
        }
    }
}");
        }

        [Fact]
        public Task Array()
        {
            var src = @"
using Serde;
[GenerateDeserialize]
partial struct ArrayField
{
    public int[] IntArr = new[] { 1, 2, 3 };
}";
            return VerifyGeneratedCode(src, "ArrayField", @"
#nullable enable
using Serde;

partial struct ArrayField : Serde.IDeserialize<ArrayField>
{
    static ArrayField Serde.IDeserialize<ArrayField>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{""IntArr""};
        return deserializer.DeserializeType<ArrayField, SerdeVisitor>(""ArrayField"", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ArrayField>
    {
        public string ExpectedTypeName => ""ArrayField"";
        ArrayField Serde.IDeserializeVisitor<ArrayField>.VisitDictionary<D>(ref D d)
        {
            ArrayField newType = new ArrayField();
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case ""IntArr"":
                        newType.IntArr = d.GetNextValue<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                        break;
                    default:
                        throw new InvalidDeserializeValueException(""Unexpected field or property name in type ArrayField: '"" + key + ""'"");
                }
            }

            return newType;
        }
    }
}");
        }

        private static Task VerifyGeneratedCode(
            string src,
            params DiagnosticResult[] diagnostics)
            => GeneratorSerializeTests.VerifyGeneratedCode(src, System.Array.Empty<(string, string)>(), diagnostics);

        private static Task VerifyGeneratedCode(
            string src,
            string typeName,
            string expected,
            params DiagnosticResult[] diagnostics)
            => GeneratorSerializeTests.VerifyGeneratedCode(src, new[] { (typeName + ".IDeserialize", expected)}, diagnostics);
    }
}