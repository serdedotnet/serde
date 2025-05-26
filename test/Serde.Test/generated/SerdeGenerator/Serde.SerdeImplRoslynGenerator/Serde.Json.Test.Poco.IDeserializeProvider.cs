
namespace Serde.Json.Test;

partial class Poco : Serde.IDeserializeProvider<Serde.Json.Test.Poco>
{
    static global::Serde.IDeserialize<Serde.Json.Test.Poco> global::Serde.IDeserializeProvider<Serde.Json.Test.Poco>.Instance { get; }
        = new Serde.Json.Test.Poco._DeObj();
}
