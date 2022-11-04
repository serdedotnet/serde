
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class TestCase5
        {
            partial record Type3 : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.TestCase5.Type3>
            {
                static Serde.Test.JsonDeserializeTests.TestCase5.Type3 Serde.IDeserialize<Serde.Test.JsonDeserializeTests.TestCase5.Type3>.Deserialize<D>(ref D deserializer)
                {
                    var visitor = new SerdeVisitor();
                    var fieldNames = new[]{"Field0"};
                    return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.TestCase5.Type3, SerdeVisitor>("Type3", fieldNames, visitor);
                }

                private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.TestCase5.Type3>
                {
                    public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.TestCase5.Type3";
                    Serde.Test.JsonDeserializeTests.TestCase5.Type3 Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.TestCase5.Type3>.VisitDictionary<D>(ref D d)
                    {
                        Serde.Option<bool> field0 = default;
                        while (d.TryGetNextKey<string, StringWrap>(out string? key))
                        {
                            switch (key)
                            {
                                case "field0":
                                    field0 = d.GetNextValue<bool, BoolWrap>();
                                    break;
                                default:
                                    break;
                            }
                        }

                        var newType = new Serde.Test.JsonDeserializeTests.TestCase5.Type3()
                        {Field0 = field0.GetValueOrThrow("Field0"), };
                        return newType;
                    }
                }
            }
        }
    }
}