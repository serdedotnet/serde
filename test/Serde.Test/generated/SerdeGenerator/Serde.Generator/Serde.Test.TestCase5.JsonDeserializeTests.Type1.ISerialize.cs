
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class TestCase5
        {
            partial record Type1 : Serde.ISerialize
            {
                void Serde.ISerialize.Serialize(ISerializer serializer)
                {
                    var type = serializer.SerializeType("Type1", 3);
                    type.SerializeField("field0", new ListWrap.SerializeImpl<int, Int32Wrap>(this.Field0));
                    type.SerializeField("field1", this.Field1);
                    type.SerializeField("field2", this.Field2);
                    type.End();
                }
            }
        }
    }
}