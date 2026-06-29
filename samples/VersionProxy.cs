using System;
using Serde;
using Serde.Json;

namespace VersionProxySample;

// System.Version is an external BCL type: we can't add [GenerateSerde] to it, and it has no
// parameterless constructor, so an *empty* proxy won't work. Instead we write a *non-empty*
// proxy that mirrors the data we want on the wire and provide explicit conversion operators
// in both directions. Serde generates serialization against the proxy's own fields and uses
// the operators to convert to and from Version at the call site:
//   Version -> VersionProxy  (used when serializing)
//   VersionProxy -> Version  (used when deserializing)
[GenerateSerde(ForType = typeof(Version))]
partial struct VersionProxy
{
    public int Major;
    public int Minor;
    public int Build;
    public int Revision;

    public static explicit operator Version(VersionProxy p) =>
        new Version(p.Major, p.Minor, p.Build, p.Revision);

    public static explicit operator VersionProxy(Version v) =>
        new VersionProxy
        {
            Major = v.Major,
            Minor = v.Minor,
            Build = v.Build,
            Revision = v.Revision,
        };
}

public static class Sample
{
    public static void Run()
    {
        var version = new Version(1, 2, 3, 4);
        Console.WriteLine($"Original version: {version}");

        // Serialize the Version, going through the Version -> VersionProxy conversion.
        // produces: {"major":1,"minor":2,"build":3,"revision":4}
        var json = JsonSerializer.Serialize<Version, VersionProxy>(version);
        Console.WriteLine($"Serialized version: {json}");

        // Deserialize, going through the VersionProxy -> Version conversion.
        var deVersion = JsonSerializer.Deserialize<Version, VersionProxy>(json);
        Utils.AssertEq(version, deVersion);
        Console.WriteLine($"Deserialized version: {deVersion}");
    }
}
