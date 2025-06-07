
namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record DtWrap : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.DtWrap>
    {
        static global::Serde.ISerialize<Serde.Test.JsonSerializerTests.DtWrap> global::Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.DtWrap>.Instance { get; }
            = new Serde.Test.JsonSerializerTests.DtWrap._SerObj();
    }
}
