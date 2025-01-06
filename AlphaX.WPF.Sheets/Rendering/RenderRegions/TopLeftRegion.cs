using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal class TopLeftRegion : AlphaXSheetViewRegion
    {
        private SpreadHitTestResult _hitTest;

        public TopLeftRegion()
        {
            
        }

        protected override Drawing GetDrawing()
        {
            return SheetView.Spread.RenderEngine.TopLeftRenderer.Drawing;
        }

        protected override SpreadHitTestResult HitTestCore(AlphaXSheetView sheetView, Point point)
        {
            if (_hitTest == null)
            {              
                _hitTest = new SpreadHitTestResult()
                {
                    ActualHitTestPoint = point,
                    Position = new Point(0, 0),
                    Element = VisualElement.TopLeft,
                    Row = -1,
                    Column = -1,
                    Sheet = sheetView
                };
            }

            return _hitTest;
        }
    }
}
