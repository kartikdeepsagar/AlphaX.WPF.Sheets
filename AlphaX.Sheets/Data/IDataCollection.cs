using System;
using System.Reflection;

namespace AlphaX.Sheets.Data
{
    public interface IDataCollection : IDisposable
    {
        int Count { get; }
        object GetItemAt(int index);
        PropertyInfo GetPropertyInfo(string name);
    }
}
