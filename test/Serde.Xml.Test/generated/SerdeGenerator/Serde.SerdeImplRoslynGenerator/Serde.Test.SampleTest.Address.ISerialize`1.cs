
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class SampleTest
    {
        partial record Address : Serde.ISerialize<Serde.Test.SampleTest.Address>
        {
            void ISerialize<Serde.Test.SampleTest.Address>.Serialize(Serde.Test.SampleTest.Address value, ISerializer serializer)
            {
                var type = serializer.SerializeType("Address", 5);
                type.SerializeField<string, StringWrap>("Name", value.Name, new System.Attribute[] { new System.Xml.Serialization.XmlAttributeAttribute() { }, new Serde.SerdeMemberOptions() { ProvideAttributes = true } });
                type.SerializeField<string, StringWrap>("Line1", value.Line1);
                type.SerializeFieldIfNotNull<string?, Serde.NullableRefWrap.SerializeImpl<string, StringWrap>>("City", value.City);
                type.SerializeField<string, StringWrap>("State", value.State);
                type.SerializeField<string, StringWrap>("Zip", value.Zip);
                type.End();
            }
        }
    }
}