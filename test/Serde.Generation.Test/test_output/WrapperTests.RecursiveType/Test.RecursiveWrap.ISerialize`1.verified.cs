//HintName: Test.RecursiveWrap.ISerialize`1.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial record struct RecursiveWrap : Serde.ISerialize<Recursive>
    {
        void ISerialize<Recursive>.Serialize(Recursive value, ISerializer serializer)
        {
            var type = serializer.SerializeType("Recursive", 1);
            type.SerializeFieldIfNotNull<Recursive?, RecursiveWrap>("next", Value.Next);
            type.End();
        }
    }
}