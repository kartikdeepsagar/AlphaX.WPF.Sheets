using AlphaX.WPF.Sheets.Rendering;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.UI.Interaction
{
    internal abstract class InteractionLayer : Canvas
    {
        private AlphaXSheetViewRegion _ownerRegion;

        protected AlphaXSheetView SheetView { get; private set; }
        public bool IsAttached { get; private set; }

        public InteractionLayer()
        {
            Background = Brushes.Transparent;
            Focusable = true;
            FocusVisualStyle = null;
        }

        /// <summary>
        /// Attaches this layer with the specified region.
        /// </summary>
        /// <param name="region"></param>
        public virtual void AttachToRegion(AlphaXSheetViewRegion region)
        {
            _ownerRegion = region;
            _ownerRegion.AddInteractionLayer(this);
            SheetView = _ownerRegion.SheetView;
            IsAttached = true;
        }

        /// <summary>
        /// Detached the layer from the region if attached.
        /// </summary>
        public virtual void DetachFromRegion()
        {
            _ownerRegion.RemoveInteractionLayer();
            _ownerRegion = null;
            SheetView = null;
            IsAttached = false;
        }

        /// <summary>
        /// Hittest this layer at the current mouse point.
        /// </summary>
        /// <returns></returns>
        protected SpreadHitTestResult HitTest()
        {
            return HitTest(Mouse.GetPosition(SheetView.Spread));
        }

        protected SpreadHitTestResult HitTest(Point point)
        {
            return SheetView.Spread.HitTest(point);
        }

        protected Rect ToSheetViewRect(Rect rect)
        {
            var viewPort = SheetView.ViewPort.As<ViewPort>();
            rect.X -= viewPort.LeftColumnLocation;
            rect.Y -= viewPort.TopRowLocation;
            return rect;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            Focus();
            CaptureMouse();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            ReleaseMouseCapture();
        }
    }
}
