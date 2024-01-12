//HintName: Test.Parent.ISerialize`1.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial record Parent : Serde.ISerialize<Test.Parent>
    {
        void ISerialize<Test.Parent>.Serialize(Test.Parent value, ISerializer serializer)
        {
            var type = serializer.SerializeType("Parent", 1);
            type.SerializeField<Recursive, Test.RecursiveWrap>("r", value.R);
            type.End();
        }
    }
}