
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class JsonSerializerTests
{
    partial class NullableFields : Serde.ISerializeProvider<Serde.Test.JsonSerializerTests.NullableFields>
    {
        static ISerialize<Serde.Test.JsonSerializerTests.NullableFields> ISerializeProvider<Serde.Test.JsonSerializerTests.NullableFields>.SerializeInstance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<Serde.Test.JsonSerializerTests.NullableFields>
        {
            void global::Serde.ISerialize<Serde.Test.JsonSerializerTests.NullableFields>.Serialize(Serde.Test.JsonSerializerTests.NullableFields value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<NullableFields>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteStringIfNotNull(_l_info, 0, value.S);
                _l_type.WriteValue<System.Collections.Generic.Dictionary<string, string?>, Serde.DictProxy.Ser<string, string?, global::Serde.StringProxy, Serde.NullableRefProxy.Ser<string, global::Serde.StringProxy>>>(_l_info, 1, value.D);
                _l_type.End(_l_info);
            }
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
