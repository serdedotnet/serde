//HintName: SetToNull.IDeserializeProvider.g.cs
partial record struct SetToNull : Serde.IDeserializeProvider<SetToNull>
{
    static global::Serde.IDeserialize<SetToNull> global::Serde.IDeserializeProvider<SetToNull>.Instance { get; }
        = new SetToNull._DeObj();
}
