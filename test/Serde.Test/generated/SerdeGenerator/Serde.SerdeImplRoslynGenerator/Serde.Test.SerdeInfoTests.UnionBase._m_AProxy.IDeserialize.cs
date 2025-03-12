
#nullable enable

using System;
using Serde;

namespace Serde.Test;

partial class SerdeInfoTests
{
    partial record UnionBase
    {
        partial class _m_AProxy : Serde.IDeserializeProvider<Serde.Test.SerdeInfoTests.UnionBase.A>
        {
            static IDeserialize<Serde.Test.SerdeInfoTests.UnionBase.A> IDeserializeProvider<Serde.Test.SerdeInfoTests.UnionBase.A>.DeserializeInstance
                => _DeObj.Instance;

            sealed partial class _DeObj :Serde.IDeserialize<Serde.Test.SerdeInfoTests.UnionBase.A>
            {
                Serde.Test.SerdeInfoTests.UnionBase.A Serde.IDeserialize<Serde.Test.SerdeInfoTests.UnionBase.A>.Deserialize(IDeserializer deserializer)
                {

                    byte _r_assignedValid = 0;

                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<_m_AProxy>();
                    var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                    int _l_index_;
                    while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
                    {
                        switch (_l_index_)
                        {
                            case Serde.ITypeDeserializer.IndexNotFound:
                                typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                                break;
                            default:
                                throw new InvalidOperationException("Unexpected index: " + _l_index_);
                        }
                    }
                    if ((_r_assignedValid & 0b0) != 0b0)
                    {
                        throw Serde.DeserializeException.UnassignedMember();
                    }
                    var newType = new Serde.Test.SerdeInfoTests.UnionBase.A() {
                    };

                    return newType;
                }
                public static readonly _DeObj Instance = new();
                private _DeObj() { }

            }
        }
    }
}
