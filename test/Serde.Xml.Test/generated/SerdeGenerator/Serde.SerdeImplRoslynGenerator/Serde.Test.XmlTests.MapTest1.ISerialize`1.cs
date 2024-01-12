
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
                var type = serializer.SerializeType("MapTest1", 1);
                type.SerializeField<System.Collections.Generic.Dictionary<string, int>, Serde.DictWrap.SerializeImpl<string, StringWrap, int, Int32Wrap>>("MapField", value.MapField);
                type.End();
            }
        }
    }
}