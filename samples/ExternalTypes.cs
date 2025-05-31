
using System;
using Serde;
using Serde.Json;

namespace ExternalTypesSample;

// Pretend that Response is an external type that we can't modify directly
public record Response(string ResponseType, string Body);

// Proxy for the Response type.
// We use the [GenerateSerde] attribute with the `ForType` parameter to control
// generation for the proxy type. Since the ResponseProxy type is empty, Serde
// will assume that we want to automatically use all the public properties and
// fields of the Response type, with no further customization.
[GenerateSerde(ForType = typeof(Response))]
partial class ResponseProxy;

public class Sample
{
    public static void Run()
    {
        var resp = new Response(ResponseType: "success", Body: "hello, world");
        Console.WriteLine($"Original version: {resp}");

        // Serialize the Response to a JSON string
        // In addition to the Response type, we also have to pass in the proxy type
        var json = JsonSerializer.Serialize<Response, ResponseProxy>(resp);
        Console.WriteLine($"Serialized version: {json}");

        // Deserialize the JSON string back to a Response object
        var deResp = JsonSerializer.Deserialize<Response, ResponseProxy>(json);
        Utils.AssertEq(resp, deResp);
        Console.WriteLine($"Deserialized version: {deResp}");
    }
}