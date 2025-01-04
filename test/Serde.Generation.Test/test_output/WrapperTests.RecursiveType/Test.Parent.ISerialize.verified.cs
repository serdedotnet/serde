//HintName: Test.Parent.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Test;

partial record Parent : Serde.ISerializeProvider<Test.Parent>
{
    static ISerialize<Test.Parent> ISerializeProvider<Test.Parent>.SerializeInstance
        => ParentSerializeProxy.Instance;

    sealed partial class ParentSerializeProxy :Serde.ISerialize<Test.Parent>
    {
        void global::Serde.ISerialize<Test.Parent>.Serialize(Test.Parent value, global::Serde.ISerializer serializer)
        {
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Parent>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeField<Recursive,Test.RecursiveWrap>(_l_serdeInfo,0,value.R);
            type.End();
        }
        public static readonly ParentSerializeProxy Instance = new();
        private ParentSerializeProxy() { }

    }
}
