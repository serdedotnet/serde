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
            "ColorInt",
            "ColorByte",
            "ColorLong",
            "ColorULong"
        };
        return deserializer.DeserializeType<C, SerdeVisitor>("C", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<C>
    {
        public string ExpectedTypeName => "C";

        C Serde.IDeserializeVisitor<C>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<ColorInt> colorint = default;
            Serde.Option<ColorByte> colorbyte = default;
            Serde.Option<ColorLong> colorlong = default;
            Serde.Option<ColorULong> colorulong = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "colorInt":
                        colorint = d.GetNextValue<ColorInt, ColorIntWrap>();
                        break;
                    case "colorByte":
                        colorbyte = d.GetNextValue<ColorByte, ColorByteWrap>();
                        break;
                    case "colorLong":
                        colorlong = d.GetNextValue<ColorLong, ColorLongWrap>();
                        break;
                    case "colorULong":
                        colorulong = d.GetNextValue<ColorULong, ColorULongWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new C()
            {
                ColorInt = colorint.GetValueOrThrow("ColorInt"),
                ColorByte = colorbyte.GetValueOrThrow("ColorByte"),
                ColorLong = colorlong.GetValueOrThrow("ColorLong"),
                ColorULong = colorulong.GetValueOrThrow("ColorULong"),
            };
            return newType;
        }
    }
}