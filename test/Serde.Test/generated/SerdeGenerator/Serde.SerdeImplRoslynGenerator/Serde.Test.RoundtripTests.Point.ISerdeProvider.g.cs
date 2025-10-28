
namespace Serde.Test;

partial class RoundtripTests
{
    partial record Point : Serde.ISerdeProvider<Serde.Test.RoundtripTests.Point, Serde.Test.RoundtripTests.Point._SerdeObj, Serde.Test.RoundtripTests.Point>
    {
        static Serde.Test.RoundtripTests.Point._SerdeObj global::Serde.ISerdeProvider<Serde.Test.RoundtripTests.Point, Serde.Test.RoundtripTests.Point._SerdeObj, Serde.Test.RoundtripTests.Point>.Instance { get; }
            = new Serde.Test.RoundtripTests.Point._SerdeObj();
    }
}
