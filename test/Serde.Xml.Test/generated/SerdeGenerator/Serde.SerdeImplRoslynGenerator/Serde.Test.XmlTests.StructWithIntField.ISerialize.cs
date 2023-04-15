
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial record StructWithIntField : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("StructWithIntField", 1);
                type.SerializeField("X", new Int32Wrap(this.X));
                type.End();
            }
        }
    }
}