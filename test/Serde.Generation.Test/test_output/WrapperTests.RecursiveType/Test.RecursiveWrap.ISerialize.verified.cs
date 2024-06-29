//HintName: Test.RecursiveWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial record struct RecursiveWrap : Serde.ISerialize<Recursive>
    {
        void ISerialize<Recursive>.Serialize(Recursive value, ISerializer serializer)
        {
            var _l_typeInfo = RecursiveSerdeTypeInfo.TypeInfo;
            var type = serializer.SerializeType(_l_typeInfo);
            type.SerializeFieldIfNotNull<Recursive?, RecursiveWrap>(_l_typeInfo, 0, Value.Next);
            type.End();
        }
    }
}