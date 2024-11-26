
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
  <boolField>true</boolField>
  <charField>#</charField>
  <byteField>255</byteField>
  <uShortField>65535</uShortField>
  <uIntField>4294967295</uIntField>
  <uLongField>18446744073709551615</uLongField>
  <sByteField>127</sByteField>
  <shortField>32767</shortField>
  <intField>2147483647</intField>
  <longField>9223372036854775807</longField>
  <stringField>StringValue</stringField>
  <escapedStringField>+0 11 222 333 44</escapedStringField>
  <uIntArr>
    <int>1</int>
    <int>2</int>
    <int>3</int>
  </uIntArr>
  <nestedArr>
    <ArrayOfArrayOfInt32>
      <ArrayOfInt32>
        <int>1</int>
      </ArrayOfInt32>
      <ArrayOfInt32>
        <int>2</int>
      </ArrayOfInt32>
    </ArrayOfArrayOfInt32>
  </nestedArr>
  <intImm>
    <ImmutableArrayOfInt32>
      <int>1</int>
      <int>2</int>
    </ImmutableArrayOfInt32>
  </intImm>
  <color>blue</color>
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
        [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
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
        [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
        public partial class TypeWithArrayField
        {
            public StructWithIntField[] ArrayField = new[] {
                new StructWithIntField(1),
                new StructWithIntField(2),
                new StructWithIntField(3)
            };
        }

        [GenerateSerialize]
        [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
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
        [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
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
        [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
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
            where T : ISerializeProvider<T>
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