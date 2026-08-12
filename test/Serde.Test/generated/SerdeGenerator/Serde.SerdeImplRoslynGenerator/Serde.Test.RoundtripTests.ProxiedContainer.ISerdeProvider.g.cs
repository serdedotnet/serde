
namespace Serde.Test;

partial class RoundtripTests
{
    partial record ProxiedContainer : Serde.ISerdeProvider<Serde.Test.RoundtripTests.ProxiedContainer, Serde.Test.RoundtripTests.ProxiedContainer._SerdeObj, Serde.Test.RoundtripTests.ProxiedContainer>
    {
        static Serde.Test.RoundtripTests.ProxiedContainer._SerdeObj global::Serde.ISerdeProvider<Serde.Test.RoundtripTests.ProxiedContainer, Serde.Test.RoundtripTests.ProxiedContainer._SerdeObj, Serde.Test.RoundtripTests.ProxiedContainer>.Instance { get; }
            = new Serde.Test.RoundtripTests.ProxiedContainer._SerdeObj();
    }
}
