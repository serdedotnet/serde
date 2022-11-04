
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class TestCase5
        {
            partial record Type2 : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.TestCase5.Type2>
            {
                static Serde.Test.JsonDeserializeTests.TestCase5.Type2 Serde.IDeserialize<Serde.Test.JsonDeserializeTests.TestCase5.Type2>.Deserialize<D>(ref D deserializer)
                {
                    var visitor = new SerdeVisitor();
                    var fieldNames = new[]{"Field0"};
                    return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.TestCase5.Type2, SerdeVisitor>("Type2", fieldNames, visitor);
                }

                private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.TestCase5.Type2>
                {
                    public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.TestCase5.Type2";
                    Serde.Test.JsonDeserializeTests.TestCase5.Type2 Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.TestCase5.Type2>.VisitDictionary<D>(ref D d)
                    {
                        Serde.Option<char> field0 = default;
                        while (d.TryGetNextKey<string, StringWrap>(out string? key))
                        {
                            switch (key)
                            {
                                case "field0":
                                    field0 = d.GetNextValue<char, CharWrap>();
                                    break;
                                default:
                                    break;
                            }
                        }

                        var newType = new Serde.Test.JsonDeserializeTests.TestCase5.Type2()
                        {Field0 = field0.GetValueOrThrow("Field0"), };
                        return newType;
                    }
                }
            }
        }
    }
}