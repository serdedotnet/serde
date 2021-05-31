
using Microsoft.CodeAnalysis;
using SerdeGenerator;

namespace Serde
{
    internal enum DiagId
    {
        ERR_DoesntImplementISerialize = 1,
        ERR_TypeNotPartial = 2
    }
}

namespace Serde
{
    using static Serde.DiagId;

    internal static class Diagnostics
    {
        public static string GetName(this DiagId id) => id switch
        {
            ERR_DoesntImplementISerialize => nameof(ERR_DoesntImplementISerialize),
            ERR_TypeNotPartial => nameof(ERR_TypeNotPartial),
            _ => throw ExceptionUtilities.Unreachable
        };

        public static Diagnostic CreateDiagnostic(DiagId id, Location location, params object[] args)
        {
            var name = id.GetName();
            var severity = name.StartsWith("ERR") ? DiagnosticSeverity.Error : DiagnosticSeverity.Warning;
            return Diagnostic.Create(
                name,
                category: "Serde",
                string.Format(Resources.ResourceManager.GetString(name), args),
                severity: severity,
                defaultSeverity: severity,
                isEnabledByDefault: true,
                warningLevel: 0,
                location: location);
        }
    }
}