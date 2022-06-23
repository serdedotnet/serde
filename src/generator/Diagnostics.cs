
using Microsoft.CodeAnalysis;
using SerdeGenerator;

namespace Serde
{
    internal enum DiagId
    {
        ERR_DoesntImplementInterface = 1,
        ERR_TypeNotPartial = 2,
        ERR_CantWrapSpecialType = 3
    }
}

namespace Serde
{
    using static Serde.DiagId;

    internal static class Diagnostics
    {
        public static string GetName(this DiagId id) => id switch
        {
            ERR_DoesntImplementInterface => nameof(ERR_DoesntImplementInterface),
            ERR_TypeNotPartial => nameof(ERR_TypeNotPartial),
            ERR_CantWrapSpecialType => nameof(ERR_CantWrapSpecialType),
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