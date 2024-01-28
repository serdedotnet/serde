
#nullable disable

using System;
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

        public const string SampleString = """
{
    "id": 1234,
    "address1": "The Street Name",
    "address2": "20/11",
    "city": "The City",
    "state": "The State",
    "postalCode": "abc-12",
    "name": "Nonexisting",
    "phoneNumber": "+0 11 222 333 44",
    "country": "The Greatest"
}
""";

        public static Location Sample => new Location
        {
            Id = 1234,
            Address1 = "The Street Name",
            Address2 = "20/11",
            City = "The City",
            State = "The State",
            PostalCode = "abc-12",
            Name = "Nonexisting",
            PhoneNumber = "+0 11 222 333 44",
            Country = "The Greatest"
        };
    }

    public partial record LocationWrap : IDeserialize<Location>
    {
        static Benchmarks.Location Serde.IDeserialize<Benchmarks.Location>.Deserialize<D>(ref D deserializer)
        {
            var fieldNames = new[]
            {
                "Id",
                "Address1",
                "Address2",
                "City",
                "State",
                "PostalCode",
                "Name",
                "PhoneNumber",
                "Country"
            };
            return deserializer.DeserializeType("Location", fieldNames, SerdeVisitor.Instance);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<Benchmarks.Location>
        {
            public static readonly SerdeVisitor Instance = new SerdeVisitor();
            public string ExpectedTypeName => "Benchmarks.Location";

            private sealed class FieldNameVisitor : Serde.IDeserialize<byte>, Serde.IDeserializeVisitor<byte>
            {
                public static readonly FieldNameVisitor Instance = new FieldNameVisitor();
                public static byte Deserialize<D>(ref D deserializer)
                    where D : IDeserializer => deserializer.DeserializeString(Instance);
                public string ExpectedTypeName => "string";

                byte Serde.IDeserializeVisitor<byte>.VisitString(string s) => VisitUtf8Span(System.Text.Encoding.UTF8.GetBytes(s));
                public byte VisitUtf8Span(System.ReadOnlySpan<byte> s)
                {
                    switch (s[0])
                    {
                        case (byte)'i'when s.SequenceEqual("id"u8):
                            return 1;
                        case (byte)'a'when s.SequenceEqual("address1"u8):
                            return 2;
                        case (byte)'a'when s.SequenceEqual("address2"u8):
                            return 3;
                        case (byte)'c'when s.SequenceEqual("city"u8):
                            return 4;
                        case (byte)'s'when s.SequenceEqual("state"u8):
                            return 5;
                        case (byte)'p'when s.SequenceEqual("postalCode"u8):
                            return 6;
                        case (byte)'n'when s.SequenceEqual("name"u8):
                            return 7;
                        case (byte)'p'when s.SequenceEqual("phoneNumber"u8):
                            return 8;
                        case (byte)'c'when s.SequenceEqual("country"u8):
                            return 9;
                        default:
                            return 0;
                    }
                }
            }

            Benchmarks.Location Serde.IDeserializeVisitor<Benchmarks.Location>.VisitDictionary<D>(ref D d)
            {
                int _l_id = default !;
                string _l_address1 = default !;
                string _l_address2 = default !;
                string _l_city = default !;
                string _l_state = default !;
                string _l_postalcode = default !;
                string _l_name = default !;
                string _l_phonenumber = default !;
                string _l_country = default !;
                ushort _r_assignedValid = 0b0;
                while (d.TryGetNextKey<byte, FieldNameVisitor>(out byte key))
                {
                    switch (key)
                    {
                        case 1:
                            _l_id = d.GetNextValue<int, Int32Wrap>();
                            _r_assignedValid |= ((ushort)1) << 0;
                            break;
                        case 2:
                            _l_address1 = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= ((ushort)1) << 1;
                            break;
                        case 3:
                            _l_address2 = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= ((ushort)1) << 2;
                            break;
                        case 4:
                            _l_city = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= ((ushort)1) << 3;
                            break;
                        case 5:
                            _l_state = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= ((ushort)1) << 4;
                            break;
                        case 6:
                            _l_postalcode = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= ((ushort)1) << 5;
                            break;
                        case 7:
                            _l_name = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= ((ushort)1) << 6;
                            break;
                        case 8:
                            _l_phonenumber = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= ((ushort)1) << 7;
                            break;
                        case 9:
                            _l_country = d.GetNextValue<string, StringWrap>();
                            _r_assignedValid |= ((ushort)1) << 8;
                            break;
                    }
                }

                if (_r_assignedValid != 0b111111111)
                {
                    throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
                }

                var newType = new Benchmarks.Location()
                {
                    Id = _l_id,
                    Address1 = _l_address1,
                    Address2 = _l_address2,
                    City = _l_city,
                    State = _l_state,
                    PostalCode = _l_postalcode,
                    Name = _l_name,
                    PhoneNumber = _l_phonenumber,
                    Country = _l_country,
                };
                return newType;
            }
        }

    }
}