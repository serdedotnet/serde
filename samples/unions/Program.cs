using System.Runtime.Serialization;
using Serde;
using Serde.Json;

var json = """
{ "bar": { "_t": "Bar1", "bar": 1 } }
""";
var bar = JsonSerializer.Deserialize<Foo>(json);
Console.WriteLine(bar);

[GenerateDeserialize]
partial class Foo {
  public AbstractBar bar;
}

[GenerateDeserialize]
partial class Bar1 : AbstractBar {
  public int bar;
}

[GenerateSerde]
partial class Bar2 : AbstractBar {
  public double bar;
}

abstract partial class AbstractBar : IDeserialize<AbstractBar>
{
    static AbstractBar IDeserialize<AbstractBar>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor(deserializer);
        var fieldNames = new[]
        {
            "bar"
        };
        return deserializer.DeserializeType<AbstractBar, SerdeVisitor>("AbstractBar", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : IDeserializeVisitor<AbstractBar>
    {
        private readonly IDeserializer _deserializer;
        public SerdeVisitor(IDeserializer deserializer)
        {
            _deserializer = deserializer;
        }

        public string ExpectedTypeName => "AbstractBar";

        AbstractBar IDeserializeVisitor<AbstractBar>.VisitDictionary<D>(ref D d)
        {
            var result = d.TryGetNextKey<string, StringWrap>(out string? key);
            if (!result || key != "_t")
            {
                throw new InvalidDeserializeValueException("Expected a _t field");
            }
            var value = d.GetNextValue<string, StringWrap>();
            var inline = new InlineDeserializer(_deserializer, d);
            switch (value)
            {
                case "Bar1":
                    var bar1 = InlineDeserialize<Bar1>(inline);
                    return bar1;
                case "Bar2":
                    var bar2 = InlineDeserialize<Bar2>(inline);
                    return bar2;
                default:
                    throw new InvalidDeserializeValueException($"Unexpected value {value}");
            }
        }

        private static T InlineDeserialize<T>(IDeserializer deserializer) where T : IDeserialize<T>
        {
            return T.Deserialize(ref deserializer);
        }
    }

    private class InlineDeserializer : IDeserializer
    {
        private readonly IDeserializer _deserializer;
        private IDeserializeDictionary _deserializeDictionary;

        public InlineDeserializer(IDeserializer deserializer, IDeserializeDictionary deserializeDictionary)
        {
            _deserializer = deserializer;
            _deserializeDictionary = deserializeDictionary;
        }

        public T DeserializeAny<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeBool<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeByte<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeChar<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeDecimal<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeDictionary<T, V>(V v) where V : IDeserializeVisitor<T>
            => v.VisitDictionary(ref _deserializeDictionary);

        public T DeserializeDouble<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeEnumerable<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeFloat<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeI16<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeI32<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeI64<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeIdentifier<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeNullableRef<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeSByte<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeString<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeType<T, V>(string typeName, ReadOnlySpan<string> fieldNames, V v) where V : IDeserializeVisitor<T>
            => DeserializeDictionary<T, V>(v);

        public T DeserializeU16<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeU32<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();

        public T DeserializeU64<T, V>(V v) where V : IDeserializeVisitor<T> => throw new NotImplementedException();
    }
}