
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record SkipDeserialize : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.SkipDeserialize>
    {
        static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.SkipDeserialize> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.SkipDeserialize>.Instance { get; }
            = new Serde.Test.JsonDeserializeTests.SkipDeserialize._DeObj();
    }
}
