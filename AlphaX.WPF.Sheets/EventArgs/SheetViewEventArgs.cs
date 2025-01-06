using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaX.WPF.Sheets
{
    public class SheetViewEventArgs
    {
        public IAlphaXSheetView OldSheetView { get; internal set; }
        public IAlphaXSheetView NewSheetView { get; internal set; }
    }
}
