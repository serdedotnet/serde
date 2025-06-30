
#nullable disable

using System;
using System.Threading.Tasks;
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

    public partial record LocationWrap : IDeserialize<Location>, IDeserializeProvider<Location>
    {
        public static LocationWrap Instance { get; } = new();
        static IDeserialize<Location> IDeserializeProvider<Location>.Instance => Instance;
        private LocationWrap() { }

        public ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "Location",
            typeof(Location).GetCustomAttributesData(),
            [
                ("id", I32Proxy.SerdeInfo, typeof(Location).GetProperty("Id")),
                ("address1", StringProxy.SerdeInfo, typeof(Location).GetProperty("Address1")),
                ("address2", StringProxy.SerdeInfo, typeof(Location).GetProperty("Address2")),
                ("city", StringProxy.SerdeInfo, typeof(Location).GetProperty("City")),
                ("state", StringProxy.SerdeInfo, typeof(Location).GetProperty("State")),
                ("postalCode", StringProxy.SerdeInfo, typeof(Location).GetProperty("PostalCode")),
                ("name", StringProxy.SerdeInfo, typeof(Location).GetProperty("Name")),
                ("phoneNumber", StringProxy.SerdeInfo, typeof(Location).GetProperty("PhoneNumber")),
                ("country", StringProxy.SerdeInfo, typeof(Location).GetProperty("Country"))
            ]);

        async Task<Benchmarks.Location> Serde.IDeserialize<Benchmarks.Location>.Deserialize(IDeserializer deserializer)
        {
            int _l_id = default!;
            string _l_address1 = default!;
            string _l_address2 = default!;
            string _l_city = default!;
            string _l_state = default!;
            string _l_postalcode = default!;
            string _l_name = default!;
            string _l_phonenumber = default!;
            string _l_country = default!;
            ushort _r_assignedValid = 0b0;

            var _l_serdeInfo = SerdeInfo;
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            while (true)
            {
                var index = await typeDeserialize.TryReadIndex(_l_serdeInfo);
                if (index == ITypeDeserializer.EndOfType)
                {
                    break;
                }
                switch (index)
                {
                    case 0:
                        _l_id = await typeDeserialize.ReadI32(_l_serdeInfo, index);
                        _r_assignedValid |= ((ushort)1) << 0;
                        break;
                    case 1:
                        _l_address1 = await typeDeserialize.ReadString(_l_serdeInfo, index);
                        _r_assignedValid |= ((ushort)1) << 1;
                        break;
                    case 2:
                        _l_address2 = await typeDeserialize.ReadString(_l_serdeInfo, index);
                        _r_assignedValid |= ((ushort)1) << 2;
                        break;
                    case 3:
                        _l_city = await typeDeserialize.ReadString(_l_serdeInfo, index);
                        _r_assignedValid |= ((ushort)1) << 3;
                        break;
                    case 4:
                        _l_state = await typeDeserialize.ReadString(_l_serdeInfo, index);
                        _r_assignedValid |= ((ushort)1) << 4;
                        break;
                    case 5:
                        _l_postalcode = await typeDeserialize.ReadString(_l_serdeInfo, index);
                        _r_assignedValid |= ((ushort)1) << 5;
                        break;
                    case 6:
                        _l_name = await typeDeserialize.ReadString(_l_serdeInfo, index);
                        _r_assignedValid |= ((ushort)1) << 6;
                        break;
                    case 7:
                        _l_phonenumber = await typeDeserialize.ReadString(_l_serdeInfo, index);
                        _r_assignedValid |= ((ushort)1) << 7;
                        break;
                    case 8:
                        _l_country = await typeDeserialize.ReadString(_l_serdeInfo, index);
                        _r_assignedValid |= ((ushort)1) << 8;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        await typeDeserialize.SkipValue(_l_serdeInfo, index);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + index);
                }
            }

            if (_r_assignedValid != 0b111111111)
            {
                throw Serde.DeserializeException.UnassignedMember();
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

    [GenerateSerde]
    public partial record Primitives(
        bool BoolField,
        char CharField,
        byte ByteField,
        ushort UShortField,
        uint UIntField,
        ulong ULongField,

        sbyte SByteField,
        short ShortField,
        int IntField,
        long LongField
    )
    {
        public static readonly Primitives Sample = new Primitives(
            BoolField: true,
            CharField: '#',
            ByteField: byte.MaxValue,
            UShortField: ushort.MaxValue,
            UIntField: uint.MaxValue,
            ULongField: ulong.MaxValue,

            SByteField: sbyte.MaxValue,
            ShortField: short.MaxValue,
            IntField: int.MaxValue,
            LongField: long.MaxValue
        );

        public const string SampleSerialized = """
{
  "boolField": true,
  "charField": "#",
  "byteField": 255,
  "uShortField": 65535,
  "uIntField": 4294967295,
  "uLongField": 18446744073709551615,
  "sByteField": 127,
  "shortField": 32767,
  "intField": 2147483647,
  "longField": 9223372036854775807
}
""";


    }
}