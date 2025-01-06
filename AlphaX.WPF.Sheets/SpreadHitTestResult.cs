using System.Windows;

namespace AlphaX.WPF.Sheets
{
    public class SpreadHitTestResult
    {
        public IAlphaXSheetView Sheet { get; internal set; }
        public int Row { get; internal set; }
        public int Column { get; internal set; }
        /// <summary>
        /// Position of the hittest element.
        /// </summary>
        internal Point Position { get; set; }
        /// <summary>
        /// Actual hit test position
        /// </summary>
        internal Point ActualHitTestPoint { get; set; }
        public VisualElement Element { get; internal set; }
    }
}
