//HintName: PointWrap.IDeserialize.cs

#nullable enable
using Serde;

partial struct PointWrap : Serde.IDeserialize<Point>
{
    static Point Serde.IDeserialize<Point>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "X",
            "Y"
        };
        return deserializer.DeserializeType<Point, SerdeVisitor>("Point", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Point>
    {
        public string ExpectedTypeName => "Point";

        Point Serde.IDeserializeVisitor<Point>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<int> x = default;
            Serde.Option<int> y = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "x":
                        x = d.GetNextValue<int, Int32Wrap>();
                        break;
                    case "y":
                        y = d.GetNextValue<int, Int32Wrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new Point()
            {
                X = x.GetValueOrThrow("X"),
                Y = y.GetValueOrThrow("Y"),
            };
            return newType;
        }
    }
}