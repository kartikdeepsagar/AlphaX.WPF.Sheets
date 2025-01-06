using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AlphaX.WPF.Sheets.UI.Managers
{
    internal abstract class ResizeManagerBase : UIManager
    {
        public Line ResizeLine { get; }

        protected ResizeManagerBase(AlphaXSpread spread) : base(spread)
        {
            ResizeLine = new Line();
            ResizeLine.Stroke = Brushes.Black;
            ResizeLine.StrokeThickness = 0.75;
            ResizeLine.StrokeDashArray = new DoubleCollection(new double[] { 5, 2 });
            ResizeLine.Visibility = Visibility.Collapsed;
        }
    }
}
