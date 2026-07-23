using AlphaX.Sheets;

namespace AlphaX.WPF.Sheets.UI.Managers
{
    /// <summary>
    /// Contract for managing clipboard operations (copy and paste) in AlphaXSpread.
    /// </summary>
    public interface IClipboardManager
    {
        /// <summary>
        /// Copies current active sheet selection to clipboard.
        /// </summary>
        void Copy();

        /// <summary>
        /// Copies current selection of the specified sheet view to clipboard.
        /// </summary>
        /// <param name="sheetView"></param>
        void Copy(IAlphaXSheetView sheetView);

        /// <summary>
        /// Copies the specified cell range of the specified sheet view to clipboard.
        /// </summary>
        /// <param name="sheetView"></param>
        /// <param name="range"></param>
        void Copy(IAlphaXSheetView sheetView, CellRange range);

        /// <summary>
        /// Pastes data from clipboard to active sheet view.
        /// </summary>
        void Paste();

        /// <summary>
        /// Pastes data from clipboard to the specified sheet view.
        /// </summary>
        /// <param name="sheetView"></param>
        void Paste(IAlphaXSheetView sheetView);

        /// <summary>
        /// Determines whether a copy operation can be performed.
        /// </summary>
        /// <param name="sheetView"></param>
        /// <returns></returns>
        bool CanCopy(IAlphaXSheetView sheetView = null);

        /// <summary>
        /// Determines whether a paste operation can be performed.
        /// </summary>
        /// <param name="sheetView"></param>
        /// <returns></returns>
        bool CanPaste(IAlphaXSheetView sheetView = null);
    }
}
