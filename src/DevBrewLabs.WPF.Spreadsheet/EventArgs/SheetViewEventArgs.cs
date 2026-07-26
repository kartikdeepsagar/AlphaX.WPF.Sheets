using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBrewLabs.WPF.Spreadsheet
{
    public class SheetViewEventArgs
    {
        public ISheetView OldSheetView { get; internal set; }
        public ISheetView NewSheetView { get; internal set; }
    }
}
