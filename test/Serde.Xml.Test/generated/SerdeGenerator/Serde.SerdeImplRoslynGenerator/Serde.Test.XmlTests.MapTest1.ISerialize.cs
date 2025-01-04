
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class XmlTests
{
    partial class MapTest1 : Serde.ISerializeProvider<Serde.Test.XmlTests.MapTest1>
    {
        static ISerialize<Serde.Test.XmlTests.MapTest1> ISerializeProvider<Serde.Test.XmlTests.MapTest1>.SerializeInstance
            => MapTest1SerializeProxy.Instance;

        sealed partial class MapTest1SerializeProxy :Serde.ISerialize<Serde.Test.XmlTests.MapTest1>
        {
            void global::Serde.ISerialize<Serde.Test.XmlTests.MapTest1>.Serialize(Serde.Test.XmlTests.MapTest1 value, global::Serde.ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<MapTest1>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<System.Collections.Generic.Dictionary<string, int>,Serde.DictProxy.Serialize<string,int,global::Serde.StringProxy,global::Serde.Int32Proxy>>(_l_serdeInfo,0,value.MapField);
                type.End();
            }
            public static readonly MapTest1SerializeProxy Instance = new();
            private MapTest1SerializeProxy() { }

        }
    }
}
