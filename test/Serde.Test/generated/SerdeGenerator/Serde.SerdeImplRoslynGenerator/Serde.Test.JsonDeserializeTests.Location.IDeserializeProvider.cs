
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record Location : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.Location>
    {
        static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.Location> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.Location>.Instance { get; }
            = new Serde.Test.JsonDeserializeTests.Location._DeObj();
    }
}
