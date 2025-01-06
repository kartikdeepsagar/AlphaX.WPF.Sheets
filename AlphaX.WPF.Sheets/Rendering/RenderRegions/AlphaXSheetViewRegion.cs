using AlphaX.WPF.Sheets.UI.Interaction;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    /// <summary>
    /// An abstract class for sheet UI regions.
    /// </summary>
    internal abstract class AlphaXSheetViewRegion : Canvas
    {
        private InteractionLayer _interactionLayer;

        public AlphaXSheetView SheetView { get; private set; }

        public AlphaXSheetViewRegion()
        {
            SnapsToDevicePixels = true;
        }

        public virtual void AttachSheet(AlphaXSheetView sheetView)
        {
            SheetView = sheetView;
        }

        /// <summary>
        /// Adds interaction layer for this region.
        /// </summary>
        /// <param name="interactionLayer"></param>
        public void AddInteractionLayer(InteractionLayer interactionLayer)
        {
            _interactionLayer = interactionLayer;
            Children.Add(_interactionLayer);
            Panel.SetZIndex(_interactionLayer, 1);
        }

        /// <summary>
        /// Removes interaction layer.
        /// </summary>
        public void RemoveInteractionLayer()
        {
            if (SheetView.Spread.EditingManager.IsEditing)
                SheetView.Spread.EditingManager.EndEdit(true);

            Children.Remove(_interactionLayer);
            _interactionLayer = null;
        }

        /// <summary>
        /// Gets the interaction layer for this region.
        /// </summary>
        /// <returns></returns>
        public InteractionLayer GetInteractionLayer()
        {
            return _interactionLayer;
        }

        /// <summary>
        /// Hit tests sheet on the provided coordinates.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public SpreadHitTestResult HitTest(Point point)
        {
            return HitTestCore(SheetView, point);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawDrawing(GetDrawing());
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (_interactionLayer == null)
                return;

            _interactionLayer.Width = sizeInfo.NewSize.Width;
            _interactionLayer.Height = sizeInfo.NewSize.Height;
            //_interactionLayer.Clip = new RectangleGeometry(new Rect(new Point(0, 0), sizeInfo.NewSize));
        }

        /// <summary>
        /// Gets the drawing responsible for this canvas UI.
        /// </summary>
        /// <returns></returns>
        protected abstract Drawing GetDrawing();

        /// <summary>
        /// Provides hit test support.
        /// </summary>
        /// <param name="dc"></param>
        protected abstract SpreadHitTestResult HitTestCore(AlphaXSheetView sheetView, Point hitPoint);
    }
}
