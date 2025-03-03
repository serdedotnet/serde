
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class CustomImplTests
{
    partial record RgbWithFieldMap : Serde.ISerializeProvider<Serde.Test.CustomImplTests.RgbWithFieldMap>
    {
        static ISerialize<Serde.Test.CustomImplTests.RgbWithFieldMap> ISerializeProvider<Serde.Test.CustomImplTests.RgbWithFieldMap>.SerializeInstance
            => RgbWithFieldMapSerializeProxy.Instance;

        sealed partial class RgbWithFieldMapSerializeProxy :Serde.ISerialize<Serde.Test.CustomImplTests.RgbWithFieldMap>
        {
            void global::Serde.ISerialize<Serde.Test.CustomImplTests.RgbWithFieldMap>.Serialize(Serde.Test.CustomImplTests.RgbWithFieldMap value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo<RgbWithFieldMap>();
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteI32(_l_info, 0, value.Red);
                _l_type.WriteI32(_l_info, 1, value.Green);
                _l_type.WriteI32(_l_info, 2, value.Blue);
                _l_type.End(_l_info);
            }
            public static readonly RgbWithFieldMapSerializeProxy Instance = new();
            private RgbWithFieldMapSerializeProxy() { }

        }
    }
}
