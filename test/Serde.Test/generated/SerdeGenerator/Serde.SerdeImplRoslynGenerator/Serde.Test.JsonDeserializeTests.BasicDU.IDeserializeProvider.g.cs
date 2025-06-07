
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.BasicDU>
    {
        static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.BasicDU>.Instance { get; }
            = new Serde.Test.JsonDeserializeTests.BasicDU._DeObj();
    }
}
