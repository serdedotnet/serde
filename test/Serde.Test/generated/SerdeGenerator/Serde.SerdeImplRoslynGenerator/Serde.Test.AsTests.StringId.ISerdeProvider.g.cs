
namespace Serde.Test;

partial class AsTests
{
    partial struct StringId : Serde.ISerdeProvider<Serde.Test.AsTests.StringId, Serde.Test.AsTests.StringId._SerdeObj, Serde.Test.AsTests.StringId>
    {
        static Serde.Test.AsTests.StringId._SerdeObj global::Serde.ISerdeProvider<Serde.Test.AsTests.StringId, Serde.Test.AsTests.StringId._SerdeObj, Serde.Test.AsTests.StringId>.Instance { get; }
            = new Serde.Test.AsTests.StringId._SerdeObj();
    }
}
