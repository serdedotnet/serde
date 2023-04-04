
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial record struct ThrowMissing : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ThrowMissing>
        {
            static Serde.Test.JsonDeserializeTests.ThrowMissing Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ThrowMissing>.Deserialize<D>(ref D deserializer)
            {
                var visitor = new SerdeVisitor();
                var fieldNames = new[]
                {
                    "Present",
                    "Missing"
                };
                return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.ThrowMissing, SerdeVisitor>("ThrowMissing", fieldNames, visitor);
            }

            private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.ThrowMissing>
            {
                public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.ThrowMissing";
                Serde.Test.JsonDeserializeTests.ThrowMissing Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.ThrowMissing>.VisitDictionary<D>(ref D d)
                {
                    Serde.Option<string> present = default;
                    Serde.Option<string?> missing = default;
                    while (d.TryGetNextKey<string, StringWrap>(out string? key))
                    {
                        switch (key)
                        {
                            case "present":
                                present = d.GetNextValue<string, StringWrap>();
                                break;
                            case "missing":
                                missing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                                break;
                            default:
                                break;
                        }
                    }

                    var newType = new Serde.Test.JsonDeserializeTests.ThrowMissing()
                    {
                        Present = present.GetValueOrThrow("Present"),
                        Missing = missing.GetValueOrThrow("Missing"),
                    };
                    return newType;
                }
            }
        }
    }
}