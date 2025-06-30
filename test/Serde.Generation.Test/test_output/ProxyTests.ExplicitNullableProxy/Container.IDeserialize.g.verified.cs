//HintName: Container.IDeserialize.g.cs

#nullable enable

using System;
using Serde;
partial record Container
{
    sealed partial class _DeObj : Serde.IDeserialize<Container>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Container.s_serdeInfo;

        async global::System.Threading.Tasks.ValueTask<Container> Serde.IDeserialize<Container>.Deserialize(IDeserializer deserializer)
        {
            Original? _l_sdkdir = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = await typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_sdkdir = await typeDeserialize.ReadBoxedValue<Original?, Serde.NullableProxy.De<Original, Proxy>>(_l_serdeInfo, _l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
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
            var newType = new Container() {
                SdkDir = _l_sdkdir,
            };

            return newType;
        }
    }
}
