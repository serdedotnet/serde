
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record EmptyRecord
    {
        sealed partial class _DeObj : Serde.IDeserialize<Serde.Test.SerdeInfoTests.EmptyRecord>
        {
            global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Serde.Test.SerdeInfoTests.EmptyRecord.s_serdeInfo;

            async global::System.Threading.Tasks.Task<Serde.Test.SerdeInfoTests.EmptyRecord> Serde.IDeserialize<Serde.Test.SerdeInfoTests.EmptyRecord>.Deserialize(IDeserializer deserializer)
            {

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                while (true)
                {
                    var (_l_index_, _) = await typeDeserialize.TryReadIndexWithName(_l_serdeInfo);
                    if (_l_index_ == Serde.ITypeDeserializer.EndOfType)
                    {
                        break;
                    }

                    switch (_l_index_)
                    {
                        case Serde.ITypeDeserializer.IndexNotFound:
                            await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                if ((_r_assignedValid & 0b0) != 0b0)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Serde.Test.SerdeInfoTests.EmptyRecord() {
                };

                return newType;
            }
        }
    }
}
