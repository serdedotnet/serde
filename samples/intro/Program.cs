using Serde;
using Serde.Json;

string output = JsonSerializer.Serialize(new SampleClass());

// prints: {"X":3,"Y":"sample"}
Console.WriteLine(output);

var deserialized = JsonSerializer.Deserialize<SampleClass>(output);

// prints SampleClass { X = 3, Y = sample }
Console.WriteLine(deserialized);

[GenerateSerialize, GenerateDeserialize]
partial record SampleClass
{     
    // automatically includes public properties and fields  
    public int X { get; init; } = 3;
    public string Y = "sample";
}