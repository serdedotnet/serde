//HintName: SimpleRecord.ISerdeProvider.cs
partial record SimpleRecord : Serde.ISerdeProvider<SimpleRecord, SimpleRecord._SerdeObj, SimpleRecord>
{
    static SimpleRecord._SerdeObj global::Serde.ISerdeProvider<SimpleRecord, SimpleRecord._SerdeObj, SimpleRecord>.Instance { get; }
        = new SimpleRecord._SerdeObj();
}
