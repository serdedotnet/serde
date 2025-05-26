
namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithIntList : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithIntList>
    {
        static global::Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithIntList> global::Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithIntList>.Instance { get; }
            = new Serde.Json.Test.InvalidJsonTests.ClassWithIntList._DeObj();
    }
}
