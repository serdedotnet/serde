using System.Diagnostics;
using Serde;
using Serde.Json;
using StaticCs;

var a = new BaseType.DerivedA { A = 1 };
var b = new BaseType.DerivedB { B = "foo" };
var aSerialized = JsonSerializer.Serialize(a);
var bSerialized = JsonSerializer.Serialize(b);

Console.WriteLine($"a: {aSerialized}, " + (aSerialized == """{"DerivedA":{"a":1}}"""));
Console.WriteLine($"b: {bSerialized}, " + (bSerialized == """{"DerivedB":{"b":"foo"}}"""));

Console.WriteLine("a: " + (JsonSerializer.Deserialize<BaseType>(aSerialized) == a));
Console.WriteLine("b: " + (JsonSerializer.Deserialize<BaseType>(bSerialized) == b));

[Closed]
abstract partial record BaseType
{
    private BaseType() { }

    public sealed partial record DerivedA : BaseType
    {
        public required int A { get; init; }
    }
    public sealed partial record DerivedB : BaseType
    {
        public required string B { get; init; }
    }
}

partial record BaseType : ISerialize<BaseType>
{
    public void Serialize(BaseType value, ISerializer serializer)
    {
        var serializeType = serializer.SerializeType("BaseType", 2);
        switch (value)
        {
            case DerivedA derivedA:
                serializeType.SerializeField<DerivedA, DerivedAWrap>(nameof(DerivedA), derivedA);
                break;
            case DerivedB derivedB:
                serializeType.SerializeField<DerivedB, DerivedBWrap>(nameof(DerivedB), derivedB);
                break;
        }
        serializeType.End();
    }

    [GenerateSerde(ThroughMember = nameof(Value))]
    private readonly partial record struct DerivedAWrap(DerivedA Value);

    [GenerateSerde(ThroughMember = nameof(Value))]
    private readonly partial record struct DerivedBWrap(DerivedB Value);
}

partial record BaseType : IDeserialize<BaseType>
{
    public static BaseType Deserialize(IDeserializer deserializer) where D : IDeserializer
    {
        return deserializer.DeserializeDictionary<BaseType, DeserializeVisitor>(new DeserializeVisitor());
    }

    [Closed]
    [GenerateDeserialize]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    private enum KeyNames
    {
        DerivedA,
        DerivedB,
    }

    private sealed class DeserializeVisitor : IDeserializeVisitor<BaseType>
    {
        public string ExpectedTypeName => nameof(BaseType);

        BaseType IDeserializeVisitor<BaseType>.VisitDictionary<D>(ref D deserializer)
        {
            deserializer.TryGetNextKey<KeyNames, KeyNamesWrap>(out var type);
            switch (type)
            {
                case KeyNames.DerivedA:
                    return deserializer.GetNextValue<DerivedA, DerivedAWrap>();
                case KeyNames.DerivedB:
                    return deserializer.GetNextValue<DerivedB, DerivedBWrap>();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}