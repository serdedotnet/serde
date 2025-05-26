
namespace Serde.Test;

partial class JsonSerializerTests
{
    partial struct Color : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.Color>
    {
        static global::Serde.ISerialize<Serde.Test.JsonSerializerTests.Color> global::Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.Color>.Instance { get; }
            = new Serde.Test.JsonSerializerTests.Color._SerObj();
    }
}
