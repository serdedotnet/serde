
namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record UnionBase : Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.UnionBase>
    {
        static global::Serde.IDeserialize<Serde.Test.SerdeInfoTests.UnionBase> global::Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.UnionBase>.Instance { get; }
            = new Serde.Test.SerdeInfoTests.UnionBase._DeObj();
    }
}
