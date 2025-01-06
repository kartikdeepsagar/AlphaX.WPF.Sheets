using System;

namespace AlphaX.Sheets
{
    public class SheetEventArgs : EventArgs
    {
        public WorkSheet WorkSheet { get; }

        public SheetEventArgs(WorkSheet workSheet)
        {
            WorkSheet = workSheet;
        }
    }
}
