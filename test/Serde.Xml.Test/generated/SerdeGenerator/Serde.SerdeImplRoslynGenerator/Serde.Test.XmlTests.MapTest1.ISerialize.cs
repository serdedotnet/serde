
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
                var _l_serdeInfo = MapTest1SerdeInfo.Instance;
                var type = serializer.SerializeType(_l_serdeInfo);
                type.SerializeField<System.Collections.Generic.Dictionary<string, int>, Serde.DictWrap.SerializeImpl<string, StringWrap, int, Int32Wrap>>(_l_serdeInfo, 0, value.MapField);
                type.End();
            }
        }
    }
}