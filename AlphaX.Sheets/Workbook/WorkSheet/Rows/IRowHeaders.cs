namespace AlphaX.Sheets
{
    public interface IRowHeaders : IHeaders
    {
        /// <summary>
        /// Gets or sets the header column count.
        /// </summary>
        int ColumnCount { get; set; }
        /// <summary>
        /// Gets or sets the default column width.
        /// </summary>
        int DefaultColumnWidth { get; set; }
        /// <summary>
        /// Gets the row headers width.
        /// </summary>
        double Width { get; }
    }
}
