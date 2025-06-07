
namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record BasicDU
    {
        partial class _m_AProxy : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.BasicDU.A>
        {
            static global::Serde.ISerialize<Serde.Test.JsonSerializerTests.BasicDU.A> global::Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.BasicDU.A>.Instance { get; }
                = new Serde.Test.JsonSerializerTests.BasicDU._m_AProxy._SerObj();
        }
    }
}
