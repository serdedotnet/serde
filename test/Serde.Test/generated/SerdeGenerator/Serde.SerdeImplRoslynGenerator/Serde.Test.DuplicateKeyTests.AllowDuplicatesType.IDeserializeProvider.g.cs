
namespace Serde.Test;

partial class DuplicateKeyTests
{
    partial struct AllowDuplicatesType : Serde.IDeserializeProvider<Serde.Test.DuplicateKeyTests.AllowDuplicatesType>
    {
        static global::Serde.IDeserialize<Serde.Test.DuplicateKeyTests.AllowDuplicatesType> global::Serde.IDeserializeProvider<Serde.Test.DuplicateKeyTests.AllowDuplicatesType>.Instance { get; }
            = new Serde.Test.DuplicateKeyTests.AllowDuplicatesType._DeObj();
    }
}
