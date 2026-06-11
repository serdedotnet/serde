//HintName: TargetProxy.IDeserializeProvider.g.cs
partial struct TargetProxy : Serde.IDeserializeProvider<Target>
{
    static global::Serde.IDeserialize<Target> global::Serde.IDeserializeProvider<Target>.Instance { get; }
        = new TargetProxy._DeObj();
}
