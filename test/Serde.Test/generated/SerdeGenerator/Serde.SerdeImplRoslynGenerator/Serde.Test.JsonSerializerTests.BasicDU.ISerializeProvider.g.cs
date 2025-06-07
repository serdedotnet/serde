
namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.BasicDU>
    {
        static global::Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU> global::Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.BasicDU>.Instance { get; }
            = new Serde.Test.JsonSerializerTests.BasicDU._SerObj();
    }
}
