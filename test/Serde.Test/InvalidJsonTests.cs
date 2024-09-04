
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using Xunit;

namespace Serde.Json.Test;

public sealed partial class InvalidJsonTests
{
    [Fact]
    public void Empty()
    {
        AssertInvalid("");
    }

    [Fact]
    public void LeadingZero()
    {
        AssertInvalid("01");
    }

    [Fact]
    public void DoubleComma()
    {
        AssertInvalid("[ 1 ,, 2]");
        AssertInvalid("""
        { "a": 1,,  "b": 2 }
        """);
    }

    [Fact]
    public void SkipDoubleComma()
    {
        AssertInvalid<SkipClass>("""
        { "d": [ 1,,2 ] }, "c": 3 }
        """);
        AssertInvalid<SkipClass>("""
        { "d": { "a": 1,,  "b": 2 }, "c": 3 }
        """);
    }

    [GenerateDeserialize]
    private partial class SkipClass
    {
        public int C { get; set; }
    }

    [Fact]
    public void CommaBeforeFirstElement()
    {
        AssertInvalid("[,1]");
        AssertInvalid("{, \"a\": 1}");
    }

    private static void AssertInvalid(string json)
    {
        var stj = Assert.Throws<System.Text.Json.JsonException>(() => System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json));
        var serde = Assert.Throws<Serde.Json.JsonException>(() => Serde.Json.JsonSerializer.Deserialize<JsonValue>(json));
    }

    private static void AssertInvalid<T>(string json) where T : IDeserialize<T>
    {
        var stj = Assert.Throws<System.Text.Json.JsonException>(() => System.Text.Json.JsonSerializer.Deserialize<T>(json));
        var serde = Assert.Throws<Serde.Json.JsonException>(() => Serde.Json.JsonSerializer.Deserialize<T>(json));
    }

    [Theory]
    [InlineData(typeof(ImmutableDictionary<string, string>),
        typeof(ImmutableDictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), "\"headers\"")]
    [InlineData(typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), "\"headers\"")]
    [InlineData(typeof(PocoDictionary), typeof(PocoDictionary), "\"headers\"")]
    public static void InvalidJsonForValueShouldFail(Type type, Type deserializeImpl, string json)
    {
        AssertInvalid(type, deserializeImpl, json);
    }

    public static IEnumerable<string> InvalidJsonForIntValue()
    {
        yield return @"""1""";
        yield return "[";
        yield return "}";
        yield return @"[""1""]";
        yield return "[true]";
    }

    public static IEnumerable<string> InvalidJsonForPoco()
    {
        foreach (string value in InvalidJsonForIntValue())
        {
            yield return value;
            yield return "[" + value + "]";
            if (value != "}")
            {
                // Skip "{}}" becaues this is an invalid deserialization before the JSON error
                yield return "{" + value + "}";
            }
            yield return @"{""id"":" + value + "}";
        }
    }

    [GenerateDeserialize]
    public partial class PocoWithParameterizedCtor
    {
        public required int Obj { get; set; }
    }

    [GenerateDeserialize]
    public partial class ClassWithInt
    {
        public required int Obj { get; set; }
    }

    [GenerateDeserialize]
    public partial class ClassWithIntList
    {
        public required List<int> Obj { get; set; }
    }

    [GenerateDeserialize]
    public partial class ClassWithIntArray
    {
        public required int[] Obj { get; set; }
    }

    [GenerateDeserialize]
    public partial class ClassWithPoco
    {
        public required Poco Obj { get; set; }
    }

    //[GenerateDeserialize]
    //public partial class ClassWithParameterizedCtor_WithPoco
    //{
    //    public required PocoWithParameterizedCtor Obj { get; set; }

    //    public ClassWithParameterizedCtor_WithPoco(PocoWithParameterizedCtor obj) => Obj = obj;
    //}

    [GenerateDeserialize]
    public partial class ClassWithPocoArray
    {
        public required Poco[] Obj { get; set; }
    }

    //public class ClassWithParameterizedCtor_WithPocoArray
    //{
    //    public PocoWithParameterizedCtor[] Obj { get; set; }

    //    public ClassWithParameterizedCtor_WithPocoArray(PocoWithParameterizedCtor[] obj) => Obj = obj;
    //}

    [GenerateDeserialize]
    public partial class ClassWithDictionaryOfIntArray
    {
        public required Dictionary<string, int[]> Obj { get; set; }
    }

    [GenerateDeserialize]
    public partial class ClassWithDictionaryOfPoco
    {
        public required Dictionary<string, Poco> Obj { get; set; }
    }

    [GenerateDeserialize]
    public partial class ClassWithDictionaryOfPocoList
    {
        public required Dictionary<string, List<Poco>> Obj { get; set; }
    }

    //public class ClassWithParameterizedCtor_WithDictionaryOfPocoList
    //{
    //    public Dictionary<string, List<PocoWithParameterizedCtor>> Obj { get; set; }

    //    public ClassWithParameterizedCtor_WithDictionaryOfPocoList(Dictionary<string, List<PocoWithParameterizedCtor>> obj) => Obj = obj;
    //}

    public static IEnumerable<(Type, Type)> TypesForInvalidJsonForCollectionTests()
    {
        var elementTypes = new Dictionary<Type, Type>
        {
            [typeof(int)] = typeof(Int32Wrap),
            [typeof(Poco)] = typeof(Poco),
            [typeof(ClassWithInt)] = typeof(ClassWithInt),
            [typeof(ClassWithIntList)] = typeof(ClassWithIntList),
            [typeof(ClassWithPoco)] = typeof(ClassWithPoco),
            [typeof(ClassWithPocoArray)] = typeof(ClassWithPocoArray),
            [typeof(ClassWithDictionaryOfIntArray)] = typeof(ClassWithDictionaryOfIntArray),
            [typeof(ClassWithDictionaryOfPoco)] = typeof(ClassWithDictionaryOfPoco),
            [typeof(ClassWithDictionaryOfPocoList)] = typeof(ClassWithDictionaryOfPocoList),
            [typeof(PocoWithParameterizedCtor)] = typeof(PocoWithParameterizedCtor),
            //[typeof(ClassWithParameterizedCtor_WithPoco)] = typeof(ClassWithParameterizedCtor_WithPoco),
            //[typeof(ClassWithParameterizedCtor_WithPocoArray)] = typeof(ClassWithParameterizedCtor_WithPocoArray),
            //[typeof(ClassWithParameterizedCtor_WithDictionaryOfPocoList)] = typeof(ClassWithParameterizedCtor_WithDictionaryOfPocoList),
        };

        Type GetImplType(Type input)
        {
            // Recursively walk the type and find the impls
            if (input == typeof(string))
            {
                return typeof(StringWrap);
            }
            if (!input.IsGenericType)
            {
                return elementTypes[input];
            }
            var def = input.GetGenericTypeDefinition();
            if (def == typeof(List<>))
            {
                var arg = input.GetGenericArguments()[0];
                return typeof(ListWrap.DeserializeImpl<,>).MakeGenericType(
                    arg,
                    GetImplType(arg));
            }
            else if (def == typeof(Dictionary<,>))
            {
                var keyArg = input.GetGenericArguments()[0];
                var valueArg = input.GetGenericArguments()[1];
                return typeof(DictWrap.DeserializeImpl<,,,>).MakeGenericType(
                    keyArg,
                    GetImplType(keyArg),
                    valueArg,
                    GetImplType(valueArg));
            }
            throw new ArgumentException("Unexpected type: " + input);
        }

        (Type, Type) MakeClosedCollectionType(Type openCollectionType, Type elementType)
        {
            if (openCollectionType == typeof(Dictionary<,>))
            {
                return (typeof(Dictionary<,>).MakeGenericType(typeof(string), elementType),
                        typeof(DictWrap.DeserializeImpl<,,,>).MakeGenericType(
                            typeof(string),
                            typeof(StringWrap),
                            elementType,
                            GetImplType(elementType)));
            }
            else if (openCollectionType == typeof(List<>))
            {
                return (typeof(List<>).MakeGenericType(elementType),
                        typeof(ListWrap.DeserializeImpl<,>).MakeGenericType(
                            elementType,
                            GetImplType(elementType)));
            }
            throw new InvalidOperationException("Unexpected collection type");
        }

        Type[] collectionTypes = new Type[]
        {
            typeof(List<>),
            typeof(Dictionary<,>),
        };

        foreach (var elem in elementTypes)
        {
            yield return (elem.Key, elem.Value);
        }

        List<Type> innerTypes = new List<Type>(elementTypes.Keys);

        // Create permutations of collections with 1 and 2 levels of nesting.
        for (int i = 0; i < 2; i++)
        {
            foreach (Type collectionType in collectionTypes)
            {
                List<Type> newInnerTypes = new List<Type>();

                foreach (Type elementType in innerTypes)
                {
                    var collectionTypeAndDeserializeImpl = MakeClosedCollectionType(collectionType, elementType);
                    newInnerTypes.Add(collectionTypeAndDeserializeImpl.Item1);
                    yield return collectionTypeAndDeserializeImpl;
                }

                innerTypes = newInnerTypes;
            }
        }
    }

    static IEnumerable<string> GetInvalidJsonStringsForType(Type type)
    {
        if (type == typeof(int))
        {
            foreach (string json in InvalidJsonForIntValue())
            {
                yield return json;
            }
            yield break;
        }

        if (type == typeof(Poco))
        {
            foreach (string json in InvalidJsonForPoco())
            {
                yield return json;
            }
            yield break;
        }

        Type elementType;

        if (!typeof(IEnumerable).IsAssignableFrom(type))
        {
            // Get type of "Obj" property.
            elementType = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)[0].PropertyType;
        }
        else if (type.IsArray)
        {
            elementType = type.GetElementType()!;
        }
        else if (!type.IsGenericType)
        {
            Assert.Fail("Expected generic type");
            yield break;
        }
        else
        {
            Type genericTypeDef = type.GetGenericTypeDefinition();

            if (genericTypeDef == typeof(List<>))
            {
                elementType = type.GetGenericArguments()[0];
            }
            else if (genericTypeDef == typeof(Dictionary<,>))
            {
                elementType = type.GetGenericArguments()[1];
            }
            else
            {
                Assert.Fail("Expected List or Dictionary type");
                yield break;
            }
        }

        foreach (string invalidJson in GetInvalidJsonStringsForType(elementType))
        {
            yield return "[" + invalidJson + "]";
            if (invalidJson != "}")
            {
                // Skip "}}" becaues this is an invalid deserialization before the JSON error
                yield return "{" + invalidJson + "}";
            }
            yield return @"{""obj"":" + invalidJson + "}";
        }
    }

    public static IEnumerable<object[]> DataForInvalidJsonForTypeTests()
    {
        foreach (var (type, deserializeImpl) in TypesForInvalidJsonForCollectionTests())
        {
            foreach (string invalidJson in GetInvalidJsonStringsForType(type))
            {
                yield return new object[] { type, deserializeImpl, invalidJson };
            }
        }

        yield return new object[] { typeof(int[]), typeof(ArrayWrap.DeserializeImpl<int, Int32Wrap>), @"""test""" };
        yield return new object[] { typeof(int[]), typeof(ArrayWrap.DeserializeImpl<int, Int32Wrap>), @"1" };
        yield return new object[] { typeof(int[]), typeof(ArrayWrap.DeserializeImpl<int, Int32Wrap>), @"false" };
        yield return new object[] { typeof(int[]), typeof(ArrayWrap.DeserializeImpl<int, Int32Wrap>), @"{}" };
        yield return new object[] { typeof(int[]), typeof(ArrayWrap.DeserializeImpl<int, Int32Wrap>), @"{""test"": 1}" };
        yield return new object[] { typeof(int[]), typeof(ArrayWrap.DeserializeImpl<int, Int32Wrap>), @"[""test""" };
        yield return new object[] { typeof(int[]), typeof(ArrayWrap.DeserializeImpl<int, Int32Wrap>), @"[""test""]" };
        yield return new object[] { typeof(int[]), typeof(ArrayWrap.DeserializeImpl<int, Int32Wrap>), @"[true]" };
        yield return new object[] { typeof(int[]), typeof(ArrayWrap.DeserializeImpl<int, Int32Wrap>), @"[{}]" };
        yield return new object[] { typeof(int[]), typeof(ArrayWrap.DeserializeImpl<int, Int32Wrap>), @"[[]]" };
        yield return new object[] { typeof(int[]), typeof(ArrayWrap.DeserializeImpl<int, Int32Wrap>), @"[{""test"": 1}]" };
        yield return new object[] { typeof(int[]), typeof(ArrayWrap.DeserializeImpl<int, Int32Wrap>), @"[[true]]" };
        yield return new object[] { typeof(Dictionary<string, int[]>), typeof(DictWrap.DeserializeImpl<string, StringWrap, int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>), @"{""test"": {}}" };
        yield return new object[] { typeof(Dictionary<string, int[]>), typeof(DictWrap.DeserializeImpl<string, StringWrap, int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>), @"{""test"": {""test"": 1}}" };
        yield return new object[] { typeof(Dictionary<string, int[]>), typeof(DictWrap.DeserializeImpl<string, StringWrap, int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>), @"{""test"": ""test""}" };
        yield return new object[] { typeof(Dictionary<string, int[]>), typeof(DictWrap.DeserializeImpl<string, StringWrap, int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>), @"{""test"": 1}" };
        yield return new object[] { typeof(Dictionary<string, int[]>), typeof(DictWrap.DeserializeImpl<string, StringWrap, int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>), @"{""test"": true}" };
        yield return new object[] { typeof(Dictionary<string, int[]>), typeof(DictWrap.DeserializeImpl<string, StringWrap, int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>), @"{""test"": [""test""}" };
        yield return new object[] { typeof(Dictionary<string, int[]>), typeof(DictWrap.DeserializeImpl<string, StringWrap, int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>), @"{""test"": [""test""]}" };
        yield return new object[] { typeof(Dictionary<string, int[]>), typeof(DictWrap.DeserializeImpl<string, StringWrap, int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>), @"{""test"": [[]]}" };
        yield return new object[] { typeof(Dictionary<string, int[]>), typeof(DictWrap.DeserializeImpl<string, StringWrap, int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>), @"{""test"": [true]}" };
        yield return new object[] { typeof(Dictionary<string, int[]>), typeof(DictWrap.DeserializeImpl<string, StringWrap, int[], ArrayWrap.DeserializeImpl<int, Int32Wrap>>), @"{""test"": [{}]}" };
        yield return new object[] { typeof(ClassWithIntArray), typeof(ClassWithIntArray), @"{""obj"": ""test""}" };
        yield return new object[] { typeof(ClassWithIntArray), typeof(ClassWithIntArray), @"{""obj"": 1}" };
        yield return new object[] { typeof(ClassWithIntArray), typeof(ClassWithIntArray), @"{""obj"": false}" };
        yield return new object[] { typeof(ClassWithIntArray), typeof(ClassWithIntArray), @"{""obj"": {}}" };
        yield return new object[] { typeof(ClassWithIntArray), typeof(ClassWithIntArray), @"{""obj"": {""test"": 1}}" };
        yield return new object[] { typeof(ClassWithIntArray), typeof(ClassWithIntArray), @"{""obj"": [""test""}" };
        yield return new object[] { typeof(ClassWithIntArray), typeof(ClassWithIntArray), @"{""obj"": [""test""]}" };
        yield return new object[] { typeof(ClassWithIntArray), typeof(ClassWithIntArray), @"{""obj"": [true]}" };
        yield return new object[] { typeof(ClassWithIntArray), typeof(ClassWithIntArray), @"{""obj"": [{}]}" };
        yield return new object[] { typeof(ClassWithIntArray), typeof(ClassWithIntArray), @"{""obj"": [[]]}" };
        yield return new object[] { typeof(ClassWithIntArray), typeof(ClassWithIntArray), @"{""obj"": [{""test"": 1}]}" };
        yield return new object[] { typeof(ClassWithIntArray), typeof(ClassWithIntArray), @"{""obj"": [[true]]}" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"""test""" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"1" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"false" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"{"""": 1}" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"{"""": {}}" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"{"""": {"""":""""}}" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"[""test""" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"[""test""]" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"[true]" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"[{}]" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"[[]]" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"[{""test"": 1}]" };
        yield return new object[] { typeof(Dictionary<string, string>), typeof(DictWrap.DeserializeImpl<string, StringWrap, string, StringWrap>), @"[[true]]" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":""test""}" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":1}" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":false}" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":{"""": 1}}" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":{"""": {}}}" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":{"""": {"""":""""}}}" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":[""test""}" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":[""test""]}" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":[true]}" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":[{}]}" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":[[]]}" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":[{""test"": 1}]}" };
        yield return new object[] { typeof(ClassWithDictionaryOfIntArray), typeof(ClassWithDictionaryOfIntArray), @"{""obj"":[[true]]}" };
        yield return new object[] { typeof(Dictionary<string, Poco>), typeof(DictWrap.DeserializeImpl<string, StringWrap, Poco, Poco>), @"{""key"":[{""id"":3}]}" };
        yield return new object[] { typeof(Dictionary<string, Poco>), typeof(DictWrap.DeserializeImpl<string, StringWrap, Poco, Poco>), @"{""key"":[""test""]}" };
        yield return new object[] { typeof(Dictionary<string, Poco>), typeof(DictWrap.DeserializeImpl<string, StringWrap, Poco, Poco>), @"{""key"":[1]}" };
        yield return new object[] { typeof(Dictionary<string, Poco>), typeof(DictWrap.DeserializeImpl<string, StringWrap, Poco, Poco>), @"{""key"":[false]}" };
        yield return new object[] { typeof(Dictionary<string, Poco>), typeof(DictWrap.DeserializeImpl<string, StringWrap, Poco, Poco>), @"{""key"":[]}" };
        yield return new object[] { typeof(Dictionary<string, Poco>), typeof(DictWrap.DeserializeImpl<string, StringWrap, Poco, Poco>), @"{""key"":1}" };
        yield return new object[] { typeof(Dictionary<string, List<Poco>>), typeof(DictWrap.DeserializeImpl<string, StringWrap, List<Poco>, ListWrap.DeserializeImpl<Poco, Poco>>), @"{""key"":{}}" };
        yield return new object[] { typeof(Dictionary<string, List<Poco>>), typeof(DictWrap.DeserializeImpl<string, StringWrap, List<Poco>, ListWrap.DeserializeImpl<Poco, Poco>>), @"{""key"":[[]]}" };
        yield return new object[] { typeof(Dictionary<string, Dictionary<string, Poco>>), typeof(DictWrap.DeserializeImpl<string, StringWrap, Dictionary<string, Poco>, DictWrap.DeserializeImpl<string, StringWrap, Poco, Poco>>), @"{""key"":[]}" };
        yield return new object[] { typeof(Dictionary<string, Dictionary<string, Poco>>), typeof(DictWrap.DeserializeImpl<string, StringWrap, Dictionary<string, Poco>, DictWrap.DeserializeImpl<string, StringWrap, Poco, Poco>>), @"{""key"":1}" };
    }

    [Fact]
    public static void InvalidJsonForTypeShouldFail()
    {
        foreach (object[] args in DataForInvalidJsonForTypeTests()) // ~140K tests, too many for theory to handle well with our infrastructure
        {
            var type = (Type)args[0];
            var deserializeImpl = (Type)args[1];
            var invalidJson = (string)args[2];
            AssertInvalid(type, deserializeImpl, invalidJson);
        }
    }

    private static void AssertInvalid(Type type, Type deserializeImpl, string invalidJson)
    {
        try
        {
            GetDeserialize().MakeGenericMethod(
                type,
                deserializeImpl)!.Invoke(null, [invalidJson]);
        }
        catch (Exception e)
        {
            if (e.InnerException is { } inner and not JsonException)
            {
                ExceptionDispatchInfo.Capture(inner).Throw();
            }
        }
    }

    private static readonly MethodInfo s_deserialize = typeof(JsonSerializer)
            .GetMethods()
            .First(mi =>
                mi.Name == "Deserialize"
                && mi.GetGenericArguments().Length == 2
                && mi.GetParameters().SingleOrDefault()?.ParameterType == typeof(string));

    private static MethodInfo GetDeserialize() => s_deserialize;

    [Fact]
    public static void InvalidEmptyDictionaryInput()
    {
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<string, StringWrap>("{}"));
    }
}