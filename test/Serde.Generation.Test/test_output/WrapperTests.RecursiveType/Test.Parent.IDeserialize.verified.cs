//HintName: Test.Parent.IDeserialize.cs

#nullable enable

using System;
using Serde;

namespace Test;

partial record Parent : Serde.IDeserializeProvider<Test.Parent>
{
    static IDeserialize<Test.Parent> IDeserializeProvider<Test.Parent>.Instance
        => _DeObj.Instance;

    sealed partial class _DeObj :Serde.IDeserialize<Test.Parent>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Test.Parent.s_serdeInfo;

        Test.Parent Serde.IDeserialize<Test.Parent>.Deserialize(IDeserializer deserializer)
        {
            Recursive _l_r = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_r = typeDeserialize.ReadValue<Recursive, Test.RecursiveWrap>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }
            if ((_r_assignedValid & 0b1) != 0b1)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }
            var newType = new Test.Parent() {
                R = _l_r,
            };

            return newType;
        }
        public static readonly _DeObj Instance = new();
        private _DeObj() { }

    }
}
