
namespace Serde.Test;

partial class AsTests
{
    partial record Point : Serde.ISerdeProvider<Serde.Test.AsTests.Point, Serde.Test.AsTests.Point._SerdeObj, Serde.Test.AsTests.Point>
    {
        static Serde.Test.AsTests.Point._SerdeObj global::Serde.ISerdeProvider<Serde.Test.AsTests.Point, Serde.Test.AsTests.Point._SerdeObj, Serde.Test.AsTests.Point>.Instance { get; }
            = new Serde.Test.AsTests.Point._SerdeObj();
    }
}
