
namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class SkipClass : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.SkipClass>
    {
        static global::Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.SkipClass> global::Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.SkipClass>.Instance { get; }
            = new Serde.Json.Test.InvalidJsonTests.SkipClass._DeObj();
    }
}
