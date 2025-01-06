using System.Reflection;
using System.Windows.Input;

namespace AlphaX.WPF.Sheets
{
    internal static class SheetUtils
    {
        public static string Tab { get; }
        public static string NextLine { get; }
        public static Cursor SheetCursor { get; }
        public static Cursor ColumnHeaderCursor { get; }
        public static Cursor RowHeaderCursor { get; }
        public static Cursor ColumnResizeCursor { get; }
        public static Cursor DragFillCursor { get; }
        public static Cursor RowResizeCursor { get; }

        static SheetUtils()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Tab = "\t";
            NextLine = "\n";
            SheetCursor = new Cursor(assembly.GetManifestResourceStream("AlphaX.WPF.Sheets.Resources.SheetCursor.cur"), true);
            DragFillCursor = new Cursor(assembly.GetManifestResourceStream("AlphaX.WPF.Sheets.Resources.DragFillCursor.cur"), true);
            ColumnHeaderCursor = new Cursor(assembly.GetManifestResourceStream("AlphaX.WPF.Sheets.Resources.ColumnHeaderCursor.cur"), true);
            RowHeaderCursor = new Cursor(assembly.GetManifestResourceStream("AlphaX.WPF.Sheets.Resources.RowHeaderCursor.cur"), true);
            ColumnResizeCursor = new Cursor(assembly.GetManifestResourceStream("AlphaX.WPF.Sheets.Resources.ColumnResizeCursor.cur"), true);
            RowResizeCursor = new Cursor(assembly.GetManifestResourceStream("AlphaX.WPF.Sheets.Resources.RowResizeCursor.cur"), true);
        }
    }
}
