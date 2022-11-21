
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test
{
    public class GeneratorDeserializeTests
    {
        [Fact]
        public Task MemberSkip()
        {
            var src = """
using Serde;
[GenerateDeserialize]
partial struct Rgb
{
    public byte Red;
    [SerdeMemberOptions(Skip = true)]
    public byte Green;
    public byte Blue;
}
""";
            return VerifyDeserialize(src, "Rgb", """

#nullable enable
using Serde;

partial struct Rgb : Serde.IDeserialize<Rgb>
{
    static Rgb Serde.IDeserialize<Rgb>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{"Red", "Blue"};
        return deserializer.DeserializeType<Rgb, SerdeVisitor>("Rgb", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Rgb>
    {
        public string ExpectedTypeName => "Rgb";
        Rgb Serde.IDeserializeVisitor<Rgb>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<byte> red = default;
            Serde.Option<byte> blue = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "red":
                        red = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case "blue":
                        blue = d.GetNextValue<byte, ByteWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new Rgb()
            {Red = red.GetValueOrThrow("Red"), Blue = blue.GetValueOrThrow("Blue"), };
            return newType;
        }
    }
}
""");
        }

        [Fact]
        public Task MemberSkipSerialize()
        {
            var src = """
using Serde;
[GenerateDeserialize]
partial struct Rgb
{
    public byte Red;
    [SerdeMemberOptions(SkipDeserialize = true)]
    public byte Green;
    public byte Blue;
}
""";
            return VerifyDeserialize(src, "Rgb", """

#nullable enable
using Serde;

partial struct Rgb : Serde.IDeserialize<Rgb>
{
    static Rgb Serde.IDeserialize<Rgb>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{"Red", "Blue"};
        return deserializer.DeserializeType<Rgb, SerdeVisitor>("Rgb", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Rgb>
    {
        public string ExpectedTypeName => "Rgb";
        Rgb Serde.IDeserializeVisitor<Rgb>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<byte> red = default;
            Serde.Option<byte> blue = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "red":
                        red = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case "blue":
                        blue = d.GetNextValue<byte, ByteWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new Rgb()
            {Red = red.GetValueOrThrow("Red"), Blue = blue.GetValueOrThrow("Blue"), };
            return newType;
        }
    }
}
""");
        }

        [Fact]
        public Task MemberSkipDeserialize()
        {
            var src = """
using Serde;
[GenerateDeserialize]
partial struct Rgb
{
    public byte Red;
    [SerdeMemberOptions(SkipSerialize = true)]
    public byte Green;
    public byte Blue;
}
""";
            return VerifyDeserialize(src, "Rgb", """

#nullable enable
using Serde;

partial struct Rgb : Serde.IDeserialize<Rgb>
{
    static Rgb Serde.IDeserialize<Rgb>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{"Red", "Green", "Blue"};
        return deserializer.DeserializeType<Rgb, SerdeVisitor>("Rgb", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Rgb>
    {
        public string ExpectedTypeName => "Rgb";
        Rgb Serde.IDeserializeVisitor<Rgb>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<byte> red = default;
            Serde.Option<byte> green = default;
            Serde.Option<byte> blue = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "red":
                        red = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case "green":
                        green = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case "blue":
                        blue = d.GetNextValue<byte, ByteWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new Rgb()
            {Red = red.GetValueOrThrow("Red"), Green = green.GetValueOrThrow("Green"), Blue = blue.GetValueOrThrow("Blue"), };
            return newType;
        }
    }
}
""");
        }
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
                    case ""red"":
                        red = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case ""green"":
                        green = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case ""blue"":
                        blue = d.GetNextValue<byte, ByteWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new Rgb()
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
                    case ""f"":
                        f = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new S()
            {F = f.GetValueOrDefault(null), };
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
    public string? Missing { get; init; }
    [SerdeMemberOptions(ThrowIfMissing = true)]
    public string? ThrowMissing { get; init; }
} ";
            return VerifyDeserialize(src, "SetToNull", """

#nullable enable
using Serde;

partial record struct SetToNull : Serde.IDeserialize<SetToNull>
{
    static SetToNull Serde.IDeserialize<SetToNull>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{"Present", "Missing", "ThrowMissing"};
        return deserializer.DeserializeType<SetToNull, SerdeVisitor>("SetToNull", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<SetToNull>
    {
        public string ExpectedTypeName => "SetToNull";
        SetToNull Serde.IDeserializeVisitor<SetToNull>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<string> present = default;
            Serde.Option<string?> missing = default;
            Serde.Option<string?> throwmissing = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "present":
                        present = d.GetNextValue<string, StringWrap>();
                        break;
                    case "missing":
                        missing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        break;
                    case "throwMissing":
                        throwmissing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new SetToNull()
            {Present = present.GetValueOrThrow("Present"), Missing = missing.GetValueOrDefault(null), ThrowMissing = throwmissing.GetValueOrThrow("ThrowMissing"), };
            return newType;
        }
    }
}
""");
        }

        [Fact]
        public Task Array()
        {
            var src = @"
using Serde;
[GenerateDeserialize]
partial class ArrayField
{
    public int[] IntArr = new[] { 1, 2, 3 };
}";
            return VerifyDeserialize(src, "ArrayField", @"
#nullable enable
using Serde;

partial class ArrayField : Serde.IDeserialize<ArrayField>
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
                    case ""intArr"":
                        intarr = d.GetNextValue<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new ArrayField()
            {IntArr = intarr.GetValueOrThrow(""IntArr""), };
            return newType;
        }
    }
}");
        }

        [Fact]
        public Task EnumMember()
        {
            var src = @"
[Serde.GenerateDeserialize]
partial class C
{
    public ColorInt ColorInt;
    public ColorByte ColorByte;
    public ColorLong ColorLong;
    public ColorULong ColorULong;
}
public enum ColorInt { Red = 3, Green = 5, Blue = 7 }
public enum ColorByte : byte { Red = 3, Green = 5, Blue = 7 }
public enum ColorLong : long { Red = 3, Green = 5, Blue = 7 }
public enum ColorULong : ulong { Red = 3, Green = 5, Blue = 7 }
";

            return VerifyGeneratedCode(src, new[] {
                ("Serde.ColorIntWrap", @"
namespace Serde
{
    internal readonly partial record struct ColorIntWrap(ColorInt Value);
}"),
                ("Serde.ColorIntWrap.IDeserialize", """

#nullable enable
using Serde;

namespace Serde
{
    partial record struct ColorIntWrap : Serde.IDeserialize<ColorInt>
    {
        static ColorInt Serde.IDeserialize<ColorInt>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            return deserializer.DeserializeString<ColorInt, SerdeVisitor>(visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ColorInt>
        {
            public string ExpectedTypeName => "ColorInt";
            ColorInt Serde.IDeserializeVisitor<ColorInt>.VisitString(string s)
            {
                ColorInt enumValue;
                switch (s)
                {
                    case "red":
                        enumValue = ColorInt.Red;
                        break;
                    case "green":
                        enumValue = ColorInt.Green;
                        break;
                    case "blue":
                        enumValue = ColorInt.Blue;
                        break;
                    default:
                        throw new InvalidDeserializeValueException("Unexpected enum field name: " + s);
                }

                return enumValue;
            }
        }
    }
}
"""),
                ("Serde.ColorByteWrap", @"
namespace Serde
{
    internal readonly partial record struct ColorByteWrap(ColorByte Value);
}"),
                ("Serde.ColorByteWrap.IDeserialize", """

#nullable enable
using Serde;

namespace Serde
{
    partial record struct ColorByteWrap : Serde.IDeserialize<ColorByte>
    {
        static ColorByte Serde.IDeserialize<ColorByte>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            return deserializer.DeserializeString<ColorByte, SerdeVisitor>(visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ColorByte>
        {
            public string ExpectedTypeName => "ColorByte";
            ColorByte Serde.IDeserializeVisitor<ColorByte>.VisitString(string s)
            {
                ColorByte enumValue;
                switch (s)
                {
                    case "red":
                        enumValue = ColorByte.Red;
                        break;
                    case "green":
                        enumValue = ColorByte.Green;
                        break;
                    case "blue":
                        enumValue = ColorByte.Blue;
                        break;
                    default:
                        throw new InvalidDeserializeValueException("Unexpected enum field name: " + s);
                }

                return enumValue;
            }
        }
    }
}
"""),
                ("Serde.ColorLongWrap", @"
namespace Serde
{
    internal readonly partial record struct ColorLongWrap(ColorLong Value);
}"),
                ("Serde.ColorLongWrap.IDeserialize", """

#nullable enable
using Serde;

namespace Serde
{
    partial record struct ColorLongWrap : Serde.IDeserialize<ColorLong>
    {
        static ColorLong Serde.IDeserialize<ColorLong>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            return deserializer.DeserializeString<ColorLong, SerdeVisitor>(visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ColorLong>
        {
            public string ExpectedTypeName => "ColorLong";
            ColorLong Serde.IDeserializeVisitor<ColorLong>.VisitString(string s)
            {
                ColorLong enumValue;
                switch (s)
                {
                    case "red":
                        enumValue = ColorLong.Red;
                        break;
                    case "green":
                        enumValue = ColorLong.Green;
                        break;
                    case "blue":
                        enumValue = ColorLong.Blue;
                        break;
                    default:
                        throw new InvalidDeserializeValueException("Unexpected enum field name: " + s);
                }

                return enumValue;
            }
        }
    }
}
"""),
                ("Serde.ColorULongWrap", @"
namespace Serde
{
    internal readonly partial record struct ColorULongWrap(ColorULong Value);
}"),
                ("Serde.ColorULongWrap.IDeserialize", """

#nullable enable
using Serde;

namespace Serde
{
    partial record struct ColorULongWrap : Serde.IDeserialize<ColorULong>
    {
        static ColorULong Serde.IDeserialize<ColorULong>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            return deserializer.DeserializeString<ColorULong, SerdeVisitor>(visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ColorULong>
        {
            public string ExpectedTypeName => "ColorULong";
            ColorULong Serde.IDeserializeVisitor<ColorULong>.VisitString(string s)
            {
                ColorULong enumValue;
                switch (s)
                {
                    case "red":
                        enumValue = ColorULong.Red;
                        break;
                    case "green":
                        enumValue = ColorULong.Green;
                        break;
                    case "blue":
                        enumValue = ColorULong.Blue;
                        break;
                    default:
                        throw new InvalidDeserializeValueException("Unexpected enum field name: " + s);
                }

                return enumValue;
            }
        }
    }
}
"""),
                ("C.IDeserialize", """

#nullable enable
using Serde;

partial class C : Serde.IDeserialize<C>
{
    static C Serde.IDeserialize<C>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]{"ColorInt", "ColorByte", "ColorLong", "ColorULong"};
        return deserializer.DeserializeType<C, SerdeVisitor>("C", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<C>
    {
        public string ExpectedTypeName => "C";
        C Serde.IDeserializeVisitor<C>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<ColorInt> colorint = default;
            Serde.Option<ColorByte> colorbyte = default;
            Serde.Option<ColorLong> colorlong = default;
            Serde.Option<ColorULong> colorulong = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "colorInt":
                        colorint = d.GetNextValue<ColorInt, ColorIntWrap>();
                        break;
                    case "colorByte":
                        colorbyte = d.GetNextValue<ColorByte, ColorByteWrap>();
                        break;
                    case "colorLong":
                        colorlong = d.GetNextValue<ColorLong, ColorLongWrap>();
                        break;
                    case "colorULong":
                        colorulong = d.GetNextValue<ColorULong, ColorULongWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new C()
            {ColorInt = colorint.GetValueOrThrow("ColorInt"), ColorByte = colorbyte.GetValueOrThrow("ColorByte"), ColorLong = colorlong.GetValueOrThrow("ColorLong"), ColorULong = colorulong.GetValueOrThrow("ColorULong"), };
            return newType;
        }
    }
}
"""),
            });
        }

        private static Task VerifyDeserialize(
            string src,
            string typeName,
            string expected,
            params DiagnosticResult[] diagnostics)
            => VerifyGeneratedCode(src, new[] { (typeName + ".IDeserialize", expected)}, diagnostics);
    }
}