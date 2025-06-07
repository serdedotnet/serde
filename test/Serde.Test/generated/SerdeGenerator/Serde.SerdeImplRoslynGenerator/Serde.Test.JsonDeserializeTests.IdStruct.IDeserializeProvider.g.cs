
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial struct IdStruct : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.IdStruct>
    {
        static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.IdStruct> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.IdStruct>.Instance { get; }
            = new Serde.Test.JsonDeserializeTests.IdStruct._DeObj();
    }
}
