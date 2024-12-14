
using System.IO;
using Serde.Json;
using Xunit;

namespace Serde.Test;

public sealed partial class CustomImplTests
{
    [GenerateSerialize]
    private sealed partial record RgbWithFieldMap : IDeserializeProvider<RgbWithFieldMap>
    {
        public int Red, Green, Blue;

        public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "RgbWithFieldMap",
            typeof(RgbWithFieldMap).GetCustomAttributesData(),
            [
                ("red", Int32Proxy.SerdeInfo, typeof(RgbWithFieldMap).GetField("Red")!),
                ("green", Int32Proxy.SerdeInfo, typeof(RgbWithFieldMap).GetField("Green")!),
                ("blue", Int32Proxy.SerdeInfo, typeof(RgbWithFieldMap).GetField("Blue")!)
            ]);

        static IDeserialize<RgbWithFieldMap> IDeserializeProvider<RgbWithFieldMap>.DeserializeInstance { get; }
            = new RgbWithFieldMapDeserialize();
    }

    private sealed class RgbWithFieldMapDeserialize : IDeserialize<RgbWithFieldMap>
    {
        RgbWithFieldMap IDeserialize<RgbWithFieldMap>.Deserialize(IDeserializer deserializer)
        {
            var fieldMap = RgbWithFieldMap.SerdeInfo;
            var deType = deserializer.ReadType(fieldMap);
            int red = default;
            int green = default;
            int blue = default;
            int index;
            while ((index = deType.TryReadIndex(fieldMap, out var errorName)) != IDeserializeType.EndOfType)
            {
                switch (index)
                {
                    case 0:
                        red = deType.ReadI32(index);
                        break;
                    case 1:
                        green = deType.ReadI32(index);
                        break;
                    case 2:
                        blue = deType.ReadI32(index);
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