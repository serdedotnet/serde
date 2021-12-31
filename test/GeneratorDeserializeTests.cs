
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

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
            return VerifyDeserialize(src, "Rgb", @"
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
            Serde.Option<byte> red = default;
            Serde.Option<byte> green = default;
            Serde.Option<byte> blue = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case ""Red"":
                        red = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case ""Green"":
                        green = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case ""Blue"":
                        blue = d.GetNextValue<byte, ByteWrap>();
                        break;
                    default:
                        break;
                }
            }

            Rgb newType = new Rgb()
            {Red = red.GetValueOrThrow(""Red""), Green = green.GetValueOrThrow(""Green""), Blue = blue.GetValueOrThrow(""Blue""), };
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
            return VerifyDeserialize(src, "ArrayField", @"
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
            Serde.Option<int[]> intarr = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case ""IntArr"":
                        intarr = d.GetNextValue<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                        break;
                    default:
                        break;
                }
            }

            ArrayField newType = new ArrayField()
            {IntArr = intarr.GetValueOrThrow(""IntArr""), };
            return newType;
        }
    }
}");
        }

        private static Task VerifyDeserialize(
            string src,
            string typeName,
            string expected,
            params DiagnosticResult[] diagnostics)
            => VerifyGeneratedCode(src, new[] { (typeName + ".IDeserialize", expected)}, diagnostics);
    }
}