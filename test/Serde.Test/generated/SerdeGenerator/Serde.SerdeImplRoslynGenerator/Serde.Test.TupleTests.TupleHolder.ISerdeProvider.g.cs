
namespace Serde.Test;

partial class TupleTests
{
    partial record TupleHolder : Serde.ISerdeProvider<Serde.Test.TupleTests.TupleHolder, Serde.Test.TupleTests.TupleHolder._SerdeObj, Serde.Test.TupleTests.TupleHolder>
    {
        static Serde.Test.TupleTests.TupleHolder._SerdeObj global::Serde.ISerdeProvider<Serde.Test.TupleTests.TupleHolder, Serde.Test.TupleTests.TupleHolder._SerdeObj, Serde.Test.TupleTests.TupleHolder>.Instance { get; }
            = new Serde.Test.TupleTests.TupleHolder._SerdeObj();
    }
}
