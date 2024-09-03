// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.Serialization;

namespace Serde.Json
{
    // This class exists because the serializer needs to catch reader-originated exceptions in order to throw JsonException which has Path information.
    [Serializable]
    internal sealed class JsonReaderException : JsonException_Old
    {
        public JsonReaderException(string message, long lineNumber, long bytePositionInLine) : base(message, path: null, lineNumber, bytePositionInLine)
        {
        }
    }
}
