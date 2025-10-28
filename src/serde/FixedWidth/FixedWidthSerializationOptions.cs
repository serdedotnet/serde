namespace Serde.FixedWidth
{
    /// <summary>
    /// Gets options for configuring the Serde object.
    /// </summary>
    public sealed class FixedWidthSerializationOptions
    {
        public static FixedWidthSerializationOptions Default => new();

        /// <summary>
        /// Gets a value indicating how to handle field overflows, i.e. when the
        /// field value is longer than the field length.
        /// </summary>
        public FieldOverflowHandling FieldOverflowHandling { get; init; } = FieldOverflowHandling.Throw;
    }
}
