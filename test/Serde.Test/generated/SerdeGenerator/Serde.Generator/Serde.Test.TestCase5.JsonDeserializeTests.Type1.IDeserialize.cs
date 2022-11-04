
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class TestCase5
        {
            partial record Type1 : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.TestCase5.Type1>
            {
                static Serde.Test.JsonDeserializeTests.TestCase5.Type1 Serde.IDeserialize<Serde.Test.JsonDeserializeTests.TestCase5.Type1>.Deserialize<D>(ref D deserializer)
                {
                    var visitor = new SerdeVisitor();
                    var fieldNames = new[]{"Field0", "Field1", "Field2"};
                    return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.TestCase5.Type1, SerdeVisitor>("Type1", fieldNames, visitor);
                }

                private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.TestCase5.Type1>
                {
                    public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.TestCase5.Type1";
                    Serde.Test.JsonDeserializeTests.TestCase5.Type1 Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.TestCase5.Type1>.VisitDictionary<D>(ref D d)
                    {
                        Serde.Option<System.Collections.Generic.List<int>> field0 = default;
                        Serde.Option<Serde.Test.JsonDeserializeTests.TestCase5.Type2> field1 = default;
                        Serde.Option<Serde.Test.JsonDeserializeTests.TestCase5.Type3> field2 = default;
                        while (d.TryGetNextKey<string, StringWrap>(out string? key))
                        {
                            switch (key)
                            {
                                case "field0":
                                    field0 = d.GetNextValue<System.Collections.Generic.List<int>, ListWrap.DeserializeImpl<int, Int32Wrap>>();
                                    break;
                                case "field1":
                                    field1 = d.GetNextValue<Serde.Test.JsonDeserializeTests.TestCase5.Type2, Serde.Test.JsonDeserializeTests.TestCase5.Type2>();
                                    break;
                                case "field2":
                                    field2 = d.GetNextValue<Serde.Test.JsonDeserializeTests.TestCase5.Type3, Serde.Test.JsonDeserializeTests.TestCase5.Type3>();
                                    break;
                                default:
                                    break;
                            }
                        }

                        var newType = new Serde.Test.JsonDeserializeTests.TestCase5.Type1()
                        {Field0 = field0.GetValueOrThrow("Field0"), Field1 = field1.GetValueOrThrow("Field1"), Field2 = field2.GetValueOrThrow("Field2"), };
                        return newType;
                    }
                }
            }
        }
    }
}