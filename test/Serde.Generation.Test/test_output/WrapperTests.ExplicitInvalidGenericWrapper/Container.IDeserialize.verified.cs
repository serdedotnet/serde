//HintName: Container.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial record Container : Serde.IDeserialize<Container>
{
    static Container Serde.IDeserialize<Container>.Deserialize(IDeserializer deserializer)
    {
        Original? _l_sdkdir = default !;
        byte _r_assignedValid = 0;
        var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Container>();
        var typeDeserialize = deserializer.DeserializeType(_l_serdeInfo);
        int _l_index_;
        while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out var _l_errorName)) != IDeserializeType.EndOfType)
        {
            switch (_l_index_)
            {
                case 0:
                    _l_sdkdir = typeDeserialize.ReadValue<Original?, Proxy>(_l_index_);
                    _r_assignedValid |= ((byte)1) << 0;
                    break;
                case Serde.IDeserializeType.IndexNotFound:
                    break;
                default:
                    throw new InvalidOperationException("Unexpected index: " + _l_index_);
            }
        }

        if ((_r_assignedValid & 0b0) != 0b0)
        {
            throw new Serde.InvalidDeserializeValueException("Not all members were assigned");
        }

        var newType = new Container()
        {
            SdkDir = _l_sdkdir,
        };
        return newType;
    }
}