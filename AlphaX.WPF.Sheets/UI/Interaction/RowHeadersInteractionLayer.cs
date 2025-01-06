﻿using AlphaX.WPF.Sheets.Rendering;
using AlphaX.WPF.Sheets.UI.Managers;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.UI.Interaction
{
    internal class RowHeadersInteractionLayer : InteractionLayer
    {
        private RowResizeManager _resizeManager;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            var hitTest = HitTest();

            if (hitTest.Element == VisualElement.RowHeaderResizeBar && SheetView.Spread.AllowRowResize)
            {
                _resizeManager.BeginResizeRow(hitTest.Row, (int)hitTest.Position.Y);
                Children.Add(_resizeManager.ResizeLine);
            }
            else
            {
                if (SheetView.Spread.EditingManager.IsEditing)
                {
                    if (!SheetView.Spread.EditingManager.EndEdit(true))
                        return;
                }

                SheetView.Spread.SelectionManager.SelectRow(hitTest.Row);
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

            if(SheetView.Selection.RowCount <= 1)
                SheetView.Spread.SelectionManager.SelectRow(hitTest.Row);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            var hitTest = HitTest();

            if (_resizeManager.IsResizing)
            {
                _resizeManager.EndResizeRow();
                Children.Remove(_resizeManager.ResizeLine);
            }

            if (hitTest != null && hitTest.Element != VisualElement.RowHeaderResizeBar)
                Cursor = SheetUtils.RowHeaderCursor;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_resizeManager.IsResizing)
            {
                _resizeManager.ResizeRow((int)e.GetPosition(this).Y);
                return;
            }

            var hitTest = HitTest();

            if (hitTest == null)
                return;

            if (hitTest.Element == VisualElement.RowHeaderResizeBar && SheetView.Spread.AllowRowResize)
            {
                Cursor = SheetUtils.RowResizeCursor;
            }
            else
            {
                Cursor = SheetUtils.RowHeaderCursor;
            }

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            int topRow = Math.Min(hitTest.Row, SheetView.ActiveRow);
            int bottomRow = Math.Max(hitTest.Row, SheetView.ActiveRow);
            SheetView.Spread.SelectionManager.SelectRows(topRow, bottomRow - topRow + 1);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            var selectionRangeRect = ToSheetViewRect(SheetView.ViewPort.GetRangeRect(SheetView.Selection));
            dc.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth + 0.5, ActualHeight)));
            dc.DrawLine(SheetView.Spread.SelectionBorderPen,
                new Point(ActualWidth, selectionRangeRect.Top),
                new Point(ActualWidth, selectionRangeRect.Bottom));
            dc.Pop();
        }

        public override void AttachToRegion(AlphaXSheetViewRegion region)
        {
            base.AttachToRegion(region);

            if (_resizeManager == null)
                _resizeManager = new RowResizeManager(region.SheetView.Spread);
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
            //Clip = new RectangleGeometry(new Rect(0, 0, ActualWidth + 0.5, ActualHeight));
        }
    }
}
