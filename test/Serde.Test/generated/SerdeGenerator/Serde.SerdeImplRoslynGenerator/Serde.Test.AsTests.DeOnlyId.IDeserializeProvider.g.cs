
namespace Serde.Test;

partial class AsTests
{
    partial record struct DeOnlyId : Serde.IDeserializeProvider<Serde.Test.AsTests.DeOnlyId>
    {
        static global::Serde.IDeserialize<Serde.Test.AsTests.DeOnlyId> global::Serde.IDeserializeProvider<Serde.Test.AsTests.DeOnlyId>.Instance { get; }
            = new Serde.Test.AsTests.DeOnlyId._DeObj();
    }
}
