// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Numerics;

namespace Serde.Json.Test;

[GenerateDeserialize]
public partial class PocoDictionary
{
    public required Dictionary<string, string> key { get; set; }
}

[GenerateDeserialize]
public partial class Poco
{
    public int Id { get; set; }
}

[GenerateSerde(ForType = typeof(Vector2))]
public partial class Vector2Proxy;

[GenerateSerde(ForType = typeof(Vector2))]
public partial class Vector2Proxy2;

[GenerateSerde]
[UseProxy(ForType = typeof(Vector2), Proxy = typeof(Vector2Proxy))]
public partial class Test
{
    public required Vector2 v2;
    public required Vector2[][] vertices;
    [UseProxy(ForType = typeof(Vector2), Proxy = typeof(Vector2Proxy2), Usage = SerdeUsage.Serialize)]
    public required Vector2[][] weights;
}
