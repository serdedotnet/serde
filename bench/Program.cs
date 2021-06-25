using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Benchmarks;

var config = DefaultConfig.Instance
    .AddDiagnoser(MemoryDiagnoser.Default)
    .AddJob(Job.Default.WithRuntime(CoreRtRuntime.CoreRt50));
var summary = BenchmarkSwitcher.FromAssembly(typeof(JsonToString<>).Assembly)
    .RunAll(config);