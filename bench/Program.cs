using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using Benchmarks;

var config = DefaultConfig.Instance.AddDiagnoser(MemoryDiagnoser.Default);
var summary = BenchmarkSwitcher.FromAssembly(typeof(JsonToString<>).Assembly)
    .Run(args, config);