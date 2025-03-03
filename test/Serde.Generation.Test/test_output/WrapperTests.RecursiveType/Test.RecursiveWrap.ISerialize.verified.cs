//HintName: Test.RecursiveWrap.ISerialize.cs

#nullable enable

using System;
using Serde;

namespace Test;

partial class RecursiveWrap : Serde.ISerializeProvider<Recursive>
{
    static ISerialize<Recursive> ISerializeProvider<Recursive>.SerializeInstance
        => RecursiveWrapSerializeProxy.Instance;

    sealed partial class RecursiveWrapSerializeProxy :Serde.ISerialize<Recursive>
    {
        void global::Serde.ISerialize<Recursive>.Serialize(Recursive value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo<RecursiveWrap>();
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteFieldIfNotNull<Recursive?, Test.RecursiveWrap>(_l_info, 0, value.Next);
            _l_type.End(_l_info);
        }
        public static readonly RecursiveWrapSerializeProxy Instance = new();
        private RecursiveWrapSerializeProxy() { }

    }
}
