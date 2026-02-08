
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Serde
{
    /// <summary>
    /// Represents a member with no arguments, namely a field or property
    /// </summary>
    internal readonly struct DataMemberSymbol
    {
        private readonly TypeOptions _typeOptions;
        private readonly MemberOptions _memberOptions;

        /// <summary>
        /// The field or property may contain null.
        /// </summary>
        public bool IsNullable { get; }
        public ISymbol Symbol { get; }

        public DataMemberSymbol(
            ISymbol symbol,
            TypeOptions typeOptions,
            MemberOptions memberOptions)
        {
            Debug.Assert(symbol is
                IFieldSymbol or
                IPropertySymbol { Parameters.Length: 0 });
            Symbol = symbol;
            _typeOptions = typeOptions;
            _memberOptions = memberOptions;
            this.IsNullable = IsNullable(symbol);

            // Assumes that the symbol is in a nullable-enabled context, and lack of annotation
            // means not-nullable
            static bool IsNullable(ISymbol symbol)
            {
                var type = GetType(symbol);
                if (type.NullableAnnotation == NullableAnnotation.Annotated ||
                    type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
                {
                    return true;
                }

                if (type.IsValueType)
                {
                    return false;
                }

                if (type is ITypeParameterSymbol param)
                {
                    if (param.HasNotNullConstraint)
                    {
                        return false;
                    }
                    foreach (var ann in param.ConstraintNullableAnnotations)
                    {
                        if (ann == NullableAnnotation.Annotated)
                        {
                            return true;
                        }
                    }
                    if (param.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.Annotated)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public ITypeSymbol Type => GetType(Symbol);

        private static ITypeSymbol GetType(ISymbol symbol) => symbol switch
        {
            IFieldSymbol f => f.Type,
            IPropertySymbol p => p.Type,
            _ => throw ExceptionUtilities.Unreachable
        };

        public NullableAnnotation NullableAnnotation => Symbol switch
        {
            IFieldSymbol f => f.NullableAnnotation,
            IPropertySymbol p => p.NullableAnnotation,
            _ => throw ExceptionUtilities.Unreachable
        };

        public ImmutableArray<Location> Locations => Symbol.Locations;

        public string Name => Symbol.Name;

        public bool SkipDeserialize => _memberOptions.SkipDeserialize;

        public bool SkipSerialize => _memberOptions.SkipSerialize;

        public bool? ThrowIfMissing => _memberOptions.ThrowIfMissing;

        public bool SerializeNull => _memberOptions.SerializeNull ?? _typeOptions.SerializeNull;

        public ImmutableArray<AttributeData> Attributes => Symbol.GetAttributes();

        /// <summary>
        /// Retrieves the name of the member after formatting options are applied.
        /// </summary>
        public string GetFormattedName()
        {
            if (_memberOptions.Rename is { } renamed)
            {
                return renamed;
            }
            if (_typeOptions.MemberFormat == MemberFormat.None)
            {
                return Name;
            }
            var parts = ParseMemberName(Name);
            switch (_typeOptions.MemberFormat)
            {
                case MemberFormat.CamelCase:
                    {
                        var builder = new StringBuilder();
                        bool first = true;
                        foreach (var part in parts)
                        {
                            if (first)
                            {
                                builder.Append(char.ToLowerInvariant(part[0]));
                                first = false;
                            }
                            else
                            {
                                builder.Append(char.ToUpperInvariant(part[0]));
                            }
                            builder.Append(part.Substring(1).ToLowerInvariant());
                        }
                        return builder.ToString();
                    }
                case MemberFormat.PascalCase:
                    {
                        var builder = new StringBuilder();
                        foreach (var part in parts)
                        {
                            builder.Append(char.ToUpperInvariant(part[0]));
                            builder.Append(part.Substring(1).ToLowerInvariant());
                        }
                        return builder.ToString();
                    }
                case MemberFormat.KebabCase:
                    return string.Join("-", parts.Select(s => s.ToLowerInvariant()));

                default:
                    throw new InvalidOperationException("Invalid member format: " + _typeOptions.MemberFormat);
            }
        }

        private static ImmutableArray<string> ParseMemberName(string name)
        {
            var resultBuilder = ImmutableArray.CreateBuilder<string>();
            var wordBuilder = new StringBuilder();
            foreach (var c in name)
            {
                if (c == '_')
                {
                    AddWordAndClear();
                    continue;
                }
                if (char.IsUpper(c))
                {
                    AddWordAndClear();
                }
                wordBuilder.Append(c);
            }
            AddWordAndClear();
            return resultBuilder.ToImmutable();

            void AddWordAndClear()
            {
                if (wordBuilder.Length > 0)
                {
                    resultBuilder.Add(wordBuilder.ToString());
                    wordBuilder.Clear();
                }
            }
        }
    }
}
