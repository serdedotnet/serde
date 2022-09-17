
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial class DictTest1 : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("DictTest1", 1);
                type.SerializeField("DictField", new DictWrap.SerializeImpl<string, StringWrap, int, Int32Wrap>(this.DictField));
                type.End();
            }
        }
    }
}