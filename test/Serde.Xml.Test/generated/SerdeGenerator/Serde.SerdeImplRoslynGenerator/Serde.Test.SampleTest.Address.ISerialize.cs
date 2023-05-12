
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class SampleTest
    {
        partial record Address : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("Address", 5);
                type.SerializeField("Name"u8, new StringWrap(this.Name), new System.Attribute[] { new System.Xml.Serialization.XmlAttributeAttribute() { }, new Serde.SerdeMemberOptions() { ProvideAttributes = true } });
                type.SerializeField("Line1"u8, new StringWrap(this.Line1));
                type.SerializeFieldIfNotNull("City"u8, new NullableRefWrap.SerializeImpl<string, StringWrap>(this.City), this.City);
                type.SerializeField("State"u8, new StringWrap(this.State));
                type.SerializeField("Zip"u8, new StringWrap(this.Zip));
                type.End();
            }
        }
    }
}