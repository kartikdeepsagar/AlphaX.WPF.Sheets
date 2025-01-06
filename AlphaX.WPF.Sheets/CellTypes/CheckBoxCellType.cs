using AlphaX.Sheets;
using AlphaX.Sheets.Formatters;
using AlphaX.WPF.Sheets.UI.Editors;
using System;
using System.Windows;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.CellTypes
{
    public class CheckBoxCellType : BaseCellType
    {
        internal static Size CheckBoxSize { get; }

        static CheckBoxCellType()
        {
            CheckBoxSize = new Size(11, 11);
        }

        private Pen _pen;
        private Pen _markPen;

        public bool IsThreeState { get; set; }

        public CheckBoxCellType()
        {
            _pen = new Pen(Brushes.Black, 0.75);
            _markPen = new Pen(Brushes.Black, 1.5);
            IsThreeState = false;
        }

        internal override void DrawCell(DrawingContext context, object value, Style style, IFormatter formatter, Rect cellRect, double pixelPerDip)
        {
            var checkBoxRect = cellRect.ToCellCheckBoxRect(CheckBoxSize);
            var halfPenWidth = _pen.Thickness / 2;
            GuidelineSet guidelines = new GuidelineSet();
            guidelines.GuidelinesX.Add(checkBoxRect.Left + halfPenWidth);
            guidelines.GuidelinesX.Add(checkBoxRect.Right + halfPenWidth);
            guidelines.GuidelinesY.Add(checkBoxRect.Top + halfPenWidth);
            guidelines.GuidelinesY.Add(checkBoxRect.Bottom + halfPenWidth);
            context.PushGuidelineSet(guidelines);

            base.DrawCell(context, value, style, formatter, cellRect, pixelPerDip);

            context.DrawRectangle(null, _pen, checkBoxRect);
            DrawMark(context, checkBoxRect, value);
            context.Pop();
        }

        /// <summary>
        /// Draws checkbox mark
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="checkBoxRect"></param>
        /// <param name="value"></param>
        private void DrawMark(DrawingContext ctx, Rect checkBoxRect, object value)
        {
            if(IsThreeState && value == null)
            {
                checkBoxRect.Inflate(-2, -2);
                ctx.DrawRectangle(Brushes.Black, null, checkBoxRect);
            }
            else if(value != null && Convert.ToBoolean(value))
            {
                var bottom = new Point(checkBoxRect.Left + checkBoxRect.Width / 2, checkBoxRect.Bottom - 1.5);
                ctx.DrawLine(_markPen, new Point(checkBoxRect.Left + 1.5, checkBoxRect.Top + checkBoxRect.Height / 2),
                    bottom);
                ctx.DrawLine(_markPen, bottom, new Point(checkBoxRect.Right - 1.5, checkBoxRect.Top + 1.5));
            }
        }

        public override AlphaXEditorBase GetEditor(Style style)
        {
            throw new NotImplementedException();
        }
    }
}
