//HintName: OptsWrap.IDeserializeProvider.g.cs
partial record struct OptsWrap : Serde.IDeserializeProvider<System.Runtime.InteropServices.ComTypes.BIND_OPTS>
{
    static global::Serde.IDeserialize<System.Runtime.InteropServices.ComTypes.BIND_OPTS> global::Serde.IDeserializeProvider<System.Runtime.InteropServices.ComTypes.BIND_OPTS>.Instance { get; }
        = new OptsWrap._DeObj();
}
