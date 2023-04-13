//HintName: S1.ISerialize.cs

#nullable enable
using Serde;

partial struct S1 : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("S1", 1);
        type.End();
    }
}