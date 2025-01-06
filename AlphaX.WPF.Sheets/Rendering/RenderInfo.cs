using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal class RenderInfo
    {
        public bool PartialRender { get; set; }
        public Geometry ViewPortGeometry { get; set; }
    }
}
