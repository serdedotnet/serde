
using System.IO;
using Serde.Json;
using Xunit;

namespace Serde.Test;

public sealed partial class CustomImplTests
{
    [GenerateSerialize]
    private sealed partial record RgbWithFieldMap : IDeserialize<RgbWithFieldMap>
    {
        public int Red, Green, Blue;

        private static readonly TypeInfo s_fieldMap = TypeInfo.Create(TypeInfo.TypeKind.CustomType, [
            ("red", typeof(RgbWithFieldMap).GetField("Red")!),
            ("green", typeof(RgbWithFieldMap).GetField("Green")!),
            ("blue", typeof(RgbWithFieldMap).GetField("Blue")!)
        ]);

        static RgbWithFieldMap IDeserialize<RgbWithFieldMap>.Deserialize(IDeserializer deserializer)
        {
            var fieldMap = s_fieldMap;
            var deType = deserializer.DeserializeType(fieldMap);
            int red = default;
            int green = default;
            int blue = default;
            int index;
            while ((index = deType.TryReadIndex(fieldMap, out var errorName)) != IDeserializeType.EndOfType)
            {
                switch (index)
                {
                    case 0:
                        red = deType.ReadValue<int, Int32Wrap>(index);
                        break;
                    case 1:
                        green = deType.ReadValue<int, Int32Wrap>(index);
                        break;
                    case 2:
                        blue = deType.ReadValue<int, Int32Wrap>(index);
                        break;
                }
            }

            return new RgbWithFieldMap { Red = red, Green = green, Blue = blue };
        }
    }

    [Fact]
    public void TestLocation()
    {
        var rgb = new RgbWithFieldMap { Red = 255, Green = 128, Blue = 0 };
        var json = JsonSerializer.Serialize(rgb);
        var deserialized = JsonSerializer.Deserialize<RgbWithFieldMap>(json);
        Assert.Equal(rgb.Red, deserialized.Red);
    }
}