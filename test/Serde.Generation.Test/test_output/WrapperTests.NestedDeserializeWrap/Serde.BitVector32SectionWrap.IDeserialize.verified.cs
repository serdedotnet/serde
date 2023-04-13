//HintName: Serde.BitVector32SectionWrap.IDeserialize.cs

#nullable enable
using Serde;

namespace Serde
{
    partial record struct BitVector32SectionWrap : Serde.IDeserialize<System.Collections.Specialized.BitVector32.Section>
    {
        static System.Collections.Specialized.BitVector32.Section Serde.IDeserialize<System.Collections.Specialized.BitVector32.Section>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            var fieldNames = new[]
            {
                "Mask",
                "Offset"
            };
            return deserializer.DeserializeType<System.Collections.Specialized.BitVector32.Section, SerdeVisitor>("Section", fieldNames, visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<System.Collections.Specialized.BitVector32.Section>
        {
            public string ExpectedTypeName => "System.Collections.Specialized.BitVector32.Section";

            System.Collections.Specialized.BitVector32.Section Serde.IDeserializeVisitor<System.Collections.Specialized.BitVector32.Section>.VisitDictionary<D>(ref D d)
            {
                Serde.Option<short> mask = default;
                Serde.Option<short> offset = default;
                while (d.TryGetNextKey<string, StringWrap>(out string? key))
                {
                    switch (key)
                    {
                        case "mask":
                            mask = d.GetNextValue<short, Int16Wrap>();
                            break;
                        case "offset":
                            offset = d.GetNextValue<short, Int16Wrap>();
                            break;
                        default:
                            break;
                    }
                }

                var newType = new System.Collections.Specialized.BitVector32.Section()
                {
                    Mask = mask.GetValueOrThrow("Mask"),
                    Offset = offset.GetValueOrThrow("Offset"),
                };
                return newType;
            }
        }
    }
}