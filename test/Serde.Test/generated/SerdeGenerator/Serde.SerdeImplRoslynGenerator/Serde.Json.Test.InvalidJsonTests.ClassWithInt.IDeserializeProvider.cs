
namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithInt : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithInt>
    {
        static global::Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithInt> global::Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithInt>.Instance { get; }
            = new Serde.Json.Test.InvalidJsonTests.ClassWithInt._DeObj();
    }
}
