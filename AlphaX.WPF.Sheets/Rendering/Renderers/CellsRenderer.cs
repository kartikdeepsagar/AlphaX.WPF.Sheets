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

            for (int row = topRow; row <= bottomRow; row++)
            {
                var rowHeight = rows.GetRowHeight(row);

                if (rowHeight == 0)
                    continue;

                if (workSheet.FilterProvider.FilteredRows.ContainsKey(row))
                    continue;

                var sheetRow = rows.GetItem(row, false);
                var rowLocation = rows.GetLocation(row);

                for (int col = leftColumn; col <= rightColumn; col++)
                {
                    if(Engine.RenderInfo.PartialRender)
                        Engine.EnsureNewCacheDrawing(this, row, col);

                    var columnWidth = columns.GetColumnWidth(col);

                    if (columnWidth == 0)
                        continue;

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


                    var columnLocation = columns.GetLocation(col);

                    var cellRect = new Rect(columnLocation - viewport.LeftColumnLocation,
                        rowLocation - viewport.TopRowLocation, columnWidth - 0.75, rowHeight - 0.75);

                    var style = workSheet.WorkBook.PickStyle(cell, sheetColumn, sheetRow);

                    if (style == null)
                        style = workSheet.WorkBook.GetNamedStyle(StyleKeys.DefaultSheetStyleKey);

                    style = style.Clone();
                    var formatter = workSheet.PickFormatter(cell, sheetColumn, sheetRow);
                    var cellDrawing = Engine.CreateDrawingObject(this, row, col);

                    double halfPenWidth = (SheetView.Spread.GridLinePen.Thickness * SheetView.Spread.PixelPerDip) / 2;
                    GuidelineSet guidelines = new GuidelineSet();
                    guidelines.GuidelinesX.Add(cellRect.Left + halfPenWidth);
                    guidelines.GuidelinesX.Add(cellRect.Right + halfPenWidth);
                    guidelines.GuidelinesY.Add(cellRect.Top + halfPenWidth);
                    guidelines.GuidelinesY.Add(cellRect.Bottom + halfPenWidth);

                    var cellDrawingContext = cellDrawing.Open();
                    cellDrawingContext.PushClip(new RectangleGeometry(cellRect));
                    cellDrawingContext.PushGuidelineSet(guidelines);
                    cellType.DrawCell(cellDrawingContext, value, style.As<Style>(), formatter, cellRect, SheetView.Spread.PixelPerDip);
                    cellDrawingContext.Pop();
                    cellDrawingContext.Pop();
                    cellDrawingContext.Close();

                    context.DrawDrawing(cellDrawing);
                    style.Dispose();
                    style = null;
                }
            }
        }
    }
}
