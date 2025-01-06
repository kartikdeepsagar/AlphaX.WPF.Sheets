using AlphaX.Sheets;
using AlphaX.WPF.Sheets.UI;
using System.Windows;

namespace AlphaX.WPF.Sheets
{
    /// <summary>
    /// Represents the view for a worksheet
    /// </summary>
    public interface IAlphaXSheetView
    {
        /// <summary>
        /// Gets or sets the grid line visibility.
        /// </summary>
        GridLineVisibility GridLineVisibility { get; set; }
        /// <summary>
        /// Gets or sets headers visibility
        /// </summary>
        HeadersVisibility HeadersVisibility { get; set; }
        /// <summary>
        /// Gets or sets selection mode.
        /// </summary>
        SelectionMode SelectionMode { get; set; }
        /// <summary>
        /// Gets the sheet view port.
        /// </summary>
        IViewPort ViewPort { get; }
        /// <summary>
        /// Gets the current scroll position.
        /// </summary>
        Point ScrollPosition { get; }
        /// <summary>
        /// Gets or sets the mouse wheel scroll direction.
        /// </summary>
        MouseWheelScrollDirection MouseWheelScrollDirection { get; set; }
        /// <summary>
        /// Gets the owner spread.
        /// </summary>
        AlphaXSpread Spread { get; }
        /// <summary>
        /// Gets the active row.
        /// </summary>
        int ActiveRow { get; }
        /// <summary>
        /// Gets the active column.
        /// </summary>
        int ActiveColumn { get; }
        /// <summary>
        /// Gets the current selection.
        /// </summary>
        CellRange Selection { get; }
        /// <summary>
        /// Gets the underlying worksheet for this view.
        /// </summary>
        WorkSheet WorkSheet { get; }
        /// <summary>
        /// Invalidates the view.
        /// </summary>
        /// <param name="rowHeaders"></param>
        /// <param name="columnHeaders"></param>
        /// <param name="cells"></param>
        /// <param name="topLeft"></param>
        void Invalidate(bool rowHeaders = true, bool columnHeaders = true, bool cells = true, bool topLeft = true);
        /// <summary>
        /// Invalidates provided cell range
        /// </summary>
        /// <param name="range"></param>
        void InvalidateCellRange(CellRange range);
        /// <summary>
        /// Invalidates provided cell range.
        /// </summary>
        /// <param name="topRow"></param>
        /// <param name="leftCol"></param>
        /// <param name="bottomRow"></param>
        /// <param name="rightCol"></param>
        void InvalidateCellRange(int topRow, int leftCol, int bottomRow, int rightCol);
        /// <summary>
        /// Copies current selection to clipboard.
        /// </summary>
        void CopyToClipboard();
        /// <summary>
        /// Pastes data from clipboard to sheet.
        /// </summary>
        void PasteFromClipboard();
        /// <summary>
        /// Copies the provided cell range to clipboard.
        /// </summary>
        /// <param name="range"></param>
        void CopyToClipboard(CellRange range);
        /// <summary>
        /// Horizontally scrolls the sheet.
        /// </summary>
        /// <param name="offset"></param>
        void ScrollToHorizontalOffset(double offset);
        /// <summary>
        ///  Vertically scrolls the sheet.
        /// </summary>
        /// <param name="offset"></param>
        void ScrollToVerticalOffset(double offset);
        /// <summary>
        /// Autosize column.
        /// </summary>
        /// <param name="column"></param>
        void AutoSizeColumn(int column);
    }
}
