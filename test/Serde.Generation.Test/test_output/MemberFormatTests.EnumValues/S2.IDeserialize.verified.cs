//HintName: S2.IDeserialize.cs

#nullable enable
using Serde;

partial struct S2 : Serde.IDeserialize<S2>
{
    static S2 Serde.IDeserialize<S2>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "E"
        };
        return deserializer.DeserializeType<S2, SerdeVisitor>("S2", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<S2>
    {
        public string ExpectedTypeName => "S2";

        S2 Serde.IDeserializeVisitor<S2>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<ColorEnum> e = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "E":
                        e = d.GetNextValue<ColorEnum, ColorEnumWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new S2()
            {
                E = e.GetValueOrThrow("E"),
            };
            return newType;
        }
    }
}