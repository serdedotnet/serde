
#nullable enable

using System;
using Serde;

namespace Serde.Json.Test;

partial class InvalidJsonTests
{
    partial class ClassWithDictionaryOfPocoList : Serde.IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPocoList>
    {
        static IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPocoList> IDeserializeProvider<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPocoList>.DeserializeInstance
            => ClassWithDictionaryOfPocoListDeserializeProxy.Instance;

        sealed partial class ClassWithDictionaryOfPocoListDeserializeProxy :Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPocoList>
        {
            Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPocoList Serde.IDeserialize<Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPocoList>.Deserialize(IDeserializer deserializer)
            {
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<Serde.Json.Test.Poco>> _l_obj = default!;

                byte _r_assignedValid = 0;

                var _l_serdeInfo = global::Serde.SerdeInfoProvider.GetInfo<ClassWithDictionaryOfPocoList>();
                var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
                int _l_index_;
                while ((_l_index_ = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
                {
                    switch (_l_index_)
                    {
                        case 0:
                            _l_obj = typeDeserialize.ReadValue<System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<Serde.Json.Test.Poco>>, Serde.DictProxy.Deserialize<string, System.Collections.Generic.List<Serde.Json.Test.Poco>, global::Serde.StringProxy, Serde.ListProxy.Deserialize<Serde.Json.Test.Poco, Serde.Json.Test.Poco>>>(_l_index_);
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
                var newType = new Serde.Json.Test.InvalidJsonTests.ClassWithDictionaryOfPocoList() {
                    Obj = _l_obj,
                };

                return newType;
            }
            public static readonly ClassWithDictionaryOfPocoListDeserializeProxy Instance = new();
            private ClassWithDictionaryOfPocoListDeserializeProxy() { }

        }
    }
}
