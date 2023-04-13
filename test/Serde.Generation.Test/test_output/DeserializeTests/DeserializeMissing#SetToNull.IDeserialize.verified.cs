//HintName: SetToNull.IDeserialize.cs

#nullable enable
using Serde;

partial record struct SetToNull : Serde.IDeserialize<SetToNull>
{
    static SetToNull Serde.IDeserialize<SetToNull>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "Present",
            "Missing",
            "ThrowMissing"
        };
        return deserializer.DeserializeType<SetToNull, SerdeVisitor>("SetToNull", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<SetToNull>
    {
        public string ExpectedTypeName => "SetToNull";

        SetToNull Serde.IDeserializeVisitor<SetToNull>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<string> present = default;
            Serde.Option<string?> missing = default;
            Serde.Option<string?> throwmissing = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "present":
                        present = d.GetNextValue<string, StringWrap>();
                        break;
                    case "missing":
                        missing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        break;
                    case "throwMissing":
                        throwmissing = d.GetNextValue<string?, NullableRefWrap.DeserializeImpl<string, StringWrap>>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new SetToNull()
            {
                Present = present.GetValueOrThrow("Present"),
                Missing = missing.GetValueOrDefault(null),
                ThrowMissing = throwmissing.GetValueOrThrow("ThrowMissing"),
            };
            return newType;
        }
    }
}