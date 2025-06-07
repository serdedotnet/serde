
namespace Serde.Test;

partial record AllInOne : Serde.ISerializeProvider<Serde.Test.AllInOne>
{
    static global::Serde.ISerialize<Serde.Test.AllInOne> global::Serde.ISerializeProvider<Serde.Test.AllInOne>.Instance { get; }
        = new Serde.Test.AllInOne._SerObj();
}
