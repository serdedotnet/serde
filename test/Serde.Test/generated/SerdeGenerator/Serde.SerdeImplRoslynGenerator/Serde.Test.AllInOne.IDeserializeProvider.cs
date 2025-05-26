
namespace Serde.Test;

partial record AllInOne : Serde.IDeserializeProvider<Serde.Test.AllInOne>
{
    static global::Serde.IDeserialize<Serde.Test.AllInOne> global::Serde.IDeserializeProvider<Serde.Test.AllInOne>.Instance { get; }
        = new Serde.Test.AllInOne._DeObj();
}
