using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    /// <summary>
    /// This is an abstract base class for sheet component renderers.
    /// </summary>
    internal abstract class Renderer
    {
        private DrawingContext _context;
        protected RenderEngine Engine { get; private set; }
        protected AlphaXSheetView SheetView { get; private set; }
        public DrawingGroup Drawing { get; private set; }

        public Renderer()
        {
            Drawing = new DrawingGroup();
        }

        public void SetRenderSheet(AlphaXSheetView sheetView)
        {
            Drawing.Children.Clear();
            SheetView = sheetView;
            Engine = SheetView.Spread.RenderEngine;
        }

        /// <summary>
        /// Renders the content to the Drawing property.
        /// </summary>
        /// <param name="sheetView"></param>
        /// <param name="topRow"></param>
        /// <param name="leftColumn"></param>
        /// <param name="bottomRow"></param>
        /// <param name="rightColumn"></param>
        /// <returns></returns>
        public void Render(int topRow, int leftColumn, int bottomRow, int rightColumn)
        {
            Drawing.ClipGeometry = Engine.RenderInfo.ViewPortGeometry;
            if (_context != null)
                _context.Close();
            _context = Engine.RenderInfo.PartialRender ? Drawing.Append() : Drawing.Open();
            OnRender(_context, topRow, leftColumn, bottomRow, rightColumn);
        }

        /// <summary>
        /// Ends the rendering.
        /// </summary>
        public void EndRender()
        {
            if (_context == null)
                return;

            _context.Close();
            _context = null;
        }

        /// <summary>
        /// Provides the rendering logic.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="topRow"></param>
        /// <param name="leftColumn"></param>
        /// <param name="bottomRow"></param>
        /// <param name="rightColumn"></param>
        protected abstract void OnRender(DrawingContext context, int topRow, int leftColumn, int bottomRow, int rightColumn);
    }
}
