//HintName: Test.Parent.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial record Parent : Serde.ISerialize<Test.Parent>
    {
        void ISerialize<Test.Parent>.Serialize(Test.Parent value, ISerializer serializer)
        {
            var _l_typeInfo = ParentSerdeTypeInfo.TypeInfo;
            var type = serializer.SerializeType(_l_typeInfo);
            type.SerializeField<Recursive, Test.RecursiveWrap>(_l_typeInfo, 0, this.R);
            type.End();
        }
    }
}