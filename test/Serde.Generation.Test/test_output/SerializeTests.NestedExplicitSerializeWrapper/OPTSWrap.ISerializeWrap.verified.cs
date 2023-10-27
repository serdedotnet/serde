//HintName: OPTSWrap.ISerializeWrap.cs

partial record struct OPTSWrap : Serde.ISerializeWrap<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OPTSWrap>
{
    static OPTSWrap Serde.ISerializeWrap<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OPTSWrap>.Create(System.Runtime.InteropServices.ComTypes.BIND_OPTS value) => new(value);
}