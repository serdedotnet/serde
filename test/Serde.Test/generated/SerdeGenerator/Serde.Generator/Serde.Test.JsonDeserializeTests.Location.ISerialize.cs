
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial class Location : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("Location", 9);
                type.SerializeField("id", new Int32Wrap(this.Id));
                type.SerializeField("address1", new StringWrap(this.Address1));
                type.SerializeField("address2", new StringWrap(this.Address2));
                type.SerializeField("city", new StringWrap(this.City));
                type.SerializeField("state", new StringWrap(this.State));
                type.SerializeField("postalCode", new StringWrap(this.PostalCode));
                type.SerializeField("name", new StringWrap(this.Name));
                type.SerializeField("phoneNumber", new StringWrap(this.PhoneNumber));
                type.SerializeField("country", new StringWrap(this.Country));
                type.End();
            }
        }
    }
}