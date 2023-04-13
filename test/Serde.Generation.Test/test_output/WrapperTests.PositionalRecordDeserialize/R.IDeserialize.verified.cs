//HintName: R.IDeserialize.cs

#nullable enable
using Serde;

partial record R : Serde.IDeserialize<R>
{
    static R Serde.IDeserialize<R>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "A",
            "B"
        };
        return deserializer.DeserializeType<R, SerdeVisitor>("R", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<R>
    {
        public string ExpectedTypeName => "R";

        R Serde.IDeserializeVisitor<R>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<int> a = default;
            Serde.Option<string> b = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "a":
                        a = d.GetNextValue<int, Int32Wrap>();
                        break;
                    case "b":
                        b = d.GetNextValue<string, StringWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new R(a.GetValueOrThrow("A"), b.GetValueOrThrow("B"))
            {
            };
            return newType;
        }
    }
}