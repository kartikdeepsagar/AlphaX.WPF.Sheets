using System.Windows;
using System.Windows.Media;

namespace DevBrewLabs.WPF.Spreadsheet.Rendering
{
    internal class TopLeftSurface : SheetViewSurface
    {
        private SpreadHitTestResult _hitTest;

        public TopLeftSurface()
        {
            
        }

        protected override Drawing GetDrawing()
        {
            return SheetView.Spread.RenderEngine.TopLeftRenderer.Drawing;
        }

        protected override SpreadHitTestResult HitTestCore(SheetView sheetView, Point point)
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
