
// Define a custom Color type
using System;
using System.Threading.Tasks;
using Serde;
using Serde.Json;

namespace CustomSerdeSample;

// Attach a custom serializer to the Color type
[GenerateSerde(With = typeof(ColorSerdeObj))]
partial record Color(int R, int G, int B);

// Create a serde object for the Color type that serializes as a hex string
class ColorSerdeObj : ISerde<Color>
{
    // Color is serialized as a hex string, so it looks just like a string in the serialized form.
    public ISerdeInfo SerdeInfo => StringProxy.SerdeInfo;

    public void Serialize(Color color, ISerializer serializer)
    {
        var hex = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        serializer.WriteString(hex);
    }

    public async ValueTask<Color> Deserialize(IDeserializer deserializer)
    {
        var hex = await deserializer.ReadString();
        if (hex.Length != 7 || hex[0] != '#')
            throw new FormatException("Invalid hex color format");

        return new Color(
            Convert.ToInt32(hex[1..3], 16),
            Convert.ToInt32(hex[3..5], 16),
            Convert.ToInt32(hex[5..7], 16));
    }
}

static class CustomSerializationSample
{
    public static void Run()
    {
        var color = new Color(255, 0, 0);
        Console.WriteLine($"Original color: {color}");

        // Serialize the color to a JSON string
        var json = JsonSerializer.Serialize(color);
        Console.WriteLine($"Serialized color: {json}");

        // Deserialize the JSON string back to a Color object
        var deserializedColor = JsonSerializer.Deserialize<Color>(json);
        Utils.AssertEq(color, deserializedColor);
        Console.WriteLine($"Deserialized color: {deserializedColor}");
    }
}