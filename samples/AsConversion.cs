using System;
using System.Globalization;
using Serde;
using Serde.Json;

namespace AsConversionSample;

// An RGB color is naturally three bytes, but is often represented on the wire as a "#RRGGBB" hex
// string. The `As` option serializes and deserializes the declaring type *as* another type, going
// through user-defined conversions. You must define both directions:
//   Rgb    -> string  (used when serializing)
//   string -> Rgb     (used when deserializing)
[GenerateSerde(As = typeof(string))]
partial record struct Rgb(byte R, byte G, byte B)
{
    public static explicit operator string(Rgb c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

    public static explicit operator Rgb(string s)
    {
        var hex = s.AsSpan();
        hex = hex[0] == '#' ? hex[1..] : hex;
        return new Rgb(
            byte.Parse(hex[..2], NumberStyles.HexNumber),
            byte.Parse(hex[2..4], NumberStyles.HexNumber),
            byte.Parse(hex[4..6], NumberStyles.HexNumber)
        );
    }
}

public static class Sample
{
    public static void Run()
    {
        var color = new Rgb(0x12, 0xAB, 0xFF);
        Console.WriteLine($"Original color: R={color.R} G={color.G} B={color.B}");

        // Serialized as a JSON string, going through the Rgb -> string conversion.
        var json = JsonSerializer.Serialize(color);
        Console.WriteLine($"Serialized color: {json}");

        // Deserialized from a JSON string, going through the string -> Rgb conversion.
        var deColor = JsonSerializer.Deserialize<Rgb>(json);
        Utils.AssertEq(color, deColor);
        Console.WriteLine($"Deserialized color: R={deColor.R} G={deColor.G} B={deColor.B}");
    }
}
