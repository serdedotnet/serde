
using System;
using Serde;
using Serde.Json;
using static Utils;

// Sample code for serializing and deserializing union types


// ANCHOR: union-def
[StaticCs.Closed] // Optional library for annotating union types
[GenerateSerde]
abstract partial record UnionBase
{
    private UnionBase() { }

    public partial record DerivedA(int A) : UnionBase;
    public partial record DerivedB(string B) : UnionBase;
}
// ANCHOR_END: union-def

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
