// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    [GenericTypeArguments(typeof(LoginViewModel))]
    [GenericTypeArguments(typeof(Location))]
    [GenericTypeArguments(typeof(Serde.Test.AllInOne))]
    public class SerializeToString<T> where T : Serde.ISerialize<T>
    {
        private JsonSerializerOptions _options = null!;
        private T value = default!;

        [GlobalSetup]
        public void Setup()
        {
            _options = new JsonSerializerOptions();
            _options.IncludeFields = true;
            _options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            value = DataGenerator.GenerateSerialize<T>();
        }

        [Benchmark]
        public string JsonNet() => Newtonsoft.Json.JsonConvert.SerializeObject(value);

        [Benchmark]
        public string SystemText()
        {
            return System.Text.Json.JsonSerializer.Serialize(value, _options);
        }

        [Benchmark]
        public string SerdeJson() => Serde.Json.JsonSerializer.Serialize(value);

        // DataContractJsonSerializer does not provide an API to serialize to string
        // so it's not included here (apples vs apples thing)
    }
}