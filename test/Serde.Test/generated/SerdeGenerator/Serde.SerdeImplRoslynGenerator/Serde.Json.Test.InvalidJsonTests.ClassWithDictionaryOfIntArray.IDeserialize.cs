
#nullable enable
using System;
using Serde;

namespace Serde.Json.Test
{
    partial class InvalidJsonTests
    {
        partial class ClassWithDictionaryOfIntArray : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfIntArray>
        {
            static IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfIntArray> IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfIntArray>.DeserializeInstance => ClassWithDictionaryOfIntArrayDeserializeProxy.Instance;

            sealed class ClassWithDictionaryOfIntArrayDeserializeProxy : Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfIntArray>
            {
                Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfIntArray Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfIntArray>.Deserialize(IDeserializer deserializer)
                {
                    System.Collections.Generic.Dictionary<string, int[]> _l_obj = default !;
                    byte _r_assignedValid = 0;
                    var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ClassWithDictionaryOfIntArray>();
                    var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                    int _l_index_;
                    while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
                    {
                        switch (_l_index_)
                        {
                            case 0:
                                _l_obj = typeDeserialize.ReadValue<System.Collections.Generic.Dictionary<string, int[]>, Serde.DictProxy.Deserialize<string, int[], global::Serde.StringProxy, Serde.ArrayProxy.Deserialize<int, global::Serde.Int32Proxy>>>(_l_index_);
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

                    var newType = new Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfIntArray()
                    {
                        Obj = _l_obj,
                    };
                    return newType;
                }

                public static readonly ClassWithDictionaryOfIntArrayDeserializeProxy Instance = new();
                private ClassWithDictionaryOfIntArrayDeserializeProxy()
                {
                }
            }
        }
    }
}