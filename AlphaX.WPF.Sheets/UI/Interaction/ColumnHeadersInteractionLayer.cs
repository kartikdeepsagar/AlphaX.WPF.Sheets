using AlphaX.WPF.Sheets.Rendering;
using AlphaX.WPF.Sheets.UI.Managers;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.UI.Interaction
{
    internal class ColumnHeadersInteractionLayer : InteractionLayer
    {
        private ColumnResizeManager _resizeManager;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            var hitTest = HitTest();

            if (hitTest.Element == VisualElement.ColumnHeaderResizeBar && SheetView.Spread.AllowColumnResize)
            {
                _resizeManager.BeginResizeColumn(hitTest.Column, (int)hitTest.Position.X);
                Children.Add(_resizeManager.ResizeLine);
            }
            else
            {
                if (SheetView.Spread.EditingManager.IsEditing)
                {
                    if (!SheetView.Spread.EditingManager.EndEdit(true))
                        return;
                }
                SheetView.Spread.SelectionManager.SelectColumn(hitTest.Column);
            }
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
            var hitTest = HitTest();

            if (SheetView.Spread.EditingManager.IsEditing)
            {
                if (!SheetView.Spread.EditingManager.EndEdit(true))
                    return;
            }

            if (SheetView.Selection.ColumnCount <= 1)
                SheetView.Spread.SelectionManager.SelectColumn(hitTest.Column);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            var hitTest = HitTest();

            if (_resizeManager.IsResizing)
            {
                _resizeManager.EndResizeColumn();
                Children.Remove(_resizeManager.ResizeLine);
                SheetView.Spread.SheetTabControl.UpdateScrollbars();
            }

            if(hitTest != null && hitTest.Element != VisualElement.ColumnHeaderResizeBar)
                Cursor = SheetUtils.ColumnHeaderCursor;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if(_resizeManager.IsResizing)
            {
                _resizeManager.ResizeColumn((int)e.GetPosition(this).X);
                return;
            }

            var hitTest = HitTest();

            if (hitTest == null)
                return;

            if(hitTest.Element == VisualElement.ColumnHeaderResizeBar && SheetView.Spread.AllowColumnResize)
            {
                Cursor = SheetUtils.ColumnResizeCursor;
            }
            else
            {
                Cursor = SheetUtils.ColumnHeaderCursor;
            }

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            int leftColumn = Math.Min(hitTest.Column, SheetView.ActiveColumn);
            int rightColumn = Math.Max(hitTest.Column, SheetView.ActiveColumn);
            SheetView.Spread.SelectionManager.SelectColumns(leftColumn, rightColumn - leftColumn + 1);
        }      

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            var selectionRangeRect = ToSheetViewRect(SheetView.ViewPort.GetRangeRect(SheetView.Selection));
            dc.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight + 0.5)));
            dc.DrawLine(SheetView.Spread.SelectionBorderPen,
                new Point(selectionRangeRect.Left, ActualHeight), 
                new Point(selectionRangeRect.Right, ActualHeight));
            dc.Pop();
        }

        public override void AttachToRegion(AlphaXSheetViewRegion region)
        {
            base.AttachToRegion(region);

            if (_resizeManager == null)
                _resizeManager = new ColumnResizeManager(region.SheetView.Spread);
        }

        public override void DetachFromRegion()
        {
            base.DetachFromRegion();
            if (_resizeManager != null)
            {
                _resizeManager.Dispose();
                _resizeManager = null;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            //Clip = new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight + 0.5));
        }
    }
}
