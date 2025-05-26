
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct SetToNull : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.SetToNull>
    {
        static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SetToNull> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.SetToNull>.Instance { get; }
            = new Serde.Test.JsonDeserializeTests.SetToNull._DeObj();
    }
}
