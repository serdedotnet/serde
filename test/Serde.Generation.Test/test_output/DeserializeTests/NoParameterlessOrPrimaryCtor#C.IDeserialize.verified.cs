//HintName: C.IDeserialize.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.IDeserializeProvider<C>
{
    static IDeserialize<C> IDeserializeProvider<C>.DeserializeInstance => CDeserializeProxy.Instance;

    sealed class CDeserializeProxy : Serde.IDeserialize<C>
    {
        C Serde.IDeserialize<C>.Deserialize(IDeserializer deserializer)
        {
            int _l_a = default !;
            byte _r_assignedValid = 0;
            var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<C>();
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int _l_index_;
            while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
            {
                switch (_l_index_)
                {
                    case 0:
                        _l_a = typeDeserialize.ReadI32(_l_index_);
                        _r_assignedValid |= ((byte)1) << 0;
                        break;
                    case Serde.IDeserializeType.IndexNotFound:
                        typeDeserialize.SkipValue();
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + _l_index_);
                }
            }

            var newType = new C();
            return newType;
        }

        public static readonly CDeserializeProxy Instance = new();
        private CDeserializeProxy()
        {
        }
    }
}