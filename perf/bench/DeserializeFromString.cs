// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Serde;
using Serde.Test;
using STJ = System.Text.Json;

namespace Benchmarks
{
    [GenericTypeArguments(typeof(LoginViewModel), typeof(LoginViewModel))]
    [GenericTypeArguments(typeof(Location), typeof(LocationWrap))]
    [GenericTypeArguments(typeof(Primitives), typeof(Primitives))]
    [GenericTypeArguments(typeof(Guids), typeof(Guids))]
    [GenericTypeArguments(typeof(AllInOne), typeof(AllInOne))]
    public class DeserializeFromString<T, U>
        where T : Serde.IDeserializeProvider<T>
        where U : Serde.IDeserializeProvider<T>
    {
        private JsonSerializerOptions _options = null!;
        private string value = null!;

        private readonly IDeserialize<T> _proxy = T.Instance;
        private readonly IDeserialize<T> _manualProxy = U.Instance;

        [GlobalSetup]
        public void Setup()
        {
            _options = new JsonSerializerOptions()
            {
                IncludeFields = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new STJ.Serialization.JsonStringEnumConverter(STJ.JsonNamingPolicy.CamelCase)
                }
            };
            value = DataGenerator.GenerateDeserialize<T>();
        }

        [Benchmark]
        public T? JsonNet() => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);

        [Benchmark]
        public T? SystemText()
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(value, _options);
        }

        [Benchmark]
        public T SerdeJson() => Serde.Json.JsonSerializer.Deserialize(value, _proxy);

        [Benchmark]
        public T SerdeManual() => Serde.Json.JsonSerializer.Deserialize(value, _manualProxy);

        // DataContractJsonSerializer does not provide an API to serialize to string
        // so it's not included here (apples vs apples thing)
    }
}