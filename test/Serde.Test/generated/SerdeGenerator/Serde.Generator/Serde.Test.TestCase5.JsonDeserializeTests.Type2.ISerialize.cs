
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class TestCase5
        {
            partial record Type2 : Serde.ISerialize
            {
                void Serde.ISerialize.Serialize(ISerializer serializer)
                {
                    var type = serializer.SerializeType("Type2", 1);
                    type.SerializeField("field0", new CharWrap(this.Field0));
                    type.End();
                }
            }
        }
    }
}