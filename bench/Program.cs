using System;
using System.Diagnostics;
using System.Text.Json;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using Benchmarks;

#if DEBUG

const string LocationSample = DataGenerator.LocationSample;
var options = new JsonSerializerOptions()
{
    IncludeFields = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
var loc1 = System.Text.Json.JsonSerializer.Deserialize<Location>(LocationSample, options);
var loc2 = Serde.Json.JsonSerializer.Deserialize<Location>(LocationSample);

Console.WriteLine(loc1 == loc2);

#else

var config = DefaultConfig.Instance.AddDiagnoser(MemoryDiagnoser.Default);
var summary = BenchmarkSwitcher.FromAssembly(typeof(JsonToString<>).Assembly)
    .Run(args, config);

#endif