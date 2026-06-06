
namespace Serde.Test;

partial class RoundtripTests
{
    partial struct ForeignPointProxy : Serde.ISerdeProvider<Serde.Test.RoundtripTests.ForeignPointProxy, Serde.Test.RoundtripTests.ForeignPointProxy._SerdeObj, Serde.Test.RoundtripTests.ForeignPoint>
    {
        static Serde.Test.RoundtripTests.ForeignPointProxy._SerdeObj global::Serde.ISerdeProvider<Serde.Test.RoundtripTests.ForeignPointProxy, Serde.Test.RoundtripTests.ForeignPointProxy._SerdeObj, Serde.Test.RoundtripTests.ForeignPoint>.Instance { get; }
            = new Serde.Test.RoundtripTests.ForeignPointProxy._SerdeObj();
    }
}
