
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
        ERR_MissingPrimaryCtor = 4,
        ERR_CantFindNestedWrapper = 5,
        ERR_WrapperDoesntImplementInterface = 6,
        ERR_CantImplementAbstract = 7,
        ERR_CantFindTypeParameter = 8,
        ERR_CtorParamMismatch = 9,
        ERR_MissingExplicitConversion = 10,
        ERR_MissingReverseConversion = 11,
        ERR_AsTypeNoConversion = 12,
        ERR_AsTypeNotNamed = 13,
        ERR_AsTypeOnEnum = 14,
        ERR_AsTypeWithOption = 15,
        ERR_ForTypeUnsupported = 16,
        ERR_WithTypeUnsupported = 17,
    }

    internal static class Diagnostics
    {
        public static string GetName(this DiagId id) => id switch
        {
            ERR_DoesntImplementInterface => nameof(ERR_DoesntImplementInterface),
            ERR_TypeNotPartial => nameof(ERR_TypeNotPartial),
            ERR_CantWrapSpecialType => nameof(ERR_CantWrapSpecialType),
            ERR_MissingPrimaryCtor => nameof(ERR_MissingPrimaryCtor),
            ERR_CantFindNestedWrapper => nameof(ERR_CantFindNestedWrapper),
            ERR_WrapperDoesntImplementInterface => nameof(ERR_WrapperDoesntImplementInterface),
            ERR_CantImplementAbstract => nameof(ERR_CantImplementAbstract),
            ERR_CantFindTypeParameter => nameof(ERR_CantFindTypeParameter),
            ERR_CtorParamMismatch => nameof(ERR_CtorParamMismatch),
            ERR_MissingExplicitConversion => nameof(ERR_MissingExplicitConversion),
            ERR_MissingReverseConversion => nameof(ERR_MissingReverseConversion),
            ERR_AsTypeNoConversion => nameof(ERR_AsTypeNoConversion),
            ERR_AsTypeNotNamed => nameof(ERR_AsTypeNotNamed),
            ERR_AsTypeOnEnum => nameof(ERR_AsTypeOnEnum),
            ERR_AsTypeWithOption => nameof(ERR_AsTypeWithOption),
            ERR_ForTypeUnsupported => nameof(ERR_ForTypeUnsupported),
            ERR_WithTypeUnsupported => nameof(ERR_WithTypeUnsupported),
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