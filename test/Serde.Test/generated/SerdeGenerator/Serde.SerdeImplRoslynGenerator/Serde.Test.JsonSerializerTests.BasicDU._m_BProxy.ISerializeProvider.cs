
namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU
    {
        partial class _m_BProxy : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.BasicDU.B>
        {
            static global::Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU.B> global::Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.BasicDU.B>.Instance { get; }
                = new Serde.Test.JsonSerializerTests.BasicDU._m_BProxy._SerObj();
        }
    }
}
