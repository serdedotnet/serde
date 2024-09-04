using System;
using System.IO;
using System.Text.Json;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using Benchmarks;

const string LocationSample = Location.SampleString;
var options = new JsonSerializerOptions()
{
    IncludeFields = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
var json1 = System.Text.Json.JsonSerializer.Serialize(DataGenerator.CreateLocation(), options);
var json2 = Serde.Json.JsonSerializer.Serialize(DataGenerator.CreateLocation());
var loc1 = System.Text.Json.JsonSerializer.Deserialize<Location>(LocationSample, options);
var loc2 = Serde.Json.JsonSerializer.Deserialize<Location, LocationWrap>(LocationSample);

Console.WriteLine("Checking correctness of serialization: " + (loc1 == loc2));
if (loc1 != loc2)
{
    throw new InvalidOperationException($"""
Serialization is not correct
STJ:
{loc1}

Serde:
{loc2}
""");
}

var config = DefaultConfig.Instance.AddDiagnoser(MemoryDiagnoser.Default);
var summary = BenchmarkSwitcher.FromAssembly(typeof(SerializeToString<>).Assembly)
    .Run(args, config);