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
        private Dictionary<IWorkSheet, IAlphaXSheetView> _sheetViewStore;

        public IAlphaXSheetView ActiveSheetView { get; private set; }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event EventHandler<SheetViewEventArgs> ActiveSheetChanged;

        public SheetViewCollection(AlphaXSpread spread)
        {
            _spread = spread;
            _sheetViewStore = new Dictionary<IWorkSheet, IAlphaXSheetView>();
            var workSheets = (WorkSheets)_spread.WorkBook.WorkSheets;
            WeakEventManager<WorkSheets, SheetChangedEventArgs>.AddHandler(workSheets, "SheetAdded", OnSheetAdded);
            WeakEventManager<WorkSheets, SheetChangedEventArgs>.AddHandler(workSheets, "SheetRemoved", OnSheetRemoved);
            WeakEventManager<WorkSheets, SheetChangedEventArgs>.AddHandler(workSheets, "ActiveSheetChanged", OnActiveSheetChanged);
        }

        ~SheetViewCollection()
        {
            var workSheets = (WorkSheets)_spread.WorkBook.WorkSheets;
            WeakEventManager<WorkSheets, SheetChangedEventArgs>.RemoveHandler(workSheets, "SheetAdded", OnSheetAdded);
            WeakEventManager<WorkSheets, SheetChangedEventArgs>.RemoveHandler(workSheets, "SheetRemoved", OnSheetRemoved);
            WeakEventManager<WorkSheets, SheetChangedEventArgs>.RemoveHandler(workSheets, "ActiveSheetChanged", OnActiveSheetChanged);
        }

        private void OnSheetAdded(object sender, SheetChangedEventArgs e)
        {
            var sheetView = new AlphaXSheetView(_spread, e.WorkSheet.As<WorkSheet>());
            _sheetViewStore.Add((WorkSheet)e.WorkSheet, sheetView);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, sheetView));
        }

        private void OnSheetRemoved(object sender, SheetChangedEventArgs e)
        {
            var sheetView = _sheetViewStore[(WorkSheet)e.WorkSheet];
            _sheetViewStore.Remove((WorkSheet)e.WorkSheet);

            if (_spread.WorkBook.WorkSheets.Count == 0)
            {
                ActiveSheetView = null;
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, sheetView));
        }

        private void OnActiveSheetChanged(object sender, SheetChangedEventArgs e)
        {
            var args = new SheetViewEventArgs() { OldSheetView = ActiveSheetView };
            ActiveSheetView = _sheetViewStore[(WorkSheet)e.WorkSheet];
            args.NewSheetView = ActiveSheetView;
            ActiveSheetChanged?.Invoke(this, args);
        }

        public IEnumerator<IAlphaXSheetView> GetEnumerator()
        {
            return _sheetViewStore.Values.GetEnumerator();
        }

        public IAlphaXSheetView GetSheetView(IWorkSheet workSheet)
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
