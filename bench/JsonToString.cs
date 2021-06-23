// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    [GenericTypeArguments(typeof(LoginViewModel))]
    [GenericTypeArguments(typeof(Location))]
    [GenericTypeArguments(typeof(Serde.Test.AllInOne))]
    public class JsonToString<T> where T : Serde.ISerialize
    {
        private T value;

        [GlobalSetup]
        public void Setup() => value = DataGenerator.Generate<T>();

        [Benchmark]
        public string JsonNet() => Newtonsoft.Json.JsonConvert.SerializeObject(value);

        [Benchmark]
        public string SystemText() => System.Text.Json.JsonSerializer.Serialize(value);

        [Benchmark]
        public string SerdeJson() => Serde.JsonSerializer.WriteToString(value);

        [Benchmark]
        public string SerdeJsonShared() => Serde.JsonSerializer.WriteToString(value);

        // DataContractJsonSerializer does not provide an API to serialize to string
        // so it's not included here (apples vs apples thing)
    }
}