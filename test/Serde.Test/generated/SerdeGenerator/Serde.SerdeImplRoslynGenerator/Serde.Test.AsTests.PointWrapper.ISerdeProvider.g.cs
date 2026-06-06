
namespace Serde.Test;

partial class AsTests
{
    partial struct PointWrapper : Serde.ISerdeProvider<Serde.Test.AsTests.PointWrapper, Serde.Test.AsTests.PointWrapper._SerdeObj, Serde.Test.AsTests.PointWrapper>
    {
        static Serde.Test.AsTests.PointWrapper._SerdeObj global::Serde.ISerdeProvider<Serde.Test.AsTests.PointWrapper, Serde.Test.AsTests.PointWrapper._SerdeObj, Serde.Test.AsTests.PointWrapper>.Instance { get; }
            = new Serde.Test.AsTests.PointWrapper._SerdeObj();
    }
}
