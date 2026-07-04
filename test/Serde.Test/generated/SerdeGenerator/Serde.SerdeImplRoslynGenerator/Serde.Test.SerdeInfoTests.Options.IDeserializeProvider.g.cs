
namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record Options : Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.Options>
    {
        static global::Serde.IDeserialize<Serde.Test.SerdeInfoTests.Options> global::Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.Options>.Instance { get; }
            = new Serde.Test.SerdeInfoTests.Options._DeObj();
    }
}
