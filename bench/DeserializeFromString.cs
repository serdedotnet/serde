// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    [GenericTypeArguments(typeof(LoginViewModel))]
    [GenericTypeArguments(typeof(Location))]
    public class DeserializeFromString<T> where T : Serde.IDeserialize<T>
    {
        private JsonSerializerOptions _options = null!;
        private string _value = null!;
        private byte[] _utf8 = null!;

        [GlobalSetup]
        public void Setup()
        {
            _options = new JsonSerializerOptions();
            _options.IncludeFields = true;
            _value = DataGenerator.GenerateDeserialize<T>();
            _utf8 = System.Text.Encoding.UTF8.GetBytes(_value);
        } 

        [Benchmark]
        public T JsonNet() => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(_value);

        [Benchmark]
        public T SystemText()
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(_value, _options);
        }

        [Benchmark]
        public T SerdeJson()
        {
            var deserializer = Serde.Json.JsonDeserializer.FromUtf8String(_utf8);
            return T.Deserialize(ref deserializer);
        }

        // DataContractJsonSerializer does not provide an API to serialize to string
        // so it's not included here (apples vs apples thing)
    }
}