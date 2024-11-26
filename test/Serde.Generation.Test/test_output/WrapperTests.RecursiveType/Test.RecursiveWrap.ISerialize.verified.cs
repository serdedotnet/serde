//HintName: Test.RecursiveWrap.ISerialize.cs

#nullable enable
using System;
using Serde;

namespace Test
{
    partial class RecursiveWrap : Serde.ISerializeProvider<Recursive>
    {
        static ISerialize<Recursive> ISerializeProvider<Recursive>.SerializeInstance => RecursiveWrapSerializeProxy.Instance;

        sealed class RecursiveWrapSerializeProxy : Serde.ISerialize<Recursive>
        {
            void ISerialize<Recursive>.Serialize(Recursive value, ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<RecursiveWrap>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeFieldIfNotNull<Recursive?, Test.RecursiveWrap>(_l_serdeInfo, 0, value.Next);
                type.End();
            }

            public static readonly RecursiveWrapSerializeProxy Instance = new();
            private RecursiveWrapSerializeProxy()
            {
            }
        }
    }
}