
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct ThrowMissing : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.ThrowMissing>
    {
        static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ThrowMissing> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.ThrowMissing>.Instance { get; }
            = new Serde.Test.JsonDeserializeTests.ThrowMissing._DeObj();
    }
}
