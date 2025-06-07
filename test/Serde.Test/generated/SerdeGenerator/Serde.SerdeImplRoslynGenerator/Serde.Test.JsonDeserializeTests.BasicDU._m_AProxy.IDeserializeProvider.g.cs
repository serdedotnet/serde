
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU
    {
        partial class _m_AProxy : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.BasicDU.A>
        {
            static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU.A> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.BasicDU.A>.Instance { get; }
                = new Serde.Test.JsonDeserializeTests.BasicDU._m_AProxy._DeObj();
        }
    }
}
