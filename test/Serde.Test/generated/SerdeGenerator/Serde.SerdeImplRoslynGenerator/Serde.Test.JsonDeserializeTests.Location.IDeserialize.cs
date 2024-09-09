
#nullable enable
using System;
using Serde;

namespace Serde.Test
{
    partial class JsonDeserializeTests
    {
        partial record Location : Serde.IDeserialize<Serde.Test.JsonDeserializeTests.Location>
        {
            static Serde.Test.JsonDeserializeTests.Location Serde.IDeserialize<Serde.Test.JsonDeserializeTests.Location>.Deserialize(IDeserializer deserializer)
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
                ushort _r_assignedValid = 0;
                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Location>();
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_id = typeDeserialize.ReadI32(_l_index_);
                            _r_assignedValid |= ((ushort)1) << 0;
                            break;
                        case 1:
                            _l_address1 = typeDeserialize.ReadString(_l_index_);
                            _r_assignedValid |= ((ushort)1) << 1;
                            break;
                        case 2:
                            _l_address2 = typeDeserialize.ReadString(_l_index_);
                            _r_assignedValid |= ((ushort)1) << 2;
                            break;
                        case 3:
                            _l_city = typeDeserialize.ReadString(_l_index_);
                            _r_assignedValid |= ((ushort)1) << 3;
                            break;
                        case 4:
                            _l_state = typeDeserialize.ReadString(_l_index_);
                            _r_assignedValid |= ((ushort)1) << 4;
                            break;
                        case 5:
                            _l_postalcode = typeDeserialize.ReadString(_l_index_);
                            _r_assignedValid |= ((ushort)1) << 5;
                            break;
                        case 6:
                            _l_name = typeDeserialize.ReadString(_l_index_);
                            _r_assignedValid |= ((ushort)1) << 6;
                            break;
                        case 7:
                            _l_phonenumber = typeDeserialize.ReadString(_l_index_);
                            _r_assignedValid |= ((ushort)1) << 7;
                            break;
                        case 8:
                            _l_country = typeDeserialize.ReadString(_l_index_);
                            _r_assignedValid |= ((ushort)1) << 8;
                            break;
                        case Serde.IDeserializeType.IndexNotFound:
                            typeDeserialize.SkipValue();
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }

                if ((_r_assignedValid & 0b111111111) != 0b111111111)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }

                var newType = new Serde.Test.JsonDeserializeTests.Location()
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