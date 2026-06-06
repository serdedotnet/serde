
namespace Serde.Test;

partial class AsTests
{
    partial record struct SerOnlyId : Serde.ISerializeProvider<Serde.Test.AsTests.SerOnlyId>
    {
        static global::Serde.ISerialize<Serde.Test.AsTests.SerOnlyId> global::Serde.ISerializeProvider<Serde.Test.AsTests.SerOnlyId>.Instance { get; }
            = new Serde.Test.AsTests.SerOnlyId._SerObj();
    }
}
