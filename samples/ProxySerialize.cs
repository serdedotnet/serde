
using System;
using System.Threading.Tasks;
using Serde;
using Serde.Json;

namespace ProxySample;

// Serde implementation for the System.Version type, which we can't modify directly

// Proxy for the Version type. It should have the same structure as the Version type.
// We use the [GenerateSerde] attribute to control generation for the proxy type. We
// will then convert between the proxy and the original type in an attached Serde object.
[GenerateSerde]
partial record VersionProxy(int Major, int Minor, int Build, int Revision);

partial record VersionProxy
    // Specify that this proxy is a provider for the VersionSerdeObj Serde object,
    // which is used to serialize and deserialize the Version type.
    : ISerdeProvider<VersionProxy, VersionSerdeObj, Version>
{
    public static VersionSerdeObj Instance { get; } = new VersionSerdeObj();
}

// Serde object for the Version type, which uses the VersionProxy
sealed partial class VersionSerdeObj : ISerde<Version>
{
    public ISerdeInfo SerdeInfo => StringProxy.SerdeInfo;

    public void Serialize(Version version, ISerializer serializer)
    {
        var proxy = new VersionProxy(version.Major, version.Minor, version.Build, version.Revision);
        serializer.WriteValue(proxy);
    }

    public async ValueTask<Version> Deserialize(IDeserializer deserializer)
    {
        var proxy = await deserializer.ReadValue<VersionProxy>();
        return new Version(proxy.Major, proxy.Major, proxy.Build, proxy.Revision);
    }
}

public class ProxySerializeSample
{
    public static void Run()
    {
        var version = new Version(1, 2, 3, 4);
        Console.WriteLine($"Original version: {version}");

        // Serialize the version to a JSON string
        // Unlike in regular custom serialization, we can't attach the serde object to
        // the Version type directly, so we have to pass it in explicitly.
        var json = JsonSerializer.Serialize<Version, VersionProxy>(version);
        Console.WriteLine($"Serialized version: {json}");

        // Deserialize the JSON string back to a Version object
        var deserializedVersion = JsonSerializer.Deserialize<Version, VersionProxy>(json);
        Utils.AssertEq(version, deserializedVersion);
        Console.WriteLine($"Deserialized version: {deserializedVersion}");
    }
}