
namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithIntArray : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithIntArray>
    {
        static global::Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithIntArray> global::Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithIntArray>.Instance { get; }
            = new Serde.Json.Test.InvalidJsonTests.ClassWithIntArray._DeObj();
    }
}
