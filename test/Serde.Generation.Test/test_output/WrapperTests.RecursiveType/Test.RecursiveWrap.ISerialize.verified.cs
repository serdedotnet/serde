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
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<RecursiveWrap>();
            var type = serializer.SerializeType(_l_serdeInfo);
            type.SerializeFieldIfNotNull<Recursive?, RecursiveWrap>(_l_serdeInfo, 0, value.Next);
            type.End();
        }
    }
}