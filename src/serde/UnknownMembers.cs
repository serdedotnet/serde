using System;
using System.Collections.Generic;
using System.Text;

namespace Serde;

/// <summary>
/// Helpers for a member marked with <see cref="SerdeMemberOptions.CaptureUnknownMembers"/>, which
/// holds the members of the enclosing type that are not in its <see cref="ISerdeInfo"/>. Used by
/// generated code.
/// </summary>
public static class UnknownMembers
{
    /// <summary>
    /// Writes each entry of <paramref name="members"/> as a member of the type described by
    /// <paramref name="typeInfo"/>, named by its key. A key that names a member of the type throws
    /// <see cref="InvalidOperationException"/>, since that member would be written twice.
    /// </summary>
    public static void Serialize<TValue, TProvider>(
        Dictionary<string, TValue> members,
        ITypeSerializer typeSerializer,
        ISerdeInfo typeInfo
    )
        where TProvider : ISerializeProvider<TValue>
    {
        var serialize = TProvider.Instance;
        foreach (var (name, value) in members)
        {
            if (TryGetIndex(typeInfo, name) != ITypeDeserializer.IndexNotFound)
            {
                throw new InvalidOperationException(
                    $"Unknown member '{name}' collides with a member of '{typeInfo.Name}'."
                );
            }
            var serializer = typeSerializer.WriteFieldStart(typeInfo, name);
            serialize.Serialize(value, serializer);
            typeSerializer.WriteFieldEnd(typeInfo, name, serializer);
        }
    }

    private static int TryGetIndex(ISerdeInfo info, string name)
    {
        var maxBytes = Encoding.UTF8.GetMaxByteCount(name.Length);
        Span<byte> buffer = maxBytes <= 256 ? stackalloc byte[256] : new byte[maxBytes];
        var written = Encoding.UTF8.GetBytes(name, buffer);
        return info.TryGetIndex(buffer[..written]);
    }
}
