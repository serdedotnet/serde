
namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BigData : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.BigData>
    {
        static global::Serde.ISerialize<Serde.Test.JsonSerializerTests.BigData> global::Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.BigData>.Instance { get; }
            = new Serde.Test.JsonSerializerTests.BigData._SerObj();
    }
}
