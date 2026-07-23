using System;

namespace AlphaX.Sheets
{
    public class SheetEventArgs : EventArgs
    {
        public IWorkSheet WorkSheet { get; }

        public SheetEventArgs(IWorkSheet workSheet)
        {
            WorkSheet = workSheet;
        }
    }
}
