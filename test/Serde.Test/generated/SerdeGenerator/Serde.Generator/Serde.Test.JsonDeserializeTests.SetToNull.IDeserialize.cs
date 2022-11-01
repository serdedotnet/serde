﻿
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial record struct SetToNull : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SetToNull>
        {
            static Serde.Test.JsonDeserializeTests.SetToNull Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SetToNull>.Deserialize<D>(ref D deserializer)
            {
                var visitor = new SerdeVisitor();
                var fieldNames = new[]{"Present", "Missing"};
                return deserializer.DeserializeType<Serde.Test.JsonDeserializeTests.SetToNull, SerdeVisitor>("SetToNull", fieldNames, visitor);
            }

            private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.SetToNull>
            {
                public string ExpectedTypeName => "Serde.Test.JsonDeserializeTests.SetToNull";
                Serde.Test.JsonDeserializeTests.SetToNull Serde.IDeserializeVisitor<Serde.Test.JsonDeserializeTests.SetToNull>.VisitDictionary<D>(ref D d)
                {
                    Serde.Option<string> present = default;
                    Serde.Option<string?> missing = default;
                    while (d.TryGetNextKey<D, string, StringWrap>(out string? key))
                    {
                        switch (key)
                        {
                            case "present":
                                present = d.GetNextValue<D, string, StringWrap>();
                                break;
                            case "missing":
                                missing = d.GetNextValue<D, string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                                break;
                            default:
                                break;
                        }
                    }

                    var newType = new Serde.Test.JsonDeserializeTests.SetToNull()
                    {Present = present.GetValueOrThrow("Present"), Missing = missing.GetValueOrDefault(null), };
                    return newType;
                }
            }
        }
    }
}