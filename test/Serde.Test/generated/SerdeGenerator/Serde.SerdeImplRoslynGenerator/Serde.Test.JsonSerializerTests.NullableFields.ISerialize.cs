
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonSerializerTests
    {
        partial class NullableFields : Serde.ISerialize<Serde.Test.JsonSerializerTests.NullableFields>
        {
            void ISerialize<Serde.Test.JsonSerializerTests.NullableFields>.Serialize(Serde.Test.JsonSerializerTests.NullableFields value, ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<NullableFields>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeFieldIfNotNull<string?, Serde.NullableRefWrap.SerializeImpl<string, global::Serde.StringWrap>>(_l_serdeInfo, 0, value.S);
                type.SerializeField<System.Collections.Generic.Dictionary<string, string?>, Serde.DictWrap.SerializeImpl<string, global::Serde.StringWrap, string?, Serde.NullableRefWrap.SerializeImpl<string, global::Serde.StringWrap>>>(_l_serdeInfo, 1, value.D);
                type.End();
            }
        }
    }
}