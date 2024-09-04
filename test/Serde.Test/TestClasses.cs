// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xunit;

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