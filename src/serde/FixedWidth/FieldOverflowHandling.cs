namespace Serde.FixedWidth
{
    /// <summary>
    /// Enumerates the supported options for field values that are too long.
    /// </summary>
    public enum FieldOverflowHandling
    {
        /// <summary>
        /// Indicates that an exception should be thrown if the field value is longer than the field length.
        /// </summary>
        Throw = 0,

        /// <summary>
        /// Indicates that the field value should be truncated if the value is longer than the field length.
        /// </summary>
        Truncate = 1,
    }
}
