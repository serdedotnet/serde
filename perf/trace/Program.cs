// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using System.Text.Json;
using Benchmarks;

RunTest();

[MethodImpl(MethodImplOptions.NoInlining)]
void RunTest()
{
    const string LocationSample = Location.SampleString;
    //var options = new JsonSerializerOptions()
    //{
    //    IncludeFields = true,
    //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    //};
    //var json1 = System.Text.Json.JsonSerializer.Serialize(Location.Sample, options);
    //var loc1 = System.Text.Json.JsonSerializer.Deserialize<Location>(LocationSample, options);
    Location loc2 = Serde.Json.JsonSerializer.Deserialize<Location>(LocationSample);
    for (int i = 0; i < 10000; i++)
    {
        loc2 = Serde.Json.JsonSerializer.Deserialize<Location>(LocationSample);
    }
    Console.WriteLine(loc2 == Location.Sample);
}