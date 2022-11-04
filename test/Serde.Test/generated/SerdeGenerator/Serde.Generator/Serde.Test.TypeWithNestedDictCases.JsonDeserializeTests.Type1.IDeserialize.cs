
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class TypeWithNestedDictCases
        {
            partial record Type1 : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1>
            {
                static Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1 Serde.IDeserialize<Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1>.Deserialize<D>(ref D deserializer)
                {
                    var visitor = new SerdeVisitor();
                    var fieldNames = new[]{"Field0"};
                    return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1, SerdeVisitor>("Type1", fieldNames, visitor);
                }

                private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1>
                {
                    public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1";
                    Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1 Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1>.VisitDictionary<D>(ref D d)
                    {
                        Serde.Option<int> field0 = default;
                        while (d.TryGetNextKey<string, StringWrap>(out string? key))
                        {
                            switch (key)
                            {
                                case "field0":
                                    field0 = d.GetNextValue<int, Int32Wrap>();
                                    break;
                                default:
                                    break;
                            }
                        }

                        var newType = new Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1()
                        {Field0 = field0.GetValueOrThrow("Field0"), };
                        return newType;
                    }
                }
            }
        }
    }
}