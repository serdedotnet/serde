
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial struct ExtraMembers : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.ExtraMembers>
    {
        static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.ExtraMembers> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.ExtraMembers>.Instance { get; }
            = new Serde.Test.JsonDeserializeTests.ExtraMembers._DeObj();
    }
}
