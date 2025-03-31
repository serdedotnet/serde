
using System;
using Serde;
using Serde.Json;
using static Utils;

// Sample code for serializing and deserializing union types


[StaticCs.Closed] // Library for annotating union types
[GenerateSerde]
abstract partial record UnionBase
{
    private UnionBase() { }

    public sealed partial record DerivedA(int A) : UnionBase;
    public sealed partial record DerivedB(string B) : UnionBase;
}

public static class UnionSample
{
    public static void RunDemo()
    {
        var a = new UnionBase.DerivedA(1);
        var b = new UnionBase.DerivedB("foo");
        Console.WriteLine($"a: {a}");
        Console.WriteLine($"b: {b}");

        var aSerialized = JsonSerializer.Serialize<UnionBase>(a);
        var bSerialized = JsonSerializer.Serialize<UnionBase>(b);

        var aExpected = """{"DerivedA":{"a":1}}""";
        AssertEq(aExpected, aSerialized);
        var bExpected = """{"DerivedB":{"b":"foo"}}""";
        AssertEq(bExpected, bSerialized);

        Console.WriteLine($"a: {aSerialized}");
        Console.WriteLine($"b: {bSerialized}");

        AssertEq(a, JsonSerializer.Deserialize<UnionBase>(aSerialized));
        AssertEq(b, JsonSerializer.Deserialize<UnionBase>(bSerialized));
    }
}
