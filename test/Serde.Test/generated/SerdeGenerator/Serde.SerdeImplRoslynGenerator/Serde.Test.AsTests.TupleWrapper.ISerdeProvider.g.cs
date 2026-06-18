
namespace Serde.Test;

partial class AsTests
{
    partial record struct TupleWrapper : Serde.ISerdeProvider<Serde.Test.AsTests.TupleWrapper, Serde.Test.AsTests.TupleWrapper._SerdeObj, Serde.Test.AsTests.TupleWrapper>
    {
        static Serde.Test.AsTests.TupleWrapper._SerdeObj global::Serde.ISerdeProvider<Serde.Test.AsTests.TupleWrapper, Serde.Test.AsTests.TupleWrapper._SerdeObj, Serde.Test.AsTests.TupleWrapper>.Instance { get; }
            = new Serde.Test.AsTests.TupleWrapper._SerdeObj();
    }
}
