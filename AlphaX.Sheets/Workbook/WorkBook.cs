using AlphaX.CalcEngine;
using System;
using System.Collections.Generic;

namespace AlphaX.Sheets
{
    public class WorkBook : IWorkBook
    {
        private WorkBookDataProvider _dataProvider;
        private IUpdateProvider _updateProvider;
        private Dictionary<string, NamedStyle> _namedStyles;

        public string Name { get; set; }
        public WorkSheets WorkSheets { get; private set; }
        public ICalcEngine CalcEngine { get; private set; }
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
            _namedStyles = new Dictionary<string, NamedStyle>();
            _dataProvider = new WorkBookDataProvider(this);
            CalcEngine = new AlphaXCalcEngine(_dataProvider);
        }

        public WorkBook(string name, IUpdateProvider updateProvider) : this(name)
        {
            if(updateProvider == null)
                throw new ArgumentNullException(nameof(updateProvider));

            UpdateProvider = updateProvider;
        }

        public void AddNamedStyle(string styleName, NamedStyle style)
        {
            if (_namedStyles.ContainsKey(styleName))
                throw new ArgumentException($"A style is already registered with the name '{styleName}'");

            _namedStyles.Add(styleName, style);
        }

        public NamedStyle GetNamedStyle(string styleName)
        {
            if (!_namedStyles.ContainsKey(styleName))
                throw new ArgumentException($"Style with name '{styleName}' not found.");

            return _namedStyles[styleName];
        }

        public void Dispose()
        {
            WorkSheets.Dispose();
            _namedStyles.Clear();
            WorkSheets = null;
            CalcEngine = null;
            _namedStyles = null;
            _dataProvider = null;
        }
    }
}
