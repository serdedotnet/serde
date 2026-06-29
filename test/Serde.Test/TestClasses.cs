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

[GenerateSerde(ForType = typeof(Vector3))]
public partial class Vector3Proxy;

[GenerateSerde(ForType = typeof(Vector4))]
public partial class Vector4Proxy;

[GenerateSerde]
[UseProxy(ForType = typeof(Vector2), Proxy = typeof(Vector2Proxy))]
[UseProxy(ForType = typeof(Vector3), Proxy = typeof(Vector3Proxy))]
public partial class Test
{
    public required Vector2 v2;
    public required Vector2[][] vertices;

    [UseProxy(ForType = typeof(Vector2), SerializeProxy = typeof(Vector2Proxy2))]
    public required Vector2[][] weights;

    [UseProxy(ForType = typeof(Vector2), DeserializeProxy = typeof(Vector2Proxy2))]
    public required Dictionary<Vector3, Vector2[][]> points;
}
