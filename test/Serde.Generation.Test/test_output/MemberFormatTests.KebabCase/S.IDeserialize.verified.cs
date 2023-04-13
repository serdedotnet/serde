//HintName: S.IDeserialize.cs

#nullable enable
using Serde;

partial struct S : Serde.IDeserialize<S>
{
    static S Serde.IDeserialize<S>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "One",
            "TwoWord"
        };
        return deserializer.DeserializeType<S, SerdeVisitor>("S", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<S>
    {
        public string ExpectedTypeName => "S";

        S Serde.IDeserializeVisitor<S>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<int> one = default;
            Serde.Option<int> twoword = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "one":
                        one = d.GetNextValue<int, Int32Wrap>();
                        break;
                    case "two-word":
                        twoword = d.GetNextValue<int, Int32Wrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new S()
            {
                One = one.GetValueOrThrow("One"),
                TwoWord = twoword.GetValueOrThrow("TwoWord"),
            };
            return newType;
        }
    }
}