using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace AlphaX.Sheets.Data
{
    internal class AlphaXDataCollection : IDataCollection
    {
        private object _actualSource;

        private Dictionary<string, PropertyInfo> _itemPropertyInfo;
        public DataSourceType DataSourceType { get; private set; }

        public int Count
        {
            get
            {
                switch(DataSourceType)
                {
                    case DataSourceType.IList:
                        return (_actualSource as IList).Count;

                    case DataSourceType.IEnumerable:
                        return 0;

                    case DataSourceType.DataTable:
                        return (_actualSource as DataTable).Rows.Count;

                    default:
                        return 0;
                }
            }
        }

        public object ActualSource => _actualSource;

        public AlphaXDataCollection(object source)
        {
            _actualSource = source;
            InitCollection();
        }

        private void InitCollection()
        {
            if (_actualSource is IList list)
            {
                var type = GetItemType(list);

                if (type != null)
                {
                    var properties = type.GetProperties();
                    _itemPropertyInfo = new Dictionary<string, PropertyInfo>();

                    foreach (var property in properties)
                    {
                        _itemPropertyInfo.Add(property.Name, property);
                    }

                    DataSourceType = DataSourceType.IList;
                }
            }
            else if(_actualSource is IEnumerable enumerable)
            {
                DataSourceType = DataSourceType.IEnumerable;
            }
            else if (_actualSource is DataTable table)
            {
                DataSourceType= DataSourceType.DataTable;
            }
            else
            {
                DataSourceType = DataSourceType.NotSupported;
            }
        }

        private Type GetItemType(IList list)
        {
            var enumerable_type =
                    list.GetType()
                    .GetInterfaces()
                    .Where(i => i.IsGenericType && i.GenericTypeArguments.Length == 1)
                    .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (enumerable_type != null)
                return enumerable_type.GenericTypeArguments[0];

            if (list.Count == 0)
                return null;

            return list[0].GetType();
        }

        public object GetItemAt(int index)
        {
            switch (DataSourceType)
            {
                case DataSourceType.IList:
                    return (_actualSource as IList)[index];

                case DataSourceType.IEnumerable:
                    int currentIndex = 0;
                    var enumerator = (_actualSource as IEnumerable).GetEnumerator();
                    do
                    {
                        if (index == currentIndex)
                            break;

                        currentIndex++;                      
                    }
                    while (enumerator.MoveNext());               
                    return enumerator.Current;

                case DataSourceType.DataTable:
                    return (_actualSource as DataTable).Rows[index];

                default:
                    return null;
            }
        }

        public PropertyInfo GetPropertyInfo(string name)
        {
            return _itemPropertyInfo[name];
        }

        public void Dispose()
        {
            _actualSource = null;
            _itemPropertyInfo = null;
        }
    }
}
