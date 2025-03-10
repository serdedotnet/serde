
#nullable enable

using System;
using Serde;

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithIntArray : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithIntArray>
    {
        static IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithIntArray> IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithIntArray>.DeserializeInstance
            => ClassWithIntArrayDeserializeProxy.Instance;

        sealed partial class ClassWithIntArrayDeserializeProxy :Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithIntArray>
        {
            Serde.Json.Test.InvalidJsonTests.ClassWithIntArray Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithIntArray>.Deserialize(IDeserializer deserializer)
            {
                int[] _l_obj = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ClassWithIntArray>();
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_obj = typeDeserialize.ReadValue<int[], Serde.ArrayProxy.Deserialize<int, global::Serde.I32Proxy>>(_l_index_);
                            _r_assignedValid |= ((byte)1) << 0;
                            break;
                        case Serde.IDeserializeType.IndexNotFound:
                            typeDeserialize.SkipValue();
                            break;
                        default:
                            throw new InvalidOperationException("Unexpected index: " + _l_index_);
                    }
                }
                if ((_r_assignedValid & 0b1) != 0b1)
                {
                    throw Serde.DeserializeException.UnassignedMember();
                }
                var newType = new Serde.Json.Test.InvalidJsonTests.ClassWithIntArray() {
                    Obj = _l_obj,
                };

                return newType;
            }
            public static readonly ClassWithIntArrayDeserializeProxy Instance = new();
            private ClassWithIntArrayDeserializeProxy() { }

        }
    }
}
