//HintName: ComplexRecord.ISerdeProvider.g.cs
partial record ComplexRecord : Serde.ISerdeProvider<ComplexRecord, ComplexRecord._SerdeObj, ComplexRecord>
{
    static ComplexRecord._SerdeObj global::Serde.ISerdeProvider<ComplexRecord, ComplexRecord._SerdeObj, ComplexRecord>.Instance { get; }
        = new ComplexRecord._SerdeObj();
}
