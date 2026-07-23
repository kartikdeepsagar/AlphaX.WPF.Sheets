using AlphaX.CalcEngine;
using AlphaX.Sheets.Core;
using System;
using System.Collections.Generic;

namespace AlphaX.Sheets
{
    public class WorkBook : IWorkBook
    {
        private WorkBookDataProvider _dataProvider;
        private IUpdateProvider _updateProvider;
        private Dictionary<string, Style> _namedStyles;

        public string Name { get; set; }
        public IWorkSheets WorkSheets { get; private set; }
        public ICalcEngine CalcEngine { get; private set; }
        public StylePalette StylePalette { get; private set; }
        public IUpdateProvider UpdateProvider
        {
            get
            {
                return _updateProvider;
            }
            private set
            {
                _updateProvider = value;
            }
        }
        internal WorkBookDataProvider DataProvider => _dataProvider;

        public WorkBook(string name)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
            WorkSheets = new WorkSheets(this);
            _namedStyles = new Dictionary<string, Style>();
            _dataProvider = new WorkBookDataProvider(this);
            CalcEngine = new AlphaXCalcEngine(_dataProvider);
            StylePalette = new StylePalette();
        }

        public WorkBook(string name, IUpdateProvider updateProvider) : this(name)
        {
            if(updateProvider == null)
                throw new ArgumentNullException(nameof(updateProvider));

            UpdateProvider = updateProvider;
        }

        public void AddNamedStyle(string styleName, Style style)
        {
            if (_namedStyles.ContainsKey(styleName))
                throw new ArgumentException($"A style is already registered with the name '{styleName}'");

            _namedStyles.Add(styleName, style);
        }

        public Style GetNamedStyle(string styleName)
        {
            if(_namedStyles.TryGetValue(styleName, out Style style))
                return style;

            return null;
        }

        public void Dispose()
        {
            WorkSheets.Dispose();
            _namedStyles.Clear();
            StylePalette?.Clear();
            StylePalette = null;
            WorkSheets = null;
            CalcEngine = null;
            _namedStyles = null;
            _dataProvider = null;
        }
    }
}
