
namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record ContactGen : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.ContactGen>
    {
        static global::Serde.ISerialize<Serde.Test.JsonSerializerTests.ContactGen> global::Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.ContactGen>.Instance { get; }
            = new Serde.Test.JsonSerializerTests.ContactGen._SerObj();
    }
}
