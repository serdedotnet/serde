
#nullable disable

using System;
using System.Runtime.CompilerServices;
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

    [GenerateSerialize]
    public partial class Location : Serde.IDeserialize<Location>
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

        private static ReadOnlyMemory<string> s_fieldNames = new[]{"Id", "Address1", "Address2", "City", "State", "PostalCode", "Name", "PhoneNumber", "Country"};

        static Location IDeserialize<Location>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            return deserializer.DeserializeType<Location, SerdeVisitor>("Location", s_fieldNames.Span, visitor);
        }

        private struct SerdeVisitor : IDeserializeVisitor<Location>
        {
            private struct FieldVisitor : IDeserializeVisitor<byte>, IDeserialize<byte>
            {
                public string ExpectedTypeName => "UTF8 string";
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                static byte IDeserialize<byte>.Deserialize<D>(ref D deserializer)
                {
                    return deserializer.DeserializeString<byte, FieldVisitor>(new FieldVisitor());
                }

                byte IDeserializeVisitor<byte>.VisitUtf8String(ReadOnlySpan<byte> utf8)
                {
                    switch ((char)utf8[0])
                    {
                        case 'i':
                            if (utf8.SequenceEqual("id"u8))
                            {
                                return 0;
                            }
                            break;
                        case 'a':
                            if (utf8.SequenceEqual("address1"u8))
                            {
                                return 1;
                            }
                            if (utf8.SequenceEqual("address2"u8))
                            {
                                return 2;
                            }
                            break;
                        case 'c':
                            if (utf8.SequenceEqual("city"u8))
                            {
                                return 3;
                            }
                            if (utf8.SequenceEqual("country"u8))
                            {
                                return 8;
                            }
                            break;
                        case 's':
                            if (utf8.SequenceEqual("state"u8))
                            {
                                return 4;
                            }
                            break;
                        case 'p':
                            if (utf8.SequenceEqual("postalCode"u8))
                            {
                                return 5;
                            }
                            if (utf8.SequenceEqual("phoneNumber"u8))
                            {
                                return 7;
                            }
                            break;
                        case 'n':
                            if (utf8.SequenceEqual("name"u8))
                            {
                                return 6;
                            }
                            break;
                    }
                    return byte.MaxValue;
                }
            }
            public string ExpectedTypeName => "Location";
            Location Serde.IDeserializeVisitor<Location>.VisitDictionary<D>(ref D d)
            {
                Serde.Option<int> id = default;
                Serde.Option<string> address1 = default;
                Serde.Option<string> address2 = default;
                Serde.Option<string> city = default;
                Serde.Option<string> state = default;
                Serde.Option<string> postalCode = default;
                Serde.Option<string> name = default;
                Serde.Option<string> phoneNumber = default;
                Serde.Option<string> country = default;
                while (d.TryGetNextKey<byte, FieldVisitor>(out byte key))
                {
                    switch (key)
                    {
                        case 0:
                            id = d.GetNextValue<int, Int32Wrap>();
                            break;
                        case 1:
                            address1 = d.GetNextValue<string, StringWrap>();
                            break;
                        case 2:
                            address2 = d.GetNextValue<string, StringWrap>();
                            break;
                        case 3:
                            city = d.GetNextValue<string, StringWrap>();
                            break;
                        case 4:
                            state = d.GetNextValue<string, StringWrap>();
                            break;
                        case 5:
                            postalCode = d.GetNextValue<string, StringWrap>();
                            break;
                        case 6:
                            name = d.GetNextValue<string, StringWrap>();
                            break;
                        case 7:
                            phoneNumber = d.GetNextValue<string, StringWrap>();
                            break;
                        case 8:
                            country = d.GetNextValue<string, StringWrap>();
                            break;
                        default:
                            break;
                    }
                }

                var newType = new Location
                { Id = id.GetValueOrThrow(), Address1 = address1.GetValueOrThrow(), Address2 = address2.GetValueOrThrow(), City = city.GetValueOrThrow(), State = state.GetValueOrThrow(), PostalCode = postalCode.GetValueOrThrow(), Name = name.GetValueOrThrow(), PhoneNumber = phoneNumber.GetValueOrThrow(), Country = country.GetValueOrThrow() };
                return newType;
            }
        }
    }
}