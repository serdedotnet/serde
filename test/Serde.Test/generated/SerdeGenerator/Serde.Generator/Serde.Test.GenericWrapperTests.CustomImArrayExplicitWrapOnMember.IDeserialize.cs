
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class GenericWrapperTests
    {
        partial record struct CustomImArrayExplicitWrapOnMember : Serde.IDeserialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>
        {
            static Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember Serde.IDeserialize<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>.Deserialize<D>(ref D deserializer)
            {
                var visitor = new SerdeVisitor();
                var fieldNames = new[]
                {
                    "A"
                };
                return deserializer.DeserializeType<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember, SerdeVisitor>("CustomImArrayExplicitWrapOnMember", fieldNames, visitor);
            }

            private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>
            {
                public string ExpectedTypeName => "Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember";
                Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember Serde.IDeserializeVisitor<Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember>.VisitDictionary<D>(ref D d)
                {
                    Serde.Option<Serde.Test.GenericWrapperTests.CustomImArray<int>> a = default;
                    while (d.TryGetNextKey<string, StringWrap>(out string? key))
                    {
                        switch (key)
                        {
                            case "a":
                                a = d.GetNextValue<Serde.Test.GenericWrapperTests.CustomImArray<int>, Serde.Test.GenericWrapperTests.CustomImArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                                break;
                            default:
                                break;
                        }
                    }

                    var newType = new Serde.Test.GenericWrapperTests.CustomImArrayExplicitWrapOnMember()
                    {
                        A = a.GetValueOrThrow("A"),
                    };
                    return newType;
                }
            }
        }
    }
}