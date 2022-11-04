
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class TestCase5
        {
            partial record Type0 : Serde.ISerialize
            {
                void Serde.ISerialize.Serialize(ISerializer serializer)
                {
                    var type = serializer.SerializeType("Type0", 1);
                    type.SerializeField("field0", this.Field0);
                    type.End();
                }
            }
        }
    }
}