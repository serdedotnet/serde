using System;
using Serde.Json;
using Serde.Test;

var allInOne = new AllInOne();
var json = JsonSerializer.Serialize(allInOne);
Console.WriteLine(json);
var second = JsonSerializer.Deserialize<AllInOne>(json);
Console.Write(allInOne == second);