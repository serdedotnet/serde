# Handling generic types

Generic types are more complicated because serialize and deserialize must be separated, in case one of the nested types only implements one of the two operations.

The pattern is to separate into two separate classes nested underneath a generic type. An example of a custom collection type is as follows:

```csharp
{{#include ../samples/GenericTypeSample.cs}}
```