
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class TestCase5
        {
            partial record Type0 : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.TestCase5.Type0>
            {
                static Serde.Test.JsonDeserializeTests.TestCase5.Type0 Serde.IDeserialize<Serde.Test.JsonDeserializeTests.TestCase5.Type0>.Deserialize<D>(ref D deserializer)
                {
                    var visitor = new SerdeVisitor();
                    var fieldNames = new[]{"Field0"};
                    return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.TestCase5.Type0, SerdeVisitor>("Type0", fieldNames, visitor);
                }

                private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.TestCase5.Type0>
                {
                    public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.TestCase5.Type0";
                    Serde.Test.JsonDeserializeTests.TestCase5.Type0 Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.TestCase5.Type0>.VisitDictionary<D>(ref D d)
                    {
                        Serde.Option<Serde.Test.JsonDeserializeTests.TestCase5.Type1> field0 = default;
                        while (d.TryGetNextKey<string, StringWrap>(out string? key))
                        {
                            switch (key)
                            {
                                case "field0":
                                    field0 = d.GetNextValue<Serde.Test.JsonDeserializeTests.TestCase5.Type1, Serde.Test.JsonDeserializeTests.TestCase5.Type1>();
                                    break;
                                default:
                                    break;
                            }
                        }

                        var newType = new Serde.Test.JsonDeserializeTests.TestCase5.Type0()
                        {Field0 = field0.GetValueOrThrow("Field0"), };
                        return newType;
                    }
                }
            }
        }
    }
}