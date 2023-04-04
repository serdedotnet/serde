
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class GenericWrapperTests
    {
        partial record struct CustomArrayWrapExplicitOnType : Serde.IDeserialize<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>
        {
            static Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType Serde.IDeserialize<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>.Deserialize<D>(ref D deserializer)
            {
                var visitor = new SerdeVisitor();
                var fieldNames = new[]
                {
                    "A"
                };
                return deserializer.DeserializeType<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType, SerdeVisitor>("CustomArrayWrapExplicitOnType", fieldNames, visitor);
            }

            private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>
            {
                public string ExpectedTypeName => "Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType";
                Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType Serde.IDeserializeVisitor<Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType>.VisitDictionary<D>(ref D d)
                {
                    Serde.Option<Serde.Test.GenericWrapperTests.CustomImArray2<int>> a = default;
                    while (d.TryGetNextKey<string, StringWrap>(out string? key))
                    {
                        switch (key)
                        {
                            case "a":
                                a = d.GetNextValue<Serde.Test.GenericWrapperTests.CustomImArray2<int>, Serde.Test.GenericWrapperTests.CustomImArray2Wrap.DeserializeImpl<int, Int32Wrap>>();
                                break;
                            default:
                                break;
                        }
                    }

                    var newType = new Serde.Test.GenericWrapperTests.CustomArrayWrapExplicitOnType()
                    {
                        A = a.GetValueOrThrow("A"),
                    };
                    return newType;
                }
            }
        }
    }
}