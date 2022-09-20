
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace Serde.Test
{
    public partial class XmlTests
    {
        private const string AllInOneSerialized = """
<?xml version="1.0" encoding="utf-16"?>
<AllInOne>
  <BoolField>true</BoolField>
  <CharField>#</CharField>
  <ByteField>255</ByteField>
  <UShortField>65535</UShortField>
  <UIntField>4294967295</UIntField>
  <ULongField>18446744073709551615</ULongField>
  <SByteField>127</SByteField>
  <ShortField>32767</ShortField>
  <IntField>2147483647</IntField>
  <LongField>9223372036854775807</LongField>
  <StringField>StringValue</StringField>
  <UIntArr>
    <int>1</int>
    <int>2</int>
    <int>3</int>
  </UIntArr>
  <NestedArr>
    <ArrayOfArrayOfInt32>
      <ArrayOfInt32>
        <int>1</int>
      </ArrayOfInt32>
      <ArrayOfInt32>
        <int>2</int>
      </ArrayOfInt32>
    </ArrayOfArrayOfInt32>
  </NestedArr>
  <IntImm>
    <ImmutableArrayOfInt32>
      <int>1</int>
      <int>2</int>
    </ImmutableArrayOfInt32>
  </IntImm>
  <Color />
</AllInOne>
""";

        [Fact]
        public void AllInOneTest()
        {
            var result = XmlSerializer.Serialize(AllInOne.Sample);
            var sxs = SxsSerialize(AllInOne.Sample);
            Assert.Equal(AllInOneSerialized, result);
        }

        private static string SxsSerialize<T>(T t)
        {
            var tmpX = new System.Xml.Serialization.XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
            {
                tmpX.Serialize(xmlWriter, t);
            }
            return stringWriter.ToString();
        }

        [GenerateSerialize]
        public partial class NestedArrays
        {
            public int[][][] A = new[] { new[] { new[] { 1, 2, 3 } } };
        }

        [Fact]
        public void NestedArrTest()
        {
            var result = XmlSerializer.Serialize(new NestedArrays());
            const string expected = """
<?xml version="1.0" encoding="utf-16"?>
<NestedArrays>
  <A>
    <ArrayOfArrayOfInt32>
      <ArrayOfInt32>
        <int>1</int>
        <int>2</int>
        <int>3</int>
      </ArrayOfInt32>
    </ArrayOfArrayOfInt32>
  </A>
</NestedArrays>
""";
            Assert.Equal(expected, result);
        }

        [GenerateSerialize]
        public partial class TypeWithArrayField
        {
            public StructWithIntField[] ArrayField = new[] {
                new StructWithIntField(1),
                new StructWithIntField(2),
                new StructWithIntField(3)
            };
        }

        [GenerateSerialize]
        public partial record StructWithIntField(int X)
        {
            public StructWithIntField() : this(11) { }
        }

        [Fact]
        public void ArrayWithType()
        {
            var s = SxsSerialize(new TypeWithArrayField());

            var result = XmlSerializer.SerializeIndented(new TypeWithArrayField());
            var expected = """
<?xml version="1.0" encoding="utf-16"?>
<TypeWithArrayField>
  <ArrayField>
    <StructWithIntField>
      <X>1</X>
    </StructWithIntField>
    <StructWithIntField>
      <X>2</X>
    </StructWithIntField>
    <StructWithIntField>
      <X>3</X>
    </StructWithIntField>
  </ArrayField>
</TypeWithArrayField>
""";
            Assert.Equal(expected, result);
        }

        [GenerateSerialize]
        public partial struct BoolStruct
        {
            public bool BoolField;
        }

        [Fact]
        public void BoolStructTest()
        {
            var b = new BoolStruct() { BoolField = true };
            var result = XmlSerializer.Serialize(b);
            const string expected = """
<?xml version="1.0" encoding="utf-16"?>
<BoolStruct>
  <BoolField>true</BoolField>
</BoolStruct>
""";
            Assert.Equal(expected.Trim(), result);
        }

        [GenerateSerialize]
        public partial class MapTest1
        {
            public Dictionary<string, int> MapField = new Dictionary<string, int>()
            {
                ["abc"] = 1,
                ["def"] = 2
            };
        }

        [Fact]
        public void DictionaryTest()
        {
            Assert.Throws<NotSupportedException>(() => XmlSerializer.Serialize(new MapTest1()));
        }

        private static void VerifyCompatSerialize<T>(T t, string expected)
            where T : ISerialize
        {
            var result = LegacySerialize(t);
            Assert.Equal(expected, result);
            result = XmlSerializer.Serialize(t);
            Assert.Equal(expected, result);
        }

        public static string LegacySerialize<T>(T t)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
            {
                serializer.Serialize(writer, t, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
            return stringWriter.ToString();
        }
    }
}