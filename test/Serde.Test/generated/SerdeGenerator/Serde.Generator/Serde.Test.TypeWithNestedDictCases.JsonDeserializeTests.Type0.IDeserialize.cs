
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class TypeWithNestedDictCases
        {
            partial record Type0 : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type0>
            {
                static Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type0 Serde.IDeserialize<Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type0>.Deserialize<D>(ref D deserializer)
                {
                    var visitor = new SerdeVisitor();
                    var fieldNames = new[]{"Field0", "Field1"};
                    return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type0, SerdeVisitor>("Type0", fieldNames, visitor);
                }

                private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type0>
                {
                    public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type0";
                    Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type0 Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type0>.VisitDictionary<D>(ref D d)
                    {
                        Serde.Option<System.Collections.Generic.Dictionary<string, Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1>> field0 = default;
                        Serde.Option<int> field1 = default;
                        while (d.TryGetNextKey<string, StringWrap>(out string? key))
                        {
                            switch (key)
                            {
                                case "field0":
                                    field0 = d.GetNextValue<System.Collections.Generic.Dictionary<string, Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1>, DictWrap.DeserializeImpl<string, StringWrap, Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1, Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type1>>();
                                    break;
                                case "field1":
                                    field1 = d.GetNextValue<int, Int32Wrap>();
                                    break;
                                default:
                                    break;
                            }
                        }

                        var newType = new Serde.Test.JsonDeserializeTests.TypeWithNestedDictCases.Type0()
                        {Field0 = field0.GetValueOrThrow("Field0"), Field1 = field1.GetValueOrThrow("Field1"), };
                        return newType;
                    }
                }
            }
        }
    }
}