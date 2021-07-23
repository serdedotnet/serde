using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Serde.Test
{
    public class AllInOneTest
    {
        [Fact]
        public Task GeneratorTest()
        {
            var curPath = GetPath();
            var allInOnePath = Path.Combine(Path.GetDirectoryName(curPath)!, "AllInOneSrc.cs");

            var src = File.ReadAllText(allInOnePath);
            // Add [GenerateSerde] to the class
            src = src.Replace("public partial class AllInOne", @"[GenerateSerde] public partial class AllInOne");
            var expected = @"
using Serde;

namespace Serde.Test
{
    public partial class AllInOne : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize<TSerializer, TSerializeType, TSerializeEnumerable, TSerializeDictionary>(ref TSerializer serializer)
        {
            var type = serializer.SerializeType(""AllInOne"", 14);
            type.SerializeField(""BoolField"", new BoolWrap(BoolField));
            type.SerializeField(""CharField"", new CharWrap(CharField));
            type.SerializeField(""ByteField"", new ByteWrap(ByteField));
            type.SerializeField(""UShortField"", new UInt16Wrap(UShortField));
            type.SerializeField(""UIntField"", new UInt32Wrap(UIntField));
            type.SerializeField(""ULongField"", new UInt64Wrap(ULongField));
            type.SerializeField(""SByteField"", new SByteWrap(SByteField));
            type.SerializeField(""ShortField"", new Int16Wrap(ShortField));
            type.SerializeField(""IntField"", new Int32Wrap(IntField));
            type.SerializeField(""LongField"", new Int64Wrap(LongField));
            type.SerializeField(""StringField"", new StringWrap(StringField));
            type.SerializeField(""IntArr"", new ArrayWrap<int, Int32Wrap>(IntArr));
            type.SerializeField(""NestedArr"", new ArrayWrap<int[], ArrayWrap<int, Int32Wrap>>(NestedArr));
            type.SerializeField(""IntImm"", new ImmutableArrayWrap<int, Int32Wrap>(IntImm));
            type.End();
        }
    }
}";
            return GeneratorTests.VerifyGeneratedCode(src, "Serde.Test.AllInOne", expected);

            static string GetPath([CallerFilePath] string path = "") => path;
        }

        [Fact]
        public void JsonEmitTest()
        {
            var allInOne = new AllInOne();
            var expected = @"
{
  ""BoolField"": true,
  ""CharField"": ""#"",
  ""ByteField"": 255,
  ""UShortField"": 65535,
  ""UIntField"": 4294967295,
  ""ULongField"": 18446744073709551615,
  ""SByteField"": 127,
  ""ShortField"": 32767,
  ""IntField"": 2147483647,
  ""LongField"": 9223372036854775807,
  ""StringField"": ""StringValue"",
  ""IntArr"": [
    1,
    2,
    3
  ],
  ""NestedArr"": [
    [
      1
    ],
    [
      2
    ]
  ],
  ""IntImm"": [
    1,
    2
  ]
}";
            var actual = JsonSerializerTests.PrettyPrint(JsonSerializer.Serialize(allInOne));
            Assert.Equal(expected.Trim(), actual);
        }
    }
}
