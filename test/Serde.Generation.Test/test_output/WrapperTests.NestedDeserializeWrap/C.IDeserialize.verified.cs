//HintName: C.IDeserialize.cs

#nullable enable
using Serde;

partial class C : Serde.IDeserialize<C>
{
    static C Serde.IDeserialize<C>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "S"
        };
        return deserializer.DeserializeType<C, SerdeVisitor>("C", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<C>
    {
        public string ExpectedTypeName => "C";

        C Serde.IDeserializeVisitor<C>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<System.Collections.Specialized.BitVector32.Section> s = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "s":
                        s = d.GetNextValue<System.Collections.Specialized.BitVector32.Section, BitVector32SectionWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new C()
            {
                S = s.GetValueOrThrow("S"),
            };
            return newType;
        }
    }
}