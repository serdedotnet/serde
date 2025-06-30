
using System.IO;
using System.Threading.Tasks;
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
                ("red", I32Proxy.SerdeInfo, typeof(RgbWithFieldMap).GetField("Red")),
                ("green", I32Proxy.SerdeInfo, typeof(RgbWithFieldMap).GetField("Green")),
                ("blue", I32Proxy.SerdeInfo, typeof(RgbWithFieldMap).GetField("Blue"))
            ]);

        static IDeserialize<RgbWithFieldMap> IDeserializeProvider<RgbWithFieldMap>.Instance { get; }
            = new RgbWithFieldMapDeserialize();
    }

    private sealed class RgbWithFieldMapDeserialize : IDeserialize<RgbWithFieldMap>
    {
        ISerdeInfo ISerdeInfoProvider.SerdeInfo => RgbWithFieldMap.SerdeInfo;
        async Task<RgbWithFieldMap> IDeserialize<RgbWithFieldMap>.Deserialize(IDeserializer deserializer)
        {
            var fieldMap = RgbWithFieldMap.SerdeInfo;
            var deType = deserializer.ReadType(fieldMap);
            int red = default;
            int green = default;
            int blue = default;
            while (true)
            {
                var index = await deType.TryReadIndex(fieldMap);
                if (index == ITypeDeserializer.EndOfType)
                {
                    break;
                }
                switch (index)
                {
                    case 0:
                        red = await deType.ReadI32(fieldMap, index);
                        break;
                    case 1:
                        green = await deType.ReadI32(fieldMap, index);
                        break;
                    case 2:
                        blue = await deType.ReadI32(fieldMap, index);
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