
namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithDictionaryOfPoco : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco>
    {
        static global::Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco> global::Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco>.Instance { get; }
            = new Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco._DeObj();
    }
}
