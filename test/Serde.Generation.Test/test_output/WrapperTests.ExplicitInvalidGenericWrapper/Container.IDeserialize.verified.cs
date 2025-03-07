﻿//HintName: Container.IDeserialize.cs

#nullable enable

using System;
using Serde;
partial record Container : Serde.IDeserializeProvider<Container>
{
    static IDeserialize<Container> IDeserializeProvider<Container>.DeserializeInstance
        => ContainerDeserializeProxy.Instance;

    sealed partial class ContainerDeserializeProxy :Serde.IDeserialize<Container>
    {
        Container Serde.IDeserialize<Container>.Deserialize(IDeserializer deserializer)
        {
            Original? _l_sdkdir = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<Container>();
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_sdkdir = typeDeserialize.ReadValue<Original?, Proxy>(_l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case Serde.IDeserializeType.IndexNotFound:
                        typeDeserialize.SkipValue();
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
        public static readonly ContainerDeserializeProxy Instance = new();
        private ContainerDeserializeProxy() { }

    }
}
