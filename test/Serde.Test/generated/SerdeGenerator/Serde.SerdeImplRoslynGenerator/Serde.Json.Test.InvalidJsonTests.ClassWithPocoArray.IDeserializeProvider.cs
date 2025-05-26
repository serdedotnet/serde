
namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithPocoArray : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithPocoArray>
    {
        static global::Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithPocoArray> global::Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithPocoArray>.Instance { get; }
            = new Serde.Json.Test.InvalidJsonTests.ClassWithPocoArray._DeObj();
    }
}
