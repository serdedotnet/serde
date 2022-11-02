// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization.Metadata;

namespace System.Text.Json
{
    internal static partial class ThrowHelper
    {
        // If the exception source is this value, the serializer will re-throw as JsonException.
        public const string ExceptionSourceValueToRethrowAsJsonException = "System.Text.Json.Rethrowable";

        [DoesNotReturn]
        public static void ThrowArgumentNullException(string parameterName)
        {
            throw new ArgumentNullException(parameterName);
        }

        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException_MaxDepthMustBePositive(string parameterName)
        {
            throw GetArgumentOutOfRangeException(parameterName, "Depth must be positive.");
        }

        private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(string parameterName, string message)
        {
            return new ArgumentOutOfRangeException(parameterName, message);
        }

        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException_CommentEnumMustBeInRange(string parameterName)
        {
            throw GetArgumentOutOfRangeException(parameterName, "Comment handling must be valid");
        }

        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException_ArrayIndexNegative(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, "Array index must be greater than or equal to zero.");
        }

        [DoesNotReturn]
        public static void ThrowArgumentException_ArrayTooSmall(string paramName)
        {
            throw new ArgumentException("Array too small", paramName);
        }

        private static ArgumentException GetArgumentException(string message)
        {
            return new ArgumentException(message);
        }

        [DoesNotReturn]
        public static void ThrowArgumentException(string message)
        {
            throw GetArgumentException(message);
        }

        public static InvalidOperationException GetInvalidOperationException_CallFlushFirst(int _buffered)
        {
            return GetInvalidOperationException("Call flush first. Bytes in buffer: " + _buffered);
        }

        [DoesNotReturn]
        public static void ThrowArgumentException_DestinationTooShort()
        {
            throw GetArgumentException("Destination too short");
        }

        [DoesNotReturn]
        public static void ThrowArgumentException_PropertyNameTooLarge(int tokenLength)
        {
            throw GetArgumentException("Property name too large: " + tokenLength);
        }

        [DoesNotReturn]
        public static void ThrowArgumentException_ValueTooLarge(int tokenLength)
        {
            throw GetArgumentException("Value too large");
        }

