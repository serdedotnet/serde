
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial class MapTest1 : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("MapTest1", 1);
                type.SerializeField("MapField", new DictWrap.SerializeImpl<string, StringWrap, int, Int32Wrap>(this.MapField));
                type.End();
            }
        }
    }
}