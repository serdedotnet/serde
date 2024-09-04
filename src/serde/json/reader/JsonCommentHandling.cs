// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Serde.Json
{
    /// <summary>
    /// This enum defines the various ways the <see cref="Utf8JsonReader_Old"/> can deal with comments.
    /// </summary>
    internal enum JsonCommentHandling : byte
    {
        /// <summary>
        /// By default, do no allow comments within the JSON input.
        /// Comments are treated as invalid JSON if found and a
        /// <see cref="JsonException_Old"/> is thrown.
        /// </summary>
        Disallow = 0,
        /// <summary>
        /// Allow comments within the JSON input and ignore them.
        /// The <see cref="Utf8JsonReader_Old"/> will behave as if no comments were present.
        /// </summary>
        Skip = 1,
        /// <summary>
        /// Allow comments within the JSON input and treat them as valid tokens.
        /// While reading, the caller will be able to access the comment values.
        /// </summary>
        Allow = 2,
    }
}
