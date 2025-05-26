
namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record RgbProxy : Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.Rgb>
    {
        static global::Serde.IDeserialize<Serde.Test.SerdeInfoTests.Rgb> global::Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.Rgb>.Instance { get; }
            = new Serde.Test.SerdeInfoTests.RgbProxy._DeObj();
    }
}
