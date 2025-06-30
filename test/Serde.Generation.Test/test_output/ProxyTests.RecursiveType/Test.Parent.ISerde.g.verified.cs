//HintName: Test.Parent.ISerde.g.cs

#nullable enable

using System;
using Serde;

namespace Test;

partial record Parent
{
    sealed partial class _SerdeObj : global::Serde.ISerde<Test.Parent>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Test.Parent.s_serdeInfo;

        void global::Serde.ISerialize<Test.Parent>.Serialize(Test.Parent value, global::Serde.ISerializer serializer)
        {
            var _l_info = global::Serde.SerdeInfoProvider.GetInfo(this);
            var _l_type = serializer.WriteType(_l_info);
            _l_type.WriteValue<Recursive, Test.RecursiveWrap>(_l_info, 0, value.R);
            _l_type.End(_l_info);
        }
        async global::System.Threading.Tasks.ValueTask<Test.Parent> Serde.IDeserialize<Test.Parent>.Deserialize(IDeserializer deserializer)
        {
            Recursive _l_r = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = await typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_r = await typeDeserialize.ReadValue<Recursive, Test.RecursiveWrap>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case Serde.ITypeDeserializer.IndexNotFound:
                        await typeDeserialize.SkipValue(_l_serdeInfo, _l_index_);
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
    }
}
