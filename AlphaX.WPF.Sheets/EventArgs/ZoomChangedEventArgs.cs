using System;

namespace AlphaX.WPF.Sheets
{
    public class ZoomChangedEventArgs : EventArgs
    {
        public double OldZoomFactor { get; }
        public double NewZoomFactor { get; }

        public ZoomChangedEventArgs(double oldZoomFactor, double newZoomFactor)
        {
            OldZoomFactor = oldZoomFactor;
            NewZoomFactor = newZoomFactor;
        }
    }
}
