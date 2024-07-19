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
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Parent>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<Recursive, Test.RecursiveWrap>(_l_serdeInfo, 0, value.R);
            type.End();
        }
    }
}