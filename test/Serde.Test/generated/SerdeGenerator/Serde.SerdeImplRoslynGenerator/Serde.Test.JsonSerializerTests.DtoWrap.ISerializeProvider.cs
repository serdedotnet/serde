
namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record DtoWrap : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.DtoWrap>
    {
        static global::Serde.ISerialize<Serde.Test.JsonSerializerTests.DtoWrap> global::Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.DtoWrap>.Instance { get; }
            = new Serde.Test.JsonSerializerTests.DtoWrap._SerObj();
    }
}
