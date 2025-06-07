
namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial record NoComma : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.NoComma>
    {
        static global::Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.NoComma> global::Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.NoComma>.Instance { get; }
            = new Serde.Json.Test.InvalidJsonTests.NoComma._DeObj();
    }
}
