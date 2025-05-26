
namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithPoco : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithPoco>
    {
        static global::Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithPoco> global::Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithPoco>.Instance { get; }
            = new Serde.Json.Test.InvalidJsonTests.ClassWithPoco._DeObj();
    }
}
