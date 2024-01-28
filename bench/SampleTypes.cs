
#nullable disable

using Serde;

namespace Benchmarks
{
    // the view models come from a real world app called "AllReady"
    [GenerateSerialize, GenerateDeserialize]
    public partial class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    [GenerateSerialize, GenerateDeserialize]
    public partial record Location
    {
        public int Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
    }

    public partial record LocationWrap : IDeserialize<Location>
    {
        [GenerateDeserialize]
        private enum FieldNames : byte
        {
            Id = 1,
            Address1 = 2,
            Address2 = 3,
            City = 4,
            State = 5,
            PostalCode = 6,
            Name = 7,
            PhoneNumber = 8,
            Country = 9,
        }

        public static Location Deserialize<D>(ref D deserializer) where D : IDeserializer
        {
            return Deserialize<Location, D>(ref deserializer);
        }
        private sealed class LocationVisitor : IDeserializeVisitor<Location>
        {
            public string ExpectedTypeName => nameof(Location);

            Location IDeserializeVisitor<Location>.VisitDictionary<D>(ref D d)
            {
                int _l_id = default!;
                string _l_address1 = default!;
                string _l_address2 = default!;
                string _l_city = default!;
                string _l_state = default!;
                string _l_postalCode = default!;
                string _l_name = default!;
                string _l_phoneNumber = default!;
                string _l_country = default!;
                short _r_assignedValid = 0b0;
                while (d.TryGetNextKey<FieldNames, FieldNamesWrap>(out var key))
                {
                    switch (key)
                    {
                        case FieldNames.Id:
                            _l_id = d.GetNextValue<int, Int32Wrap>();
                            _r_assignedValid |= 1 << 0;
                            break;
                        case FieldNames.Address1:
                            _l_address1 = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= 1 << 1;
                            break;
                        case FieldNames.Address2:
                            _l_address2 = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= 1 << 2;
                            break;
                        case FieldNames.City:
                            _l_city = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= 1 << 3;
                            break;
                        case FieldNames.State:
                            _l_state = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= 1 << 4;
                            break;
                        case FieldNames.PostalCode:
                            _l_postalCode = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= 1 << 5;
                            break;
                        case FieldNames.Name:
                            _l_name = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= 1 << 6;
                            break;
                        case FieldNames.PhoneNumber:
                            _l_phoneNumber = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= 1 << 7;
                            break;
                        case FieldNames.Country:
                            _l_country = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= 1 << 8;
                            break;
                    }
                }

                if (_r_assignedValid != 0b111111111)
                {
                    throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                }

                var newType = new Location
                {
                    Id = _l_id,
                    Address1 = _l_address1,
                    Address2 = _l_address2,
                    City = _l_city,
                    State = _l_state,
                    PostalCode = _l_postalCode,
                    Name = _l_name,
                    PhoneNumber = _l_phoneNumber,
                    Country = _l_country,
                };
                return newType;
            }
        }

        private static T Deserialize<T, D>(ref D deserialize)
            where T : IDeserialize<T>
            where D : IDeserializer
        {
            return T.Deserialize(ref deserialize);
        }
    }
}