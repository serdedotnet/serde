//HintName: Rgb.IDeserialize.cs

#nullable enable
using Serde;

partial struct Rgb : Serde.IDeserialize<Rgb>
{
    static Rgb Serde.IDeserialize<Rgb>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "Red",
            "Green",
            "Blue"
        };
        return deserializer.DeserializeType<Rgb, SerdeVisitor>("Rgb", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Rgb>
    {
        public string ExpectedTypeName => "Rgb";

        Rgb Serde.IDeserializeVisitor<Rgb>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<byte> red = default;
            Serde.Option<byte> green = default;
            Serde.Option<byte> blue = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "red":
                        red = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case "green":
                        green = d.GetNextValue<byte, ByteWrap>();
                        break;
                    case "blue":
                        blue = d.GetNextValue<byte, ByteWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new Rgb()
            {
                Red = red.GetValueOrThrow("Red"),
                Green = green.GetValueOrThrow("Green"),
                Blue = blue.GetValueOrThrow("Blue"),
            };
            return newType;
        }
    }
}