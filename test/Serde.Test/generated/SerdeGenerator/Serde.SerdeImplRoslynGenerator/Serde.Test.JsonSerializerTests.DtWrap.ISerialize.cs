
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial record DtWrap : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.DtWrap>
    {
        static ISerialize<Serde.Test.JsonSerializerTests.DtWrap> ISerializeProvider<Serde.Test.JsonSerializerTests.DtWrap>.Instance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<Serde.Test.JsonSerializerTests.DtWrap>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.JsonSerializerTests.DtWrap.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.JsonSerializerTests.DtWrap>.Serialize(Serde.Test.JsonSerializerTests.DtWrap value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteDateTime(_l_info, 0, value.Value);
                _l_type.End(_l_info);
            }
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
