
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
        public Task NullableRefField()
        {
            var src = @"
[Serde.GenerateDeserialize]
partial struct S
{
    public string? F;
}";
            return VerifyDeserialize(src, "S", @"
#nullable enable
using Serde;

partial struct S : Serde.IDeserialize<S>
{
    static S Serde.IDeserialize<S>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{""F""};
        return deserializer.DeserializeType<S, SerdeVisitor>(""S"", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<S>
    {
        public string ExpectedTypeName => ""S"";
        S Serde.IDeserializeVisitor<S>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<string?> f = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case ""F"":
                        f = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        break;
                    default:
                        break;
                }
            }

            S newType = new S()
            {F = f.GetValueOrThrow(""F""), };
            return newType;
        }
    }
}");
        }

        [Fact]
        public Task DeserializeMissing()
        {
            var src = @"
using Serde;
[GenerateDeserialize]
readonly partial record struct SetToNull
{
    public string Present { get; init; }
    [SerdeMemberOptions(NullIfMissing = true)]
    public string? Missing { get; init; }
} ";
            return VerifyDeserialize(src, "SetToNull", @"
#nullable enable
using Serde;

partial record struct SetToNull : Serde.IDeserialize<SetToNull>
{
    static SetToNull Serde.IDeserialize<SetToNull>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{""Present"", ""Missing""};
        return deserializer.DeserializeType<SetToNull, SerdeVisitor>(""SetToNull"", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<SetToNull>
    {
        public string ExpectedTypeName => ""SetToNull"";
        SetToNull Serde.IDeserializeVisitor<SetToNull>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<string> present = default;
            Serde.Option<string?> missing = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case ""Present"":
                        present = d.GetNextValue<string, StringWrap>();
                        break;
                    case ""Missing"":
                        missing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        break;
                    default:
                        break;
                }
            }

            SetToNull newType = new SetToNull()
            {Present = present.GetValueOrThrow(""Present""), Missing = missing.GetValueOrDefault(null), };
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