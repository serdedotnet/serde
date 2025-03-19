﻿//HintName: Test.ChannelList.IDeserialize.cs

#nullable enable

using System;
using Serde;

namespace Test;

partial record struct ChannelList : Serde.IDeserializeProvider<Test.ChannelList>
{
    static IDeserialize<Test.ChannelList> IDeserializeProvider<Test.ChannelList>.Instance
        => _DeObj.Instance;

    sealed partial class _DeObj :Serde.IDeserialize<Test.ChannelList>
    {
        global::Serde.ISerdeInfo global::Serde.ISerdeInfoProvider.SerdeInfo => Test.ChannelList.s_serdeInfo;

        Test.ChannelList Serde.IDeserialize<Test.ChannelList>.Deserialize(IDeserializer deserializer)
        {
            System.Collections.Immutable.ImmutableArray<Test.Channel> _l_channels = default!;

            byte _r_assignedValid = 0;

            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo(this);
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_channels = typeDeserialize.ReadBoxedValue<System.Collections.Immutable.ImmutableArray<Test.Channel>, Serde.ImmutableArrayProxy.De<Test.Channel, Test.ChannelProxy>>(_l_serdeInfo, _l_index_);
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
            var newType = new Test.ChannelList() {
                Channels = _l_channels,
            };

            return newType;
        }
        public static readonly _DeObj Instance = new();
        private _DeObj() { }

    }
}
