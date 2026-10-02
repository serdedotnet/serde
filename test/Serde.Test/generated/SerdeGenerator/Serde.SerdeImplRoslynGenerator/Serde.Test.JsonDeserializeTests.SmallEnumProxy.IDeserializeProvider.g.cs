
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial class SmallEnumProxy : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.SmallEnum>
    {
        static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SmallEnum> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.SmallEnum>.Instance { get; }
            = new Serde.Test.JsonDeserializeTests.SmallEnumProxy();
    }
}
