
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial class MapTest1 : Serde.ISerialize<Serde.Test.XmlTests.MapTest1>
        {
            void ISerialize<Serde.Test.XmlTests.MapTest1>.Serialize(Serde.Test.XmlTests.MapTest1 value, ISerializer serializer)
            {
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<MapTest1>();
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<System.Collections.Generic.Dictionary<string, int>, Serde.DictWrap.SerializeImpl<string, global::Serde.StringWrap, int, global::Serde.Int32Wrap>>(_l_serdeInfo, 0, value.MapField);
                type.End();
            }
        }
    }
}