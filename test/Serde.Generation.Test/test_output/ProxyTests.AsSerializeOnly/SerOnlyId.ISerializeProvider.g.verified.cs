//HintName: SerOnlyId.ISerializeProvider.g.cs
partial struct SerOnlyId : Serde.ISerializeProvider<SerOnlyId>
{
    static global::Serde.ISerialize<SerOnlyId> global::Serde.ISerializeProvider<SerOnlyId>.Instance { get; }
        = new SerOnlyId._SerObj();
}
