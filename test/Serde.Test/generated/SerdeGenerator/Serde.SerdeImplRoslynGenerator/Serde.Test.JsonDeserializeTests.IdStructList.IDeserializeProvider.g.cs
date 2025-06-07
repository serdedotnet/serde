
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record struct IdStructList : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.IdStructList>
    {
        static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.IdStructList> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.IdStructList>.Instance { get; }
            = new Serde.Test.JsonDeserializeTests.IdStructList._DeObj();
    }
}
