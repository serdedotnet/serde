
namespace Serde.Test;

partial class SerdeInfoTests
{
    partial class EmptyInitializedProxy : Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.InitializedTarget>
    {
        static global::Serde.IDeserialize<Serde.Test.SerdeInfoTests.InitializedTarget> global::Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.InitializedTarget>.Instance { get; }
            = new Serde.Test.SerdeInfoTests.EmptyInitializedProxy._DeObj();
    }
}
