
using System;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace Serde;

public sealed class SourceBuilder : IComparable<SourceBuilder>
{
    public static readonly Encoding UTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    private string _currentIndentWhitespace = "";
    private StringBuilder _stringBuilder;

    public SourceBuilder(string s)
    {
        _stringBuilder = new StringBuilder(s);
    }

    public SourceBuilder(SourceBuilderStringHandler s)
    {
        _currentIndentWhitespace = s._currentIndentWhitespace;
        _stringBuilder = s._stringBuilder;
    }

    public SourceBuilder()
    {
        _stringBuilder = new StringBuilder();
    }

    /// <summary>
    /// Removes trailing whitespace from every line and replace all newlines with
    /// Environment.NewLine.
    /// </summary>
    private void Normalize()
    {
        _stringBuilder.Replace("\r\n", "\n");

        // Remove trailing whitespace from every line
        int wsStart;
        for (int i = 0; i < _stringBuilder.Length; i++)
        {
            if (_stringBuilder[i] is '\n')
            {
                wsStart = i - 1;
                while (wsStart >= 0 && (_stringBuilder[wsStart] is ' ' or '\t'))
                {
                    wsStart--;
                }
                wsStart++; // Move back to first whitespace
                if (wsStart < i)
                {
                    int len = i - wsStart;
                    _stringBuilder.Remove(wsStart, len);
                    i -= len;
                }
            }
        }

        _stringBuilder.Replace("\n", Utilities.NewLine);
    }

    public override string ToString()
    {
        Normalize();
        return _stringBuilder.ToString();
    }

    public SourceText ToSourceText(SourceHashAlgorithm checksumAlgorithm = SourceHashAlgorithm.Sha1)
    {
        Normalize();
        return new StringBuilderText(_stringBuilder, SourceBuilder.UTF8Encoding, checksumAlgorithm);
    }

    public void Append(
        [InterpolatedStringHandlerArgument("")]
        SourceBuilderStringHandler s)
    {
        _currentIndentWhitespace = s._currentIndentWhitespace;
        // No need to copy the StringBuilder as it was passed in by reference
    }

    public int CompareTo(SourceBuilder other)
    {
        var lenCmp = _stringBuilder.Length.CompareTo(other._stringBuilder.Length);
        if (lenCmp != 0)
        {
            return lenCmp;
        }
        for (int i = 0; i < _stringBuilder.Length; i++)
        {
            var cCmp = _stringBuilder[i].CompareTo(other._stringBuilder[i]);
            if (cCmp != 0)
            {
                return cCmp;
            }
        }
        return 0;
    }

    [InterpolatedStringHandler]
    public ref struct SourceBuilderStringHandler
    {
        internal readonly StringBuilder _stringBuilder;
        internal string _currentIndentWhitespace;

        public SourceBuilderStringHandler(int literalLength, int formattedCount)
        {
            _stringBuilder = new StringBuilder(literalLength);
            _currentIndentWhitespace = "";
        }

        public SourceBuilderStringHandler(
            int literalLength,
            int formattedCount,
            SourceBuilder sourceBuilder)
        {
            _stringBuilder = sourceBuilder._stringBuilder;
            _currentIndentWhitespace = sourceBuilder._currentIndentWhitespace;
        }

        public void AppendLiteral(string s)
        {
            _stringBuilder.Append(s);

            int last = s.LastIndexOf('\n');
            if (last == -1)
            {
                return;
            }

            var remaining = s.AsSpan(last + 1);
            foreach (var c in remaining)
            {
                if (c is not (' ' or '\t'))
                {
                    return;
                }
            }

            _currentIndentWhitespace = remaining.ToString();
        }

        public void AppendFormatted<T>(T value)
        {
            var str = value?.ToString();
            if (str is null)
            {
                _stringBuilder.Append(str);
                return;
            }

            int start = 0;
            int nl;
            while (start < str.Length)
            {
                nl = str.IndexOf('\n', start);
                if (nl == -1)
                {
                    nl = str.Length;
                }
                // Skip blank lines
                while (nl < str.Length && (str[nl] == '\n' || str[nl] == '\r'))
                {
                    nl++;
                }
                if (start > 0)
                {
                    _stringBuilder.Append(_currentIndentWhitespace);
                }
                _stringBuilder.Append(str, start, nl - start);
                start = nl;
            }
        }
    }

    /// <summary>
    /// Implementation of <see cref="SourceText"/> based on a <see cref="StringBuilder"/> input.
    /// Copied from https://github.com/dotnet/roslyn/blob/ccbc0926d5973dac719f3108371229bc27caeff9/src/Compilers/Core/Portable/Text/StringBuilderText.cs
    /// </summary>
    internal sealed partial class StringBuilderText : SourceText
    {
        /// <summary>
        /// Underlying string on which this SourceText instance is based
        /// </summary>
        private readonly StringBuilder _builder;

        private readonly Encoding? _encodingOpt;

        public StringBuilderText(StringBuilder builder, Encoding? encodingOpt, SourceHashAlgorithm checksumAlgorithm)
             : base(checksumAlgorithm: checksumAlgorithm)
        {
            _builder = builder;
            _encodingOpt = encodingOpt;
        }

        public override Encoding? Encoding
        {
            get { return _encodingOpt; }
        }

        /// <summary>
        /// The length of the text represented by <see cref="StringBuilderText"/>.
        /// </summary>
        public override int Length
        {
            get { return _builder.Length; }
        }

        /// <summary>
        /// Returns a character at given position.
        /// </summary>
        /// <param name="position">The position to get the character from.</param>
        /// <returns>The character.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When position is negative or
        /// greater than <see cref="Length"/>.</exception>
        public override char this[int position]
        {
            get
            {
                if (position < 0 || position >= _builder.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(position));
                }

                return _builder[position];
            }
        }

        /// <summary>
        /// Provides a string representation of the StringBuilderText located within given span.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">When given span is outside of the text range.</exception>
        public override string ToString(TextSpan span)
        {
            if (span.End > _builder.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(span));
            }

            return _builder.ToString(span.Start, span.Length);
        }

        public override void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            _builder.CopyTo(sourceIndex, destination, destinationIndex, count);
        }
    }
}