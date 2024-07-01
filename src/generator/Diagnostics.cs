
using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using SerdeGenerator;
using StaticCs;
using static Serde.DiagId;

namespace Serde
{
    [Closed]
    internal enum DiagId
    {
        ERR_DoesntImplementInterface = 1,
        ERR_TypeNotPartial = 2,
        ERR_CantWrapSpecialType = 3,
        ERR_CantFindConstructorSignature = 4,
        ERR_CantFindNestedWrapper = 5,
        ERR_WrapperDoesntImplementInterface = 6,
    }

    internal static class Diagnostics
    {
        public static string GetName(this DiagId id) => id switch
        {
            ERR_DoesntImplementInterface => nameof(ERR_DoesntImplementInterface),
            ERR_TypeNotPartial => nameof(ERR_TypeNotPartial),
            ERR_CantWrapSpecialType => nameof(ERR_CantWrapSpecialType),
            ERR_CantFindConstructorSignature => nameof(ERR_CantFindConstructorSignature),
            ERR_CantFindNestedWrapper => nameof(ERR_CantFindNestedWrapper),
            ERR_WrapperDoesntImplementInterface => nameof(ERR_WrapperDoesntImplementInterface),
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