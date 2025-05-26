
namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record EmptyRecord : Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.EmptyRecord>
    {
        static global::Serde.IDeserialize<Serde.Test.SerdeInfoTests.EmptyRecord> global::Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.EmptyRecord>.Instance { get; }
            = new Serde.Test.SerdeInfoTests.EmptyRecord._DeObj();
    }
}