        [DoesNotReturn]
        public static void ThrowArgumentException_ValueNotSupported()
        {
            throw GetArgumentException("Special number values not supported");
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException_NeedLargerSpan()
        {
            throw GetInvalidOperationException("Failed to get larger span");
        }

        [DoesNotReturn]
        public static void ThrowArgumentException(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            if (propertyName.Length > JsonConstants.MaxUnescapedTokenSize)
            {
                ThrowArgumentException("Property name too large");
            }
            else
            {
                Debug.Assert(value.Length > JsonConstants.MaxUnescapedTokenSize);
                ThrowArgumentException("Value too large");
            }
        }

        [DoesNotReturn]
        public static void ThrowArgumentException(ReadOnlySpan<byte> propertyName, ReadOnlySpan<char> value)
        {
            if (propertyName.Length > JsonConstants.MaxUnescapedTokenSize)
            {
                ThrowArgumentException("Property name too large");
            }
            else
            {
                Debug.Assert(value.Length > JsonConstants.MaxCharacterTokenSize);
                ThrowArgumentException("Value too large");
            }
        }

        [DoesNotReturn]
        public static void ThrowArgumentException(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> value)
        {
            if (propertyName.Length > JsonConstants.MaxCharacterTokenSize)
            {
                ThrowArgumentException("Property name too large");
            }
            else
            {
                Debug.Assert(value.Length > JsonConstants.MaxUnescapedTokenSize);
                ThrowArgumentException("Value too large");
            }
        }

        [DoesNotReturn]
        public static void ThrowArgumentException(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
        {
            if (propertyName.Length > JsonConstants.MaxCharacterTokenSize)
            {
                ThrowArgumentException("Property name too large");
            }
            else
            {
                Debug.Assert(value.Length > JsonConstants.MaxCharacterTokenSize);
                ThrowArgumentException("Value too large");
            }
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationOrArgumentException(ReadOnlySpan<byte> propertyName, int currentDepth, int maxDepth)
        {
            currentDepth &= JsonConstants.RemoveFlagsBitMask;
            if (currentDepth >= maxDepth)
            {
                ThrowInvalidOperationException("Depth too large");
            }
            else
            {
                Debug.Assert(propertyName.Length > JsonConstants.MaxCharacterTokenSize);
                ThrowArgumentException("Property name too large");
            }
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException(int currentDepth, int maxDepth)
        {
            currentDepth &= JsonConstants.RemoveFlagsBitMask;
            Debug.Assert(currentDepth >= maxDepth);
            ThrowInvalidOperationException("Depth too large");
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException(string message)
        {
            throw GetInvalidOperationException(message);
        }

        private static InvalidOperationException GetInvalidOperationException(string message)
        {
            return new InvalidOperationException(message) { Source = ExceptionSourceValueToRethrowAsJsonException };
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException_DepthNonZeroOrEmptyJson(int currentDepth)
        {
            throw GetInvalidOperationException(currentDepth);
        }

        private static InvalidOperationException GetInvalidOperationException(int currentDepth)
        {
            currentDepth &= JsonConstants.RemoveFlagsBitMask;
            if (currentDepth != 0)
            {
                return GetInvalidOperationException("Zero depth at end of JSON.");
            }
            else
            {
                return GetInvalidOperationException("Empty JSON payload.");
            }
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationOrArgumentException(ReadOnlySpan<char> propertyName, int currentDepth, int maxDepth)
        {
            currentDepth &= JsonConstants.RemoveFlagsBitMask;
            if (currentDepth >= maxDepth)
            {
                ThrowInvalidOperationException("Depth too large");
            }
            else
            {
                Debug.Assert(propertyName.Length > JsonConstants.MaxCharacterTokenSize);
                ThrowArgumentException("Property name too large");
            }
        }

        public static InvalidOperationException GetInvalidOperationException_ExpectedArray(JsonTokenType tokenType)
        {
            return GetInvalidOperationException("array", tokenType);
        }

        public static InvalidOperationException GetInvalidOperationException_ExpectedObject(JsonTokenType tokenType)
        {
            return GetInvalidOperationException("object", tokenType);
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException_ExpectedNumber(JsonTokenType tokenType)
        {
            throw GetInvalidOperationException("number", tokenType);
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException_ExpectedBoolean(JsonTokenType tokenType)
        {
            throw GetInvalidOperationException("boolean", tokenType);
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException_ExpectedString(JsonTokenType tokenType)
        {
            throw GetInvalidOperationException("string", tokenType);
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException_ExpectedPropertyName(JsonTokenType tokenType)
        {
            throw GetInvalidOperationException("propertyName", tokenType);
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException_ExpectedStringComparison(JsonTokenType tokenType)
        {
            throw GetInvalidOperationException(tokenType);
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException_ExpectedComment(JsonTokenType tokenType)
        {
            throw GetInvalidOperationException("comment", tokenType);
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException_CannotSkipOnPartial()
        {
            throw GetInvalidOperationException("Cannot skip");
        }

        private static InvalidOperationException GetInvalidOperationException(string message, JsonTokenType tokenType)
        {
            return GetInvalidOperationException("Invalid cast");
        }

        private static InvalidOperationException GetInvalidOperationException(JsonTokenType tokenType)
        {
            return GetInvalidOperationException("Invalid comparison");
        }

        [DoesNotReturn]
        internal static void ThrowJsonElementWrongTypeException(
            JsonTokenType expectedType,
            JsonTokenType actualType)
        {
            throw GetJsonElementWrongTypeException(expectedType.ToValueKind(), actualType.ToValueKind());
        }

        internal static InvalidOperationException GetJsonElementWrongTypeException(
            JsonValueKind expectedType,
            JsonValueKind actualType)
        {
            return GetInvalidOperationException("JsonElement is not of the expected type. Expected: " + expectedType + ". Actual: " + actualType + ".");
        }

        internal static InvalidOperationException GetJsonElementWrongTypeException(
            string expectedTypeName,
            JsonValueKind actualType)
        {
            return GetInvalidOperationException("JsonElement has wrong type");
        }

        [DoesNotReturn]
        public static void ThrowJsonReaderException(ref Utf8JsonReader json, ExceptionResource resource, byte nextByte = default, ReadOnlySpan<byte> bytes = default)
        {
            throw GetJsonReaderException(ref json, resource, nextByte, bytes);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static JsonException GetJsonReaderException(ref Utf8JsonReader json, ExceptionResource resource, byte nextByte, ReadOnlySpan<byte> bytes)
        {
            string message = GetResourceString(ref json, resource, nextByte, JsonHelpers.Utf8GetString(bytes));

            long lineNumber = json.CurrentState._lineNumber;
            long bytePositionInLine = json.CurrentState._bytePositionInLine;

            message += $" LineNumber: {lineNumber} | BytePositionInLine: {bytePositionInLine}.";
            return new JsonReaderException(message, lineNumber, bytePositionInLine);
        }

        private static bool IsPrintable(byte value) => value >= 0x20 && value < 0x7F;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string GetPrintableString(byte value)
        {
            return IsPrintable(value) ? ((char)value).ToString() : $"0x{value:X2}";
        }

        // This function will convert an ExceptionResource enum value to the resource string.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetResourceString(ref Utf8JsonReader json, ExceptionResource resource, byte nextByte, string characters)
        {
            string character = GetPrintableString(nextByte);

            string message = "";
            switch (resource)
            {
                case ExceptionResource.ArrayDepthTooLarge:
                    message = "Array depth too large";
                    break;
                case ExceptionResource.MismatchedObjectArray:
                    message = "Mismatched object array";
                    break;
                case ExceptionResource.TrailingCommaNotAllowedBeforeArrayEnd:
                    message = "Trailing comma not allowed before array end";
                    break;
                case ExceptionResource.TrailingCommaNotAllowedBeforeObjectEnd:
                    message = "Trailing comma not allowed before object end";
                    break;
                case ExceptionResource.EndOfStringNotFound:
                    message = "End of string not found";
                    break;
                case ExceptionResource.RequiredDigitNotFoundAfterSign:
                    message = "Required digit not found after sign";
                    break;
                case ExceptionResource.RequiredDigitNotFoundAfterDecimal:
                    message = "Required digit not found after decimal";
                    break;
                case ExceptionResource.RequiredDigitNotFoundEndOfData:
                    message = "Required digit not found end of data";
                    break;
                case ExceptionResource.ExpectedEndAfterSingleJson:
                    message = "Expected end after single JSON value in input data";
                    break;
                case ExceptionResource.ExpectedEndOfDigitNotFound:
                    message = "Expected end of digit not found";
                    break;
                case ExceptionResource.ExpectedNextDigitEValueNotFound:
                    message = "Expected next digit or E value not found";
                    break;
                case ExceptionResource.ExpectedSeparatorAfterPropertyNameNotFound:
                    message = "Expected separator after property name not found";
                    break;
                case ExceptionResource.ExpectedStartOfPropertyNotFound:
                    message = "Expected start of property name not found";
                    break;
                case ExceptionResource.ExpectedStartOfPropertyOrValueNotFound:
                    message = "Expected start of property name or value not found";
                    break;
                case ExceptionResource.ExpectedStartOfPropertyOrValueAfterComment:
                    message = "Expected start of property name or value after comment not found";
                    break;
                case ExceptionResource.ExpectedStartOfValueNotFound:
                    message = "Expected start of value not found";
                    break;
                case ExceptionResource.ExpectedValueAfterPropertyNameNotFound:
                    message = "Expected value after property name not found";
                    break;
                case ExceptionResource.FoundInvalidCharacter:
                    message = "Found invalid character";
                    break;
                case ExceptionResource.InvalidEndOfJsonNonPrimitive:
                    message = "Invalid end of JSON; expected a primitive value";
                    break;
                case ExceptionResource.ObjectDepthTooLarge:
                    message = "Object depth too large";
                    break;
                case ExceptionResource.ExpectedFalse:
                    message = "Expected 'false'";
                    break;
                case ExceptionResource.ExpectedNull:
                    message = "Expected 'null'";
                    break;
                case ExceptionResource.ExpectedTrue:
                    message = "Expected 'true'";
                    break;
                case ExceptionResource.InvalidCharacterWithinString:
                    message = "Invalid character within string";
                    break;
                case ExceptionResource.InvalidCharacterAfterEscapeWithinString:
                    message = "Invalid character after escape within string";
                    break;
                case ExceptionResource.InvalidHexCharacterWithinString:
                    message = "Invalid hex character within string";
                    break;
                case ExceptionResource.EndOfCommentNotFound:
                    message = "End of comment not found";
                    break;
                case ExceptionResource.ZeroDepthAtEnd:
                    message = "Depth is zero at end";
                    break;
                case ExceptionResource.ExpectedJsonTokens:
                    message = "Expected JSON tokens";
                    break;
                case ExceptionResource.NotEnoughData:
                    message = "Not enough data";
                    break;
                case ExceptionResource.ExpectedOneCompleteToken:
                    message = "Expected one complete JSON value";
                    break;
                case ExceptionResource.InvalidCharacterAtStartOfComment:
                    message = "Invalid character at start of comment";
                    break;
                case ExceptionResource.UnexpectedEndOfDataWhileReadingComment:
                    message = "Unexpected end of data while reading comment";
                    break;
                case ExceptionResource.UnexpectedEndOfLineSeparator:
                    message = "Unexpected end of line separator";
                    break;
                case ExceptionResource.InvalidLeadingZeroInNumber:
                    message = "Invalid leading zero in number";
                    break;
                default:
                    Debug.Fail($"The ExceptionResource enum value: {resource} is not part of the switch. Add the appropriate case and exception message.");
                    break;
            }

            return message;
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException(ExceptionResource resource, int currentDepth, int maxDepth, byte token, JsonTokenType tokenType)
        {
            throw GetInvalidOperationException(resource, currentDepth, maxDepth, token, tokenType);
        }

        [DoesNotReturn]
        public static void ThrowArgumentException_InvalidCommentValue()
        {
            throw new ArgumentException("Cannot write comment with embedded delimiter.");
        }

        [DoesNotReturn]
        public static void ThrowArgumentException_InvalidUTF8(ReadOnlySpan<byte> value)
        {
            var builder = new StringBuilder();

            int printFirst10 = Math.Min(value.Length, 10);

            for (int i = 0; i < printFirst10; i++)
            {
                byte nextByte = value[i];
                if (IsPrintable(nextByte))
                {
                    builder.Append((char)nextByte);
                }
                else
                {
                    builder.Append($"0x{nextByte:X2}");
                }
            }

            if (printFirst10 < value.Length)
            {
                builder.Append("...");
            }

            throw new ArgumentException("Cannot write invalid UTF-8 text: " + builder.ToString());
        }

        [DoesNotReturn]
        public static void ThrowArgumentException_InvalidUTF16(int charAsInt)
        {
            throw new ArgumentException("Cannot write invalid UTF-16 text: " + charAsInt);
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException_ReadInvalidUTF16(int charAsInt)
        {
            throw GetInvalidOperationException("Cannot read invalid UTF16" + $"0x{charAsInt:X2}");
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException_ReadIncompleteUTF16()
        {
            throw GetInvalidOperationException("Incomplete UTF-16 surrogate pair");
        }

        public static InvalidOperationException GetInvalidOperationException_ReadInvalidUTF8(DecoderFallbackException innerException)
        {
            return GetInvalidOperationException("Cannot transcode invalid UTF8", innerException);
        }

        public static ArgumentException GetArgumentException_ReadInvalidUTF16(EncoderFallbackException innerException)
        {
            return new ArgumentException("Cannot transcode invalid UTF16", innerException);
        }

        public static InvalidOperationException GetInvalidOperationException(string message, Exception innerException)
        {
            InvalidOperationException ex = new InvalidOperationException(message, innerException);
            ex.Source = ExceptionSourceValueToRethrowAsJsonException;
            return ex;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static InvalidOperationException GetInvalidOperationException(ExceptionResource resource, int currentDepth, int maxDepth, byte token, JsonTokenType tokenType)
        {
            string message = GetResourceString(resource, currentDepth, maxDepth, token, tokenType);
            InvalidOperationException ex = GetInvalidOperationException(message);
            ex.Source = ExceptionSourceValueToRethrowAsJsonException;
            return ex;
        }

        [DoesNotReturn]
        public static void ThrowOutOfMemoryException(uint capacity)
        {
            throw new OutOfMemoryException($"Buffer maximum size of {capacity} bytes exceeded.");
        }

        // This function will convert an ExceptionResource enum value to the resource string.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetResourceString(ExceptionResource resource, int currentDepth, int maxDepth, byte token, JsonTokenType tokenType)
        {
            string message = "";
            switch (resource)
            {
                case ExceptionResource.MismatchedObjectArray:
                    Debug.Assert(token == JsonConstants.CloseBracket || token == JsonConstants.CloseBrace);
                    message = (tokenType == JsonTokenType.PropertyName) ?
                        "Cannot write after a property name. Expected a value." :
                        "Mismatched object or array.";
                    break;
                case ExceptionResource.DepthTooLarge:
                    message = "The object or array depth is too large to parse.";
                    break;
                case ExceptionResource.CannotStartObjectArrayWithoutProperty:
                    message = "Cannot start an object or array without a property name.";
                    break;
                case ExceptionResource.CannotStartObjectArrayAfterPrimitiveOrClose:
                    message = "Cannot start an object or array within a single JSON value.";
                    break;
                case ExceptionResource.CannotWriteValueWithinObject:
                    message = "Cannot write a JSON value within an object.";
                    break;
                case ExceptionResource.CannotWritePropertyWithinArray:
                    message = (tokenType == JsonTokenType.PropertyName) ?
                        "Cannot write a property after another property. Expected a value." :
                        "Cannot write a property within an array.";
                    break;
                case ExceptionResource.CannotWriteValueAfterPrimitiveOrClose:
                    message = "Cannot write a JSON value after a single JSON value has been written or after close.";
                    break;
                default:
                    Debug.Fail($"The ExceptionResource enum value: {resource} is not part of the switch. Add the appropriate case and exception message.");
                    break;
            }

            return message;
        }

        [DoesNotReturn]
        public static void ThrowFormatException()
        {
            throw new FormatException { Source = ExceptionSourceValueToRethrowAsJsonException };
        }

        public static void ThrowFormatException(NumericType numericType)
        {
            string message = "";

            switch (numericType)
            {
                case NumericType.Byte:
                    message = "The JSON value is not a valid byte.";
                    break;
                case NumericType.SByte:
                    message = "The JSON value is not a valid sbyte.";
                    break;
                case NumericType.Int16:
                    message = "The JSON value is not a valid short.";
                    break;
                case NumericType.Int32:
                    message = "The JSON value is not a valid int.";
                    break;
                case NumericType.Int64:
                    message = "The JSON value is not a valid long.";
                    break;
                case NumericType.UInt16:
                    message = "The JSON value is not a valid ushort.";
                    break;
                case NumericType.UInt32:
                    message = "The JSON value is not a valid uint.";
                    break;
                case NumericType.UInt64:
                    message = "The JSON value is not a valid ulong.";
                    break;
                case NumericType.Single:
                    message = "The JSON value is not a valid float.";
                    break;
                case NumericType.Double:
                    message = "The JSON value is not a valid double.";
                    break;
                case NumericType.Decimal:
                    message = "The JSON value is not a valid decimal.";
                    break;
                default:
                    Debug.Fail($"The NumericType enum value: {numericType} is not part of the switch. Add the appropriate case and exception message.");
                    break;
            }

            throw new FormatException(message) { Source = ExceptionSourceValueToRethrowAsJsonException };
        }

        [DoesNotReturn]
        public static void ThrowFormatException(DataType dataType)
        {
            string message = "";

            switch (dataType)
            {
                case DataType.Boolean:
                case DataType.DateOnly:
                case DataType.DateTime:
                case DataType.DateTimeOffset:
                case DataType.TimeOnly:
                case DataType.TimeSpan:
                case DataType.Guid:
                case DataType.Version:
                    message = "The JSON value could not be converted to " + dataType;
                    break;
                case DataType.Base64String:
                    message = "The JSON value is not a valid Base64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.";
                    break;
                default:
                    Debug.Fail($"The DataType enum value: {dataType} is not part of the switch. Add the appropriate case and exception message.");
                    break;
            }

            throw new FormatException(message) { Source = ExceptionSourceValueToRethrowAsJsonException };
        }

        [DoesNotReturn]
        public static void ThrowInvalidOperationException_ExpectedChar(JsonTokenType tokenType)
        {
            throw GetInvalidOperationException("char", tokenType);
        }

        [DoesNotReturn]
        public static void ThrowObjectDisposedException_Utf8JsonWriter()
        {
            throw new ObjectDisposedException(nameof(Utf8JsonWriter));
        }

        [DoesNotReturn]
        public static void ThrowObjectDisposedException_JsonDocument()
        {
            throw new ObjectDisposedException(nameof(JsonDocument));
        }
    }

    internal enum ExceptionResource
    {
        ArrayDepthTooLarge,
        EndOfCommentNotFound,
        EndOfStringNotFound,
        RequiredDigitNotFoundAfterDecimal,
        RequiredDigitNotFoundAfterSign,
        RequiredDigitNotFoundEndOfData,
        ExpectedEndAfterSingleJson,
        ExpectedEndOfDigitNotFound,
        ExpectedFalse,
        ExpectedNextDigitEValueNotFound,
        ExpectedNull,
        ExpectedSeparatorAfterPropertyNameNotFound,
        ExpectedStartOfPropertyNotFound,
        ExpectedStartOfPropertyOrValueNotFound,
        ExpectedStartOfPropertyOrValueAfterComment,
        ExpectedStartOfValueNotFound,
        ExpectedTrue,
        ExpectedValueAfterPropertyNameNotFound,
        FoundInvalidCharacter,
        InvalidCharacterWithinString,
        InvalidCharacterAfterEscapeWithinString,
        InvalidHexCharacterWithinString,
        InvalidEndOfJsonNonPrimitive,
        MismatchedObjectArray,
        ObjectDepthTooLarge,
        ZeroDepthAtEnd,
        DepthTooLarge,
        CannotStartObjectArrayWithoutProperty,
        CannotStartObjectArrayAfterPrimitiveOrClose,
        CannotWriteValueWithinObject,
        CannotWriteValueAfterPrimitiveOrClose,
        CannotWritePropertyWithinArray,
        ExpectedJsonTokens,
        TrailingCommaNotAllowedBeforeArrayEnd,
        TrailingCommaNotAllowedBeforeObjectEnd,
        InvalidCharacterAtStartOfComment,
        UnexpectedEndOfDataWhileReadingComment,
        UnexpectedEndOfLineSeparator,
        ExpectedOneCompleteToken,
        NotEnoughData,
        InvalidLeadingZeroInNumber,
    }

    internal enum NumericType
    {
        Byte,
        SByte,
        Int16,
        Int32,
        Int64,
        UInt16,
        UInt32,
        UInt64,
        Single,
        Double,
        Decimal
    }

    internal enum DataType
    {
        Boolean,
        DateOnly,
        DateTime,
        DateTimeOffset,
        TimeOnly,
        TimeSpan,
        Base64String,
        Guid,
        Version,
    }
}