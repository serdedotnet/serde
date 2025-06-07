//HintName: Wrap.IDeserializeProvider.g.cs
partial record struct Wrap : Serde.IDeserializeProvider<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
{
    static global::Serde.IDeserialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS> global::Serde.IDeserializeProvider<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.Instance { get; }
        = new Wrap._DeObj();
}
