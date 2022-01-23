
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Serde
{
    /// <summary>
    /// Represents a member with no arguments, namely a field or property
    /// </summary>
    internal readonly struct DataMemberSymbol
    {
        private readonly ISymbol _symbol;
        private readonly MemberFormat _format;

        public DataMemberSymbol(ISymbol symbol, MemberFormat format)
        {
            Debug.Assert(symbol is
                IFieldSymbol or
                IPropertySymbol { Parameters: { Length: 0 }});
            _symbol = symbol;
            _format = format;
        }

        public ISymbol Symbol => _symbol;

        public ITypeSymbol Type => _symbol switch
        {
            IFieldSymbol f => f.Type,
            IPropertySymbol p => p.Type,
            _ => throw ExceptionUtilities.Unreachable
        };

        public NullableAnnotation NullableAnnotation => _symbol switch
        {
            IFieldSymbol f => f.NullableAnnotation,
            IPropertySymbol p => p.NullableAnnotation,
            _ => throw ExceptionUtilities.Unreachable
        };

        public ImmutableArray<Location> Locations => _symbol.Locations;

        public string Name => _symbol.Name;

        public string GetFormattedName()
        {
            if (_format == MemberFormat.None)
            {
                return Name;
            }
            var parts = ParseMemberName(Name);
            var builder = new StringBuilder();
            switch (_format)
            {
                case MemberFormat.CamelCase:
                    {
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
                    }
                    break;
                case MemberFormat.PascalCase:
                    {
                        foreach (var part in parts)
                        {
                            builder.Append(char.ToUpperInvariant(part[0]));
                            builder.Append(part.Substring(1).ToLowerInvariant());
                        }
                    }
                    break;

                default:
                    return Name;
            }
            return builder.ToString();
        }

        private static ImmutableArray<string> ParseMemberName(string name)
        {
            var resultBuilder = ImmutableArray.CreateBuilder<string>();
            var wordBuilder = new StringBuilder();
            bool wasLowercase = false;
            foreach (var c in name)
            {
                if (c == '_')
                {
                    AddWordAndClear();
                    continue;
                }
                if (char.IsUpper(c))
                {
                    if (wasLowercase)
                    {
                        AddWordAndClear();
                    }
                }
                else
                {
                    wasLowercase = true;
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

        internal MemberOptions GetMemberOptions()
        {
            var options = new MemberOptions();
            foreach (var attr in _symbol.GetAttributes())
            {
                var attrClass = attr.AttributeClass;
                if (attrClass is null)
                {
                    continue;
                }
                if (WellKnownTypes.IsWellKnownAttribute(attrClass, WellKnownAttribute.SerdeMemberOptions))
                {
                    foreach (var named in attr.NamedArguments)
                    {
                        var value = named.Value.Value!;
                        switch (named)
                        {
                            case {
                                Key: nameof(MemberOptions.NullIfMissing),
                                Value: {
                                    Kind: TypedConstantKind.Primitive,
                                    Type: { SpecialType: SpecialType.System_Boolean }
                                } }:
                                options = options with {
                                    NullIfMissing = (bool)value
                                };
                                break;
                        }
                    }
                    break;
                }
            }
            return options;
        }
    }
}
