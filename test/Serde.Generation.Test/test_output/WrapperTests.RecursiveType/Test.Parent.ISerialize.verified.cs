//HintName: Test.Parent.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Test;

partial record Parent : Serde.ISerializeProvider<Test.Parent>
{
    static ISerialize<Test.Parent> ISerializeProvider<Test.Parent>.SerializeInstance
        => _SerObj.Instance;

    sealed partial class _SerObj :Serde.ISerialize<Test.Parent>
    {
        void global::Serde.ISerialize<Test.Parent>.Serialize(Test.Parent value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<Parent>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteValue<Recursive, Test.RecursiveWrap>(_l_info, 0, value.R);
            _l_type.End(_l_info);
        }
        public static readonly _SerObj Instance = new();
        private _SerObj() { }

    }
}
