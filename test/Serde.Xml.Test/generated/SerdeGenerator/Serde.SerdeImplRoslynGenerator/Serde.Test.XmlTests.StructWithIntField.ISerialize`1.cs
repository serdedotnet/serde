
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class XmlTests
    {
        partial record StructWithIntField : Serde.ISerialize<Serde.Test.XmlTests.StructWithIntField>
        {
            void ISerialize<Serde.Test.XmlTests.StructWithIntField>.Serialize(Serde.Test.XmlTests.StructWithIntField value, ISerializer serializer)
            {
                var type = serializer.SerializeType("StructWithIntField", 1);
                type.SerializeField<int, Int32Wrap>("X", value.X);
                type.End();
            }
        }
    }
}