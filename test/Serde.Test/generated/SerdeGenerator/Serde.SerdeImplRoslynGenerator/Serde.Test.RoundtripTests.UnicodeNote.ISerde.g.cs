
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class RoundtripTests
{
    partial record UnicodeNote
    {
        sealed partial class _SerdeObj : global::Serde.ISerde<Serde.Test.RoundtripTests.UnicodeNote>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.RoundtripTests.UnicodeNote.s_serdeInfo;

            void global::Serde.ISerialize<Serde.Test.RoundtripTests.UnicodeNote>.Serialize(Serde.Test.RoundtripTests.UnicodeNote value, global::Serde.ISerializer serializer)
            {
                var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
                var _l_type = serializer.WriteType(_l_info, 1);
                _l_type.WriteString(_l_info, 0, value.Text);
                _l_type.End(_l_info);
            }
            Serde.Test.RoundtripTests.UnicodeNote Serde.IDeserialize<Serde.Test.RoundtripTests.UnicodeNote>.Deserialize(IDeserializer deserializer)
            {
                string _l_text = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                while (true)
                {
                    var (_l_index_, _) = typeDeserialize.TryReadIndexWithName(_l_serdeInfo);
                    if (_l_index_ == Serde.ITypeDeserializer.EndOfType)
                    {
                        break;
                    }

                    switch (_l_index_)
                    {
                        case 0:
                            Serde.DeserializeException.ThrowIfDuplicate(_r_assignedValid, 0, _l_serdeInfo);
                            _l_text = typeDeserialize.ReadString(_l_serdeInfo, _l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case Serde.ITypeDeserializer.IndexNotFound:
                            typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                typeDeserialize.End(_l_serdeInfo);
                if ((_r_assignedValid & 0b1) != 0b1)
                {
                    throw Serde.DeserializeException.UnassignedMember(_r_assignedValid, 0b1, _l_serdeInfo);
                }
                var newType = new Serde.Test.RoundtripTests.UnicodeNote() {
                    Text = _l_text,
                };

                return newType;
            }
        }
    }
}
