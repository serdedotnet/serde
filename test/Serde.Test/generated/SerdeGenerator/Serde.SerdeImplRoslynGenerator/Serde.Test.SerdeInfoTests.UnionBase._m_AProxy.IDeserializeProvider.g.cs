
namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record UnionBase
    {
        partial class _m_AProxy : Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.UnionBase.A>
        {
            static global::Serde.IDeserialize<Serde.Test.SerdeInfoTests.UnionBase.A> global::Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.UnionBase.A>.Instance { get; }
                = new Serde.Test.SerdeInfoTests.UnionBase._m_AProxy._DeObj();
        }
    }
}
