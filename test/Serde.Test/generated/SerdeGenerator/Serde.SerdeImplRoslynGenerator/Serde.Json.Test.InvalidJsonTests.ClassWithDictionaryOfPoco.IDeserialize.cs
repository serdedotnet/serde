
#nullable enable

using System;
using Serde;

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithDictionaryOfPoco : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco>
    {
        static IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco> IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco>.DeserializeInstance
            => _DeObj.Instance;

        sealed partial class _DeObj :Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco>
        {
            Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco>.Deserialize(IDeserializer deserializer)
            {
                System.Collections.Generic.Dictionary<string, Serde.Json.Test.Poco> _l_obj = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ClassWithDictionaryOfPoco>();
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != ITypeDeserializer.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_obj = typeDeserialize.ReadValue<System.Collections.Generic.Dictionary<string, Serde.Json.Test.Poco>, Serde.DictProxy.De<string, Serde.Json.Test.Poco, global::Serde.StringProxy, Serde.Json.Test.Poco>>(_l_serdeInfo, _l_index_);
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
                var newType = new Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPoco() {
                    Obj = _l_obj,
                };

                return newType;
            }
            public static readonly _DeObj Instance = new();
            private _DeObj() { }

        }
    }
}
