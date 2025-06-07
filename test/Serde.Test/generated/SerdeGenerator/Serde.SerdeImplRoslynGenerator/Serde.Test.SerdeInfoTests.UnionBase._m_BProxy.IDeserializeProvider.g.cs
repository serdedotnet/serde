
namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record UnionBase
    {
        partial class _m_BProxy : Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.UnionBase.B>
        {
            static global::Serde.IDeserialize<Serde.Test.SerdeInfoTests.UnionBase.B> global::Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.UnionBase.B>.Instance { get; }
                = new Serde.Test.SerdeInfoTests.UnionBase._m_BProxy._DeObj();
        }
    }
}
