
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record DUWithInitializer : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.DUWithInitializer>
    {
        static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.DUWithInitializer> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.DUWithInitializer>.Instance { get; }
            = new Serde.Test.JsonDeserializeTests.DUWithInitializer._DeObj();
    }
}
