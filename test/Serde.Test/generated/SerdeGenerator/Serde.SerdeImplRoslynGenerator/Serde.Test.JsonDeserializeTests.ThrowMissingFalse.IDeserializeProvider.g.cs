
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record ThrowMissingFalse : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.ThrowMissingFalse>
    {
        static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ThrowMissingFalse> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.ThrowMissingFalse>.Instance { get; }
            = new Serde.Test.JsonDeserializeTests.ThrowMissingFalse._DeObj();
    }
}
