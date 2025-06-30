//HintName: S.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial struct S
{
    sealed partial class _DeObj : Serde.IDeserialize<S>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => S.s_serdeInfo;

        async global::System.Threading.Tasks.Task<S> Serde.IDeserialize<S>.Deserialize(IDeserializer deserializer)
        {
            System.Collections.Immutable.ImmutableArray<System.Runtime.InteropServices.ComTypes.BIND_OPTS> _l_opts = default!;

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
                    case 0:
                        _l_opts = await typeDeserialize.ReadBoxedValue<System.Collections.Immutable.ImmutableArray<System.Runtime.InteropServices.ComTypes.BIND_OPTS>, Serde.ImmutableArrayProxy.De<System.Runtime.InteropServices.ComTypes.BIND_OPTS, OptsWrap>>(_l_serdeInfo, _l_index_);
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
            var newType = new S() {
                Opts = _l_opts,
            };

            return newType;
        }
    }
}
