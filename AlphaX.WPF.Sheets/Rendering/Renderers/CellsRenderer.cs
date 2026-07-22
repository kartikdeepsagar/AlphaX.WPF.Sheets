using AlphaX.WPF.Sheets.CellTypes;
using AlphaX.WPF.Sheets.UI;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal class CellsRenderer : Renderer
    {
        protected override void OnRender(DrawingContext context, int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            var workSheet = SheetView.WorkSheet;
            var rows = workSheet.Rows;
            var columns = workSheet.Columns;
            var cells = workSheet.Cells;
            var viewport = SheetView.ViewPort.As<ViewPort>();

            double penThickness = SheetView.Spread.GridLinePen.Thickness;
            double halfPenWidth = (penThickness * SheetView.Spread.PixelPerDip) / 2;
            GuidelineSet guidelines = new GuidelineSet();
            context.PushGuidelineSet(guidelines);

            for (int row = topRow; row <= bottomRow; row++)
            {
                var rowHeight = rows.GetRowHeight(row);

                if (rowHeight == 0)
                    continue;

                if (workSheet.FilterProvider.FilteredRows.ContainsKey(row))
                    continue;

                var sheetRow = rows.GetItem(row, false);
                var rowLocation = rows.GetLocation(row);
                var y = rowLocation - viewport.TopRowLocation;

                guidelines.GuidelinesY.Add(y + halfPenWidth);
                guidelines.GuidelinesY.Add(y + rowHeight - penThickness + halfPenWidth);

                for (int col = leftColumn; col <= rightColumn; col++)
                {
                    var columnWidth = columns.GetColumnWidth(col);

                    if (columnWidth == 0)
                        continue;

                    var columnLocation = columns.GetLocation(col);
                    var x = columnLocation - viewport.LeftColumnLocation;

                    if (row == topRow)
                    {
                        guidelines.GuidelinesX.Add(x + halfPenWidth);
                        guidelines.GuidelinesX.Add(x + columnWidth - penThickness + halfPenWidth);
                    }

                    var cell = cells.GetCell(row, col, false);
                    var sheetColumn = columns.GetItem(col, false);

                    var cellType = RenderingExtensions.GetCellType(cell, sheetColumn);

                    object value = workSheet.DataStore.GetValue(row, col);

                    if (cell == null && value == null && sheetColumn == null && sheetRow == null)
                    {
                        if (cellType is ButtonCellType)
                            value = ((ButtonCellType)cellType).Text;
                        else if (cellType is CheckBoxCellType) { }
                           // value = false;
                        else
                            continue;
                    }

                    var cellRect = new Rect(x, y, columnWidth - penThickness, rowHeight - penThickness);

                    var style = workSheet.WorkBook.PickStyle(cell, sheetColumn, sheetRow);

                    if (style == null)
                        style = workSheet.WorkBook.GetNamedStyle(StyleKeys.DefaultSheetStyleKey);

                    style = style.Clone();
                    var formatter = workSheet.PickFormatter(cell, sheetColumn, sheetRow);

                    cellType.DrawCell(context, value, style.As<Style>(), formatter, cellRect, SheetView.Spread.PixelPerDip);

                    style.Dispose();
                    style = null;
                }
            }

            context.Pop();
        }
    }
}
