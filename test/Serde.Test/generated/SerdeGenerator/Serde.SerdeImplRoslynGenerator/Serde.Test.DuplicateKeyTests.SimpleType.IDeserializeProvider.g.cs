
namespace Serde.Test;

partial class DuplicateKeyTests
{
    partial struct SimpleType : Serde.IDeserializeProvider<Serde.Test.DuplicateKeyTests.SimpleType>
    {
        static global::Serde.IDeserialize<Serde.Test.DuplicateKeyTests.SimpleType> global::Serde.IDeserializeProvider<Serde.Test.DuplicateKeyTests.SimpleType>.Instance { get; }
            = new Serde.Test.DuplicateKeyTests.SimpleType._DeObj();
    }
}
