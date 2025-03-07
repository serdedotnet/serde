
#nullable enable

using System;
using Serde;

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithInt : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithInt>
    {
        static IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithInt> IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithInt>.DeserializeInstance
            => ClassWithIntDeserializeProxy.Instance;

        sealed partial class ClassWithIntDeserializeProxy :Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithInt>
        {
            Serde.Json.Test.InvalidJsonTests.ClassWithInt Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithInt>.Deserialize(IDeserializer deserializer)
            {
                int _l_obj = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ClassWithInt>();
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_obj = typeDeserialize.ReadI32(_l_index_);
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
                var newType = new Serde.Json.Test.InvalidJsonTests.ClassWithInt() {
                    Obj = _l_obj,
                };

                return newType;
            }
            public static readonly ClassWithIntDeserializeProxy Instance = new();
            private ClassWithIntDeserializeProxy() { }

        }
    }
}
