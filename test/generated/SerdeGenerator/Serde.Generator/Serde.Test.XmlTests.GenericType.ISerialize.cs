
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial class GenericType : Serde.ISerialize
        {
            static string ITypeName.TypeName => "GenericType";
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("GenericType", 1);
                type.SerializeField("Field", new Int32Wrap(this.Field));
                type.End();
            }
        }
    }
}