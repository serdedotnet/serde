//HintName: ArrayField.IDeserialize.cs

#nullable enable
using Serde;

partial class ArrayField : Serde.IDeserialize<ArrayField>
{
    static ArrayField Serde.IDeserialize<ArrayField>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "IntArr"
        };
        return deserializer.DeserializeType<ArrayField, SerdeVisitor>("ArrayField", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ArrayField>
    {
        public string ExpectedTypeName => "ArrayField";

        ArrayField Serde.IDeserializeVisitor<ArrayField>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<int[]> intarr = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "intArr":
                        intarr = d.GetNextValue<int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new ArrayField()
            {
                IntArr = intarr.GetValueOrThrow("IntArr"),
            };
            return newType;
        }
    }
}