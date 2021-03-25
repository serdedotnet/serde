
namespace Serde.Test
{
    partial class Rgb
    {
        public byte Red;
        public byte Green;
        public byte Blue;
    }

    partial class Rgb : ISerialize
    {
        public void Serialize<TSerializer, TSerializeStruct>(TSerializer serializer)
            where TSerializer : ISerializer<TSerializeStruct>
            where TSerializeStruct : ISerializeStruct
        {
            var rgb = serializer.SerializeStruct("Rgb", 3);
            rgb.SerializeField("Red", Red);
            rgb.SerializeField("Green", Green);
            rgb.SerializeField("Blue", Blue);
            rgb.End();
        }
    }
}