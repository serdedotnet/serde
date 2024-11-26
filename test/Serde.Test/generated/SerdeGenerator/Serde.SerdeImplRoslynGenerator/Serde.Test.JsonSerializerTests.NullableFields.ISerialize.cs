
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonSerializerTests
    {
        partial class NullableFields : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.NullableFields>
        {
            static ISerialize<Serde.Test.JsonSerializerTests.NullableFields> ISerializeProvider<Serde.Test.JsonSerializerTests.NullableFields>.SerializeInstance => NullableFieldsSerializeProxy.Instance;

            sealed class NullableFieldsSerializeProxy : Serde.ISerialize<Serde.Test.JsonSerializerTests.NullableFields>
            {
                void ISerialize<Serde.Test.JsonSerializerTests.NullableFields>.Serialize(Serde.Test.JsonSerializerTests.NullableFields value, ISerializer serializer)
                {
                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<NullableFields>();
                    var type = serializer.SerializeType(_l_serdeInfo);
                    type.SerializeFieldIfNotNull<string?, Serde.NullableRefProxy.Serialize<string, global::Serde.StringProxy>>(_l_serdeInfo, 0, value.S);
                    type.SerializeField<System.Collections.Generic.Dictionary<string, string?>, Serde.DictProxy.Serialize<string, string?, global::Serde.StringProxy, Serde.NullableRefProxy.Serialize<string, global::Serde.StringProxy>>>(_l_serdeInfo, 1, value.D);
                    type.End();
                }

                public static readonly NullableFieldsSerializeProxy Instance = new();
                private NullableFieldsSerializeProxy()
                {
                }
            }
        }
    }
}