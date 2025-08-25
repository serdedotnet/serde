
namespace Serde.Test;

partial class DuplicateKeyTests
{
    partial struct SimpleTypeAllowDuplicates : Serde.IDeserializeProvider<Serde.Test.DuplicateKeyTests.SimpleTypeAllowDuplicates>
    {
        static global::Serde.IDeserialize<Serde.Test.DuplicateKeyTests.SimpleTypeAllowDuplicates> global::Serde.IDeserializeProvider<Serde.Test.DuplicateKeyTests.SimpleTypeAllowDuplicates>.Instance { get; }
            = new Serde.Test.DuplicateKeyTests.SimpleTypeAllowDuplicates._DeObj();
    }
}
