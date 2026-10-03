
namespace Serde.Test;

partial class JsonDeserializeTests
{
    partial record DUWithInitializer
    {
        partial class _m_AProxy : Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.DUWithInitializer.A>
        {
            static global::Serde.IDeserialize<Serde.Test.JsonDeserializeTests.DUWithInitializer.A> global::Serde.IDeserializeProvider<Serde.Test.JsonDeserializeTests.DUWithInitializer.A>.Instance { get; }
                = new Serde.Test.JsonDeserializeTests.DUWithInitializer._m_AProxy._DeObj();
        }
    }
}
