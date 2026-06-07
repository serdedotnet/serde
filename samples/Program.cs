
// Unions
using System;
using CustomSerdeSample;
using EqArraySample;

Console.WriteLine("Unions sample:");
Console.WriteLine("=========================");
UnionSample.RunDemo();

Console.WriteLine();

// Custom serialization
Console.WriteLine("Custom serialization sample:");
Console.WriteLine("=========================");
CustomSerializationSample.Run();

// External types
Console.WriteLine();
Console.WriteLine("External types sample:");
Console.WriteLine("=========================");
ExternalTypesSample.Sample.Run();

// Generic types
Console.WriteLine();
Console.WriteLine("Generic types sample:");
GenericTypeSample.Run();

// Representing a type as another (the `As` option)
Console.WriteLine();
Console.WriteLine("As conversion sample:");
Console.WriteLine("=========================");
AsConversionSample.Sample.Run();

// External type via a conversion-operator proxy (the `ForType` option)
Console.WriteLine();
Console.WriteLine("Conversion proxy sample:");
Console.WriteLine("=========================");
VersionProxySample.Sample.Run();