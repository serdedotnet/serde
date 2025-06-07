
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record BasicDU
    {
        partial class _m_BProxy : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.BasicDU.B>
        {
            static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.BasicDU.B> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.BasicDU.B>.Instance { get; }
                = new Serde.Test.JsonDeserializeTests.BasicDU._m_BProxy._DeObj();
        }
    }
}
