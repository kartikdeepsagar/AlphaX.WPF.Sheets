using AlphaX.Sheets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

namespace AlphaX.WPF.Sheets
{
    public class SheetViewCollection : IEnumerable<IAlphaXSheetView>, INotifyCollectionChanged
    {
        private AlphaXSpread _spread;
        private Dictionary<WorkSheet, IAlphaXSheetView> _sheetViewStore;

        public IAlphaXSheetView ActiveSheetView { get; private set; }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event EventHandler<SheetViewEventArgs> ActiveSheetChanged;

        public SheetViewCollection(AlphaXSpread spread)
        {
            _spread = spread;
            _sheetViewStore = new Dictionary<WorkSheet, IAlphaXSheetView>();
            var workSheets = _spread.WorkBook.WorkSheets;
            WeakEventManager<WorkSheets, SheetEventArgs>.AddHandler(workSheets, "SheetAdded", OnSheetAdded);
            WeakEventManager<WorkSheets, SheetEventArgs>.AddHandler(workSheets, "SheetRemoved", OnSheetRemoved);
            WeakEventManager<WorkSheets, SheetEventArgs>.AddHandler(workSheets, "ActiveSheetChanged", OnActiveSheetChanged);
        }

        ~SheetViewCollection()
        {
            var workSheets = _spread.WorkBook.WorkSheets;
            WeakEventManager<WorkSheets, SheetEventArgs>.RemoveHandler(workSheets, "SheetAdded", OnSheetAdded);
            WeakEventManager<WorkSheets, SheetEventArgs>.RemoveHandler(workSheets, "SheetRemoved", OnSheetRemoved);
            WeakEventManager<WorkSheets, SheetEventArgs>.RemoveHandler(workSheets, "ActiveSheetChanged", OnActiveSheetChanged);
        }

        private void OnSheetAdded(object sender, SheetEventArgs e)
        {
            var sheetView = new AlphaXSheetView(_spread, e.WorkSheet.As<WorkSheet>());
            _sheetViewStore.Add(e.WorkSheet, sheetView);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, sheetView));
        }

        private void OnSheetRemoved(object sender, SheetEventArgs e)
        {
            var sheetView = _sheetViewStore[e.WorkSheet];
            _sheetViewStore.Remove(e.WorkSheet);

            if (_spread.WorkBook.WorkSheets.Count == 0)
            {
                ActiveSheetView = null;
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, sheetView));
        }

        private void OnActiveSheetChanged(object sender, SheetEventArgs e)
        {
            var args = new SheetViewEventArgs() { OldSheetView = ActiveSheetView };
            ActiveSheetView = _sheetViewStore[e.WorkSheet];
            args.NewSheetView = ActiveSheetView;
            ActiveSheetChanged?.Invoke(this, args);
        }

        public IEnumerator<IAlphaXSheetView> GetEnumerator()
        {
            return _sheetViewStore.Values.GetEnumerator();
        }

        public IAlphaXSheetView GetSheetView(WorkSheet workSheet)
        {
            return _sheetViewStore[workSheet];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
    }
}
