
using System;

namespace Serde.Json;

public sealed class JsonException : DeserializeException
{
    internal JsonException(string message) : base(message)
    {
    }
}