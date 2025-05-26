
namespace Serde.Test;

partial class CustomImplTests
{
    partial record RgbWithFieldMap : Serde.ISerializeProvider<Serde.Test.CustomImplTests.RgbWithFieldMap>
    {
        static global::Serde.ISerialize<Serde.Test.CustomImplTests.RgbWithFieldMap> global::Serde.ISerializeProvider<Serde.Test.CustomImplTests.RgbWithFieldMap>.Instance { get; }
            = new Serde.Test.CustomImplTests.RgbWithFieldMap._SerObj();
    }
}
