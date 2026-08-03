using DevBrewLabs.Spreadsheet;
using DevBrewLabs.WPF.Spreadsheet.CellTypes;
using DevBrewLabs.WPF.Spreadsheet.UI;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering
{
    internal class CellsRenderer : Renderer
    {
        protected override void OnRender(DrawingContext context, int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            var workSheet = SheetView.WorkSheet;
            var workBook = (WorkBook)workSheet.WorkBook;
            var rows = (Rows)workSheet.Rows;
            var columns = (Columns)workSheet.Columns;
            var cells = (Cells)workSheet.Cells;
            var viewport = (ViewPort)SheetView.ViewPort;

            double zoom = SheetView.ZoomFactor > 0 ? SheetView.ZoomFactor : 1.0;
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

                var sheetRow = rows.GetItem(row);
                var rowLocation = rows.GetLocation(row);
                var y = (rowLocation - viewport.TopRowLocation) * zoom;
                var scaledRowHeight = rowHeight * zoom;

                guidelines.GuidelinesY.Add(y + halfPenWidth);
                guidelines.GuidelinesY.Add(y + scaledRowHeight - penThickness + halfPenWidth);

                for (int col = leftColumn; col <= rightColumn; col++)
                {
                    var columnWidth = columns.GetColumnWidth(col);

                    if (columnWidth == 0)
                        continue;

                    var columnLocation = columns.GetLocation(col);
                    var x = (columnLocation - viewport.LeftColumnLocation) * zoom;
                    var scaledColumnWidth = columnWidth * zoom;

                    if (row == topRow)
                    {
                        guidelines.GuidelinesX.Add(x + halfPenWidth);
                        guidelines.GuidelinesX.Add(x + scaledColumnWidth - penThickness + halfPenWidth);
                    }

                    var cell = cells.GetCell(row, col, false);
                    var sheetColumn = columns.GetItem(col);

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

                    var cellRect = new Rect(x, y, scaledColumnWidth - penThickness, scaledRowHeight - penThickness);

                    var baseStyle = workBook.PickStyle(cell, sheetColumn, sheetRow, SheetRegion.Cells);
                    var style = baseStyle.GetWpfStyle();

                    var formatter = workSheet.PickFormatter(cell, sheetColumn, sheetRow);
                    cellType.DrawCell(context, value, style, formatter, cellRect, SheetView.Spread.PixelPerDip, workSheet.AllowMultiLineText, zoom);
                }
            }

            context.Pop();
        }
    }
}
