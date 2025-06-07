
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial struct MaxSizeType
{
    sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.MaxSizeType>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.MaxSizeType.s_serdeInfo;

        void global::Serde.ISerialize<Serde.Test.MaxSizeType>.Serialize(Serde.Test.MaxSizeType value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteU8(_l_info, 0, value.Field1);
            _l_type.WriteU8(_l_info, 1, value.Field2);
            _l_type.WriteU8(_l_info, 2, value.Field3);
            _l_type.WriteU8(_l_info, 3, value.Field4);
            _l_type.WriteU8(_l_info, 4, value.Field5);
            _l_type.WriteU8(_l_info, 5, value.Field6);
            _l_type.WriteU8(_l_info, 6, value.Field7);
            _l_type.WriteU8(_l_info, 7, value.Field8);
            _l_type.WriteU8(_l_info, 8, value.Field9);
            _l_type.WriteU8(_l_info, 9, value.Field10);
            _l_type.WriteU8(_l_info, 10, value.Field11);
            _l_type.WriteU8(_l_info, 11, value.Field12);
            _l_type.WriteU8(_l_info, 12, value.Field13);
            _l_type.WriteU8(_l_info, 13, value.Field14);
            _l_type.WriteU8(_l_info, 14, value.Field15);
            _l_type.WriteU8(_l_info, 15, value.Field16);
            _l_type.WriteU8(_l_info, 16, value.Field17);
            _l_type.WriteU8(_l_info, 17, value.Field18);
            _l_type.WriteU8(_l_info, 18, value.Field19);
            _l_type.WriteU8(_l_info, 19, value.Field20);
            _l_type.WriteU8(_l_info, 20, value.Field21);
            _l_type.WriteU8(_l_info, 21, value.Field22);
            _l_type.WriteU8(_l_info, 22, value.Field23);
            _l_type.WriteU8(_l_info, 23, value.Field24);
            _l_type.WriteU8(_l_info, 24, value.Field25);
            _l_type.WriteU8(_l_info, 25, value.Field26);
            _l_type.WriteU8(_l_info, 26, value.Field27);
            _l_type.WriteU8(_l_info, 27, value.Field28);
            _l_type.WriteU8(_l_info, 28, value.Field29);
            _l_type.WriteU8(_l_info, 29, value.Field30);
            _l_type.WriteU8(_l_info, 30, value.Field31);
            _l_type.WriteU8(_l_info, 31, value.Field32);
            _l_type.WriteU8(_l_info, 32, value.Field33);
            _l_type.WriteU8(_l_info, 33, value.Field34);
            _l_type.WriteU8(_l_info, 34, value.Field35);
            _l_type.WriteU8(_l_info, 35, value.Field36);
            _l_type.WriteU8(_l_info, 36, value.Field37);
            _l_type.WriteU8(_l_info, 37, value.Field38);
            _l_type.WriteU8(_l_info, 38, value.Field39);
            _l_type.WriteU8(_l_info, 39, value.Field40);
            _l_type.WriteU8(_l_info, 40, value.Field41);
            _l_type.WriteU8(_l_info, 41, value.Field42);
            _l_type.WriteU8(_l_info, 42, value.Field43);
            _l_type.WriteU8(_l_info, 43, value.Field44);
            _l_type.WriteU8(_l_info, 44, value.Field45);
            _l_type.WriteU8(_l_info, 45, value.Field46);
            _l_type.WriteU8(_l_info, 46, value.Field47);
            _l_type.WriteU8(_l_info, 47, value.Field48);
            _l_type.WriteU8(_l_info, 48, value.Field49);
            _l_type.WriteU8(_l_info, 49, value.Field50);
            _l_type.WriteU8(_l_info, 50, value.Field51);
            _l_type.WriteU8(_l_info, 51, value.Field52);
            _l_type.WriteU8(_l_info, 52, value.Field53);
            _l_type.WriteU8(_l_info, 53, value.Field54);
            _l_type.WriteU8(_l_info, 54, value.Field55);
            _l_type.WriteU8(_l_info, 55, value.Field56);
            _l_type.WriteU8(_l_info, 56, value.Field57);
            _l_type.WriteU8(_l_info, 57, value.Field58);
            _l_type.WriteU8(_l_info, 58, value.Field59);
            _l_type.WriteU8(_l_info, 59, value.Field60);
            _l_type.WriteU8(_l_info, 60, value.Field61);
            _l_type.WriteU8(_l_info, 61, value.Field62);
            _l_type.WriteU8(_l_info, 62, value.Field63);
            _l_type.WriteU8(_l_info, 63, value.Field64);
            _l_type.End(_l_info);
        }
        Serde.Test.MaxSizeType Serde.IDeserialize<Serde.Test.MaxSizeType>.Deserialize(IDeserializer deserializer)
        {
            byte _l_field1 = default!;
            byte _l_field2 = default!;
            byte _l_field3 = default!;
            byte _l_field4 = default!;
            byte _l_field5 = default!;
            byte _l_field6 = default!;
            byte _l_field7 = default!;
            byte _l_field8 = default!;
            byte _l_field9 = default!;
            byte _l_field10 = default!;
            byte _l_field11 = default!;
            byte _l_field12 = default!;
            byte _l_field13 = default!;
            byte _l_field14 = default!;
            byte _l_field15 = default!;
            byte _l_field16 = default!;
            byte _l_field17 = default!;
            byte _l_field18 = default!;
            byte _l_field19 = default!;
            byte _l_field20 = default!;
            byte _l_field21 = default!;
            byte _l_field22 = default!;
            byte _l_field23 = default!;
            byte _l_field24 = default!;
            byte _l_field25 = default!;
            byte _l_field26 = default!;
            byte _l_field27 = default!;
            byte _l_field28 = default!;
            byte _l_field29 = default!;
            byte _l_field30 = default!;
            byte _l_field31 = default!;
            byte _l_field32 = default!;
            byte _l_field33 = default!;
            byte _l_field34 = default!;
            byte _l_field35 = default!;
            byte _l_field36 = default!;
            byte _l_field37 = default!;
            byte _l_field38 = default!;
            byte _l_field39 = default!;
            byte _l_field40 = default!;
            byte _l_field41 = default!;
            byte _l_field42 = default!;
            byte _l_field43 = default!;
            byte _l_field44 = default!;
            byte _l_field45 = default!;
            byte _l_field46 = default!;
            byte _l_field47 = default!;
            byte _l_field48 = default!;
            byte _l_field49 = default!;
            byte _l_field50 = default!;
            byte _l_field51 = default!;
            byte _l_field52 = default!;
            byte _l_field53 = default!;
            byte _l_field54 = default!;
            byte _l_field55 = default!;
            byte _l_field56 = default!;
            byte _l_field57 = default!;
            byte _l_field58 = default!;
            byte _l_field59 = default!;
            byte _l_field60 = default!;
            byte _l_field61 = default!;
            byte _l_field62 = default!;
            byte _l_field63 = default!;
            byte _l_field64 = default!;

            ulong _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_field1 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 0;
                        break;
                    case 1:
                        _l_field2 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 1;
                        break;
                    case 2:
                        _l_field3 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 2;
                        break;
                    case 3:
                        _l_field4 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 3;
                        break;
                    case 4:
                        _l_field5 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 4;
                        break;
                    case 5:
                        _l_field6 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 5;
                        break;
                    case 6:
                        _l_field7 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 6;
                        break;
                    case 7:
                        _l_field8 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 7;
                        break;
                    case 8:
                        _l_field9 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 8;
                        break;
                    case 9:
                        _l_field10 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 9;
                        break;
                    case 10:
                        _l_field11 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 10;
                        break;
                    case 11:
                        _l_field12 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 11;
                        break;
                    case 12:
                        _l_field13 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 12;
                        break;
                    case 13:
                        _l_field14 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 13;
                        break;
                    case 14:
                        _l_field15 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 14;
                        break;
                    case 15:
                        _l_field16 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 15;
                        break;
                    case 16:
                        _l_field17 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 16;
                        break;
                    case 17:
                        _l_field18 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 17;
                        break;
                    case 18:
                        _l_field19 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 18;
                        break;
                    case 19:
                        _l_field20 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 19;
                        break;
                    case 20:
                        _l_field21 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 20;
                        break;
                    case 21:
                        _l_field22 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 21;
                        break;
                    case 22:
                        _l_field23 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 22;
                        break;
                    case 23:
                        _l_field24 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 23;
                        break;
                    case 24:
                        _l_field25 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 24;
                        break;
                    case 25:
                        _l_field26 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 25;
                        break;
                    case 26:
                        _l_field27 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 26;
                        break;
                    case 27:
                        _l_field28 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 27;
                        break;
                    case 28:
                        _l_field29 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 28;
                        break;
                    case 29:
                        _l_field30 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 29;
                        break;
                    case 30:
                        _l_field31 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 30;
                        break;
                    case 31:
                        _l_field32 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 31;
                        break;
                    case 32:
                        _l_field33 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 32;
                        break;
                    case 33:
                        _l_field34 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 33;
                        break;
                    case 34:
                        _l_field35 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 34;
                        break;
                    case 35:
                        _l_field36 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 35;
                        break;
                    case 36:
                        _l_field37 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 36;
                        break;
                    case 37:
                        _l_field38 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 37;
                        break;
                    case 38:
                        _l_field39 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 38;
                        break;
                    case 39:
                        _l_field40 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 39;
                        break;
                    case 40:
                        _l_field41 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 40;
                        break;
                    case 41:
                        _l_field42 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 41;
                        break;
                    case 42:
                        _l_field43 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 42;
                        break;
                    case 43:
                        _l_field44 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 43;
                        break;
                    case 44:
                        _l_field45 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 44;
                        break;
                    case 45:
                        _l_field46 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 45;
                        break;
                    case 46:
                        _l_field47 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 46;
                        break;
                    case 47:
                        _l_field48 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 47;
                        break;
                    case 48:
                        _l_field49 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 48;
                        break;
                    case 49:
                        _l_field50 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 49;
                        break;
                    case 50:
                        _l_field51 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 50;
                        break;
                    case 51:
                        _l_field52 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 51;
                        break;
                    case 52:
                        _l_field53 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 52;
                        break;
                    case 53:
                        _l_field54 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 53;
                        break;
                    case 54:
                        _l_field55 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 54;
                        break;
                    case 55:
                        _l_field56 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 55;
                        break;
                    case 56:
                        _l_field57 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 56;
                        break;
                    case 57:
                        _l_field58 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 57;
                        break;
                    case 58:
                        _l_field59 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 58;
                        break;
                    case 59:
                        _l_field60 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 59;
                        break;
                    case 60:
                        _l_field61 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 60;
                        break;
                    case 61:
                        _l_field62 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 61;
                        break;
                    case 62:
                        _l_field63 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 62;
                        break;
                    case 63:
                        _l_field64 = typeDeserialize.ReadU8(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((ulong)1) << 63;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b1111111111111111111111111111111111111111111111111111111111111111) != 0b1111111111111111111111111111111111111111111111111111111111111111)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new Serde.Test.MaxSizeType() {
                Field1 = _l_field1,
                Field2 = _l_field2,
                Field3 = _l_field3,
                Field4 = _l_field4,
                Field5 = _l_field5,
                Field6 = _l_field6,
                Field7 = _l_field7,
                Field8 = _l_field8,
                Field9 = _l_field9,
                Field10 = _l_field10,
                Field11 = _l_field11,
                Field12 = _l_field12,
                Field13 = _l_field13,
                Field14 = _l_field14,
                Field15 = _l_field15,
                Field16 = _l_field16,
                Field17 = _l_field17,
                Field18 = _l_field18,
                Field19 = _l_field19,
                Field20 = _l_field20,
                Field21 = _l_field21,
                Field22 = _l_field22,
                Field23 = _l_field23,
                Field24 = _l_field24,
                Field25 = _l_field25,
                Field26 = _l_field26,
                Field27 = _l_field27,
                Field28 = _l_field28,
                Field29 = _l_field29,
                Field30 = _l_field30,
                Field31 = _l_field31,
                Field32 = _l_field32,
                Field33 = _l_field33,
                Field34 = _l_field34,
                Field35 = _l_field35,
                Field36 = _l_field36,
                Field37 = _l_field37,
                Field38 = _l_field38,
                Field39 = _l_field39,
                Field40 = _l_field40,
                Field41 = _l_field41,
                Field42 = _l_field42,
                Field43 = _l_field43,
                Field44 = _l_field44,
                Field45 = _l_field45,
                Field46 = _l_field46,
                Field47 = _l_field47,
                Field48 = _l_field48,
                Field49 = _l_field49,
                Field50 = _l_field50,
                Field51 = _l_field51,
                Field52 = _l_field52,
                Field53 = _l_field53,
                Field54 = _l_field54,
                Field55 = _l_field55,
                Field56 = _l_field56,
                Field57 = _l_field57,
                Field58 = _l_field58,
                Field59 = _l_field59,
                Field60 = _l_field60,
                Field61 = _l_field61,
                Field62 = _l_field62,
                Field63 = _l_field63,
                Field64 = _l_field64,
            };

            return newType;
        }
    }
}
