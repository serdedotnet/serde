
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct DenyUnknown : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.DenyUnknown>
    {
        static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.DenyUnknown> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.DenyUnknown>.Instance { get; }
            = new Serde.Test.JsonDeserializeTests.DenyUnknown._DeObj();
    }
}
