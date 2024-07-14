//HintName: S1.ISerialize.cs

#nullable enable
using System;
using Serde;

partial struct S1 : Serde.ISerialize<S1>
{
    void ISerialize<S1>.Serialize(S1 value, ISerializer serializer)
    {
        var _l_serdeInfo = S1SerdeInfo.Instance;
        var type = serializer.SerializeType(_l_serdeInfo);
        type.End();
    }
}