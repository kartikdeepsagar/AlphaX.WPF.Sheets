namespace AlphaX.Sheets.Drawing
{
    public struct Thickness
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }
        public double Right { get; set; }

        public Thickness(double left, double top, double bottom, double right)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Thickness(double left, double top)
        {
            Left = left;
            Top = top;
            Right = 0;
            Bottom = 0;
        }

        public Thickness(double thickness)
        {
            Left = thickness;
            Top = thickness;
            Right = thickness;
            Bottom = thickness;
        }
    }
}
