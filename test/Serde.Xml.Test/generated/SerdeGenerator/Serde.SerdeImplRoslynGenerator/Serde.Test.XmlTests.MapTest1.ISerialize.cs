
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class XmlTests
{
    partial class MapTest1 : Serde.ISerializeProvider<Serde.Test.XmlTests.MapTest1>
    {
        static ISerialize<Serde.Test.XmlTests.MapTest1> ISerializeProvider<Serde.Test.XmlTests.MapTest1>.Instance
            => _SerObj.Instance;

        sealed partial class _SerObj :Serde.ISerialize<Serde.Test.XmlTests.MapTest1>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.XmlTests.MapTest1.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.XmlTests.MapTest1>.Serialize(Serde.Test.XmlTests.MapTest1 value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info);
                _l_type.WriteValue<System.Collections.Generic.Dictionary<string, int>, Serde.DictProxy.Ser<string, int, global::Serde.StringProxy, global::Serde.I32Proxy>>(_l_info, 0, value.MapField);
                _l_type.End(_l_info);
            }
            public static readonly _SerObj Instance = new();
            private _SerObj() { }

        }
    }
}
