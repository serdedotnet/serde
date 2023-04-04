using System.Collections.Immutable;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Serde.Json;
using Xunit;

namespace Serde.Test
{
    public class AllInOneJsonTest
    {
        [Fact]
        public Task GeneratorTest()
        {
            var curPath = GetPath();
            var allInOnePath = Path.Combine(Path.GetDirectoryName(curPath)!, "AllInOneSrc.cs");

            var src = File.ReadAllText(allInOnePath);
            var serializeSrc = """

#nullable enable
using Serde;

namespace Serde.Test
{
    partial record AllInOne : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var type = serializer.SerializeType("AllInOne", 16);
            type.SerializeField("boolField", new BoolWrap(this.BoolField));
            type.SerializeField("charField", new CharWrap(this.CharField));
            type.SerializeField("byteField", new ByteWrap(this.ByteField));
            type.SerializeField("uShortField", new UInt16Wrap(this.UShortField));
            type.SerializeField("uIntField", new UInt32Wrap(this.UIntField));
            type.SerializeField("uLongField", new UInt64Wrap(this.ULongField));
            type.SerializeField("sByteField", new SByteWrap(this.SByteField));
            type.SerializeField("shortField", new Int16Wrap(this.ShortField));
            type.SerializeField("intField", new Int32Wrap(this.IntField));
            type.SerializeField("longField", new Int64Wrap(this.LongField));
            type.SerializeField("stringField", new StringWrap(this.StringField));
            type.SerializeFieldIfNotNull("nullStringField", new NullableRefWrap.SerializeImpl<string, StringWrap>(this.NullStringField), this.NullStringField);
            type.SerializeField("uIntArr", new ArrayWrap.SerializeImpl<uint, UInt32Wrap>(this.UIntArr));
            type.SerializeField("nestedArr", new ArrayWrap.SerializeImpl<int[], ArrayWrap.SerializeImpl<int, Int32Wrap>>(this.NestedArr));
            type.SerializeField("intImm", new ImmutableArrayWrap.SerializeImpl<int, Int32Wrap>(this.IntImm));
            type.SerializeField("color", new AllInOneColorEnumWrap(this.Color));
            type.End();
        }
    }
}
""";
            var deserializeSrc = @"
#nullable enable
using Serde;

namespace Serde.Test
{
    partial record AllInOne : Serde.IDeserialize<Serde.Test.AllInOne>
    {
        static Serde.Test.AllInOne Serde.IDeserialize<Serde.Test.AllInOne>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]
            {
                ""BoolField"",
                ""CharField"",
                ""ByteField"",
                ""UShortField"",
                ""UIntField"",
                ""ULongField"",
                ""SByteField"",
                ""ShortField"",
                ""IntField"",
                ""LongField"",
                ""StringField"",
                ""NullStringField"",
                ""UIntArr"",
                ""NestedArr"",
                ""IntImm"",
                ""Color""
            };
            return deserializer.DeserializeType<Serde.Test.AllInOne, SerdeVisitor>(""AllInOne"", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.AllInOne>
        {
            public string ExpectedTypeName => ""Serde.Test.AllInOne"";

            Serde.Test.AllInOne Serde.IDeserializeVisitor<Serde.Test.AllInOne>.VisitDictionary<D>(ref D d)
            {
                Serde.Option<bool> boolfield = default;
                Serde.Option<char> charfield = default;
                Serde.Option<byte> bytefield = default;
                Serde.Option<ushort> ushortfield = default;
                Serde.Option<uint> uintfield = default;
                Serde.Option<ulong> ulongfield = default;
                Serde.Option<sbyte> sbytefield = default;
                Serde.Option<short> shortfield = default;
                Serde.Option<int> intfield = default;
                Serde.Option<long> longfield = default;
                Serde.Option<string> stringfield = default;
                Serde.Option<string?> nullstringfield = default;
                Serde.Option<uint[]> uintarr = default;
                Serde.Option<int[][]> nestedarr = default;
                Serde.Option<System.Collections.Immutable.ImmutableArray<int>> intimm = default;
                Serde.Option<Serde.Test.AllInOne.ColorEnum> color = default;
                while (d.TryGetNextKey<string, StringWrap>(out string? key))
                {
                    switch (key)
                    {
                        case ""boolField"":
                            boolfield = d.GetNextValue<bool, BoolWrap>();
                            break;
                        case ""charField"":
                            charfield = d.GetNextValue<char, CharWrap>();
                            break;
                        case ""byteField"":
                            bytefield = d.GetNextValue<byte, ByteWrap>();
                            break;
                        case ""uShortField"":
                            ushortfield = d.GetNextValue<ushort, UInt16Wrap>();
                            break;
                        case ""uIntField"":
                            uintfield = d.GetNextValue<uint, UInt32Wrap>();
                            break;
                        case ""uLongField"":
                            ulongfield = d.GetNextValue<ulong, UInt64Wrap>();
                            break;
                        case ""sByteField"":
                            sbytefield = d.GetNextValue<sbyte, SByteWrap>();
                            break;
                        case ""shortField"":
                            shortfield = d.GetNextValue<short, Int16Wrap>();
                            break;
                        case ""intField"":
                            intfield = d.GetNextValue<int, Int32Wrap>();
                            break;
                        case ""longField"":
                            longfield = d.GetNextValue<long, Int64Wrap>();
                            break;
                        case ""stringField"":
                            stringfield = d.GetNextValue<string, StringWrap>();
                            break;
                        case ""nullStringField"":
                            nullstringfield = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                            break;
                        case ""uIntArr"":
                            uintarr = d.GetNextValue<uint[], ArrayWrap.DeserializeImpl<uint, UInt32Wrap>>();
                            break;
                        case ""nestedArr"":
                            nestedarr = d.GetNextValue<int[][], ArrayWrap.DeserializeImpl<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>>();
                            break;
                        case ""intImm"":
                            intimm = d.GetNextValue<System.Collections.Immutable.ImmutableArray<int>, ImmutableArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                            break;
                        case ""color"":
                            color = d.GetNextValue<Serde.Test.AllInOne.ColorEnum, AllInOneColorEnumWrap>();
                            break;
                        default:
                            break;
                    }
                }

                var newType = new Serde.Test.AllInOne()
                {
                    BoolField = boolfield.GetValueOrThrow(""BoolField""),
                    CharField = charfield.GetValueOrThrow(""CharField""),
                    ByteField = bytefield.GetValueOrThrow(""ByteField""),
                    UShortField = ushortfield.GetValueOrThrow(""UShortField""),
                    UIntField = uintfield.GetValueOrThrow(""UIntField""),
                    ULongField = ulongfield.GetValueOrThrow(""ULongField""),
                    SByteField = sbytefield.GetValueOrThrow(""SByteField""),
                    ShortField = shortfield.GetValueOrThrow(""ShortField""),
                    IntField = intfield.GetValueOrThrow(""IntField""),
                    LongField = longfield.GetValueOrThrow(""LongField""),
                    StringField = stringfield.GetValueOrThrow(""StringField""),
                    NullStringField = nullstringfield.GetValueOrDefault(null),
                    UIntArr = uintarr.GetValueOrThrow(""UIntArr""),
                    NestedArr = nestedarr.GetValueOrThrow(""NestedArr""),
                    IntImm = intimm.GetValueOrThrow(""IntImm""),
                    Color = color.GetValueOrThrow(""Color""),
                };
                return newType;
            }
        }
    }
}";

            return GeneratorTestUtils.VerifyGeneratedCode(src, new[] {
                ("Serde.AllInOneColorEnumWrap", @"
namespace Serde
{
    internal readonly partial record struct AllInOneColorEnumWrap(Serde.Test.AllInOne.ColorEnum Value);
}"),
                ("Serde.AllInOneColorEnumWrap.ISerialize", """

#nullable enable
using Serde;

namespace Serde
{
    partial record struct AllInOneColorEnumWrap : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var name = Value switch
            {
                Serde.Test.AllInOne.ColorEnum.Red => "red",
                Serde.Test.AllInOne.ColorEnum.Blue => "blue",
                Serde.Test.AllInOne.ColorEnum.Green => "green",
                _ => null
            };
            serializer.SerializeEnumValue("ColorEnum", name, new Int32Wrap((int)Value));
        }
    }
}
"""),
                ("Serde.Test.AllInOne.ISerialize", serializeSrc),
                ("Serde.AllInOneColorEnumWrap.IDeserialize", """

#nullable enable
using Serde;

namespace Serde
{
    partial record struct AllInOneColorEnumWrap : Serde.IDeserialize<Serde.Test.AllInOne.ColorEnum>
    {
        static Serde.Test.AllInOne.ColorEnum Serde.IDeserialize<Serde.Test.AllInOne.ColorEnum>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            return deserializer.DeserializeString<Serde.Test.AllInOne.ColorEnum, SerdeVisitor>(visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.AllInOne.ColorEnum>
        {
            public string ExpectedTypeName => "Serde.Test.AllInOne.ColorEnum";

            Serde.Test.AllInOne.ColorEnum Serde.IDeserializeVisitor<Serde.Test.AllInOne.ColorEnum>.VisitString(string s) => s switch
            {
                "red" => Serde.Test.AllInOne.ColorEnum.Red,
                "blue" => Serde.Test.AllInOne.ColorEnum.Blue,
                "green" => Serde.Test.AllInOne.ColorEnum.Green,
                _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + s)};
            Serde.Test.AllInOne.ColorEnum Serde.IDeserializeVisitor<Serde.Test.AllInOne.ColorEnum>.VisitUtf8Span(System.ReadOnlySpan<byte> s) => s switch
            {
                _ when System.MemoryExtensions.SequenceEqual(s, "red"u8) => Serde.Test.AllInOne.ColorEnum.Red,
                _ when System.MemoryExtensions.SequenceEqual(s, "blue"u8) => Serde.Test.AllInOne.ColorEnum.Blue,
                _ when System.MemoryExtensions.SequenceEqual(s, "green"u8) => Serde.Test.AllInOne.ColorEnum.Green,
                _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + System.Text.Encoding.UTF8.GetString(s))};
        }
    }
}
"""),
                ("Serde.Test.AllInOne.IDeserialize", deserializeSrc),
            });

            static string GetPath([CallerFilePath] string path = "") => path;
        }

        [Fact]
        public void SerializeTest()
        {
            var actual = JsonSerializerTests.PrettyPrint(JsonSerializer.Serialize(AllInOne.Sample));
            Assert.Equal(AllInOne.SampleSerialized.Trim(), actual);
        }

        [Fact]
        public void DeserializeTest()
        {
            var actual = JsonSerializer.Deserialize<AllInOne>(AllInOne.SampleSerialized);
            Assert.Equal(AllInOne.Sample, actual);
        }
    }
}
