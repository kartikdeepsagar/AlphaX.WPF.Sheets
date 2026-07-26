using System;
using System.Collections.Generic;

namespace DevBrewLabs.Spreadsheet
{
    internal abstract class SheetDimensionCollection<T> where T : class
    {
        protected internal SortedDictionary<int, T> InternalCollection;
        protected abstract LocationCache<T> LocationCache { get; }
        public T this[int index]
        {
            get
            {
                return GetItem(index, true);
            }
        }

        internal SheetDimensionCollection()
        {
            InternalCollection = new SortedDictionary<int, T>();
        }

        internal double GetLocation(int index)
        {
            return LocationCache.GetLocation(index);
        }

        internal void UpdateLocation(int fromIndex, double offset)
        {
            LocationCache.UpdateLocation(fromIndex, offset);
        }

        public abstract void Insert(int index, int count);
        public abstract void Remove(int index, int count);

        /// <summary>
        /// Gets item from collection. returns null if item doesn't exist.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T GetItem(int index)
        {
            return GetItem(index, false);
        }

        /// <summary>
        /// Gets the item present at the specified index.
        /// </summary>
        /// <param name="index">
        /// Index of the item.
        /// </param>
        /// <param name="createIfNotExist">
        /// Whether to create and add the item if not exist.
        /// </param>
        /// <returns></returns>
        protected T GetItem(int index, bool createIfNotExist)
        {
            if (InternalCollection.TryGetValue(index, out T item))
            {
                return item;
            }
            else if (createIfNotExist)
            {
                return AddItemInternal(index);
            }
            else
                return null;
        }

        /// <summary>
        /// Adds a new item of type T at the provided index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected T AddItemInternal(int index)
        {
            var item = CreateItem(index);
            InternalCollection.Add(index, item);
            return item;
        }

        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <returns></returns>
        protected abstract T CreateItem(int index);
    }

    internal sealed class LocationCache<T>
    {
        private readonly Func<int> _count;
        private readonly Func<double> _defaultSize;
        private readonly IDictionary<int, T> _items;
        private readonly Func<T, double> _getSize;

        private double[] _locations;
        private int _lastCalculated;

        public LocationCache(
            Func<int> count,
            Func<double> defaultSize,
            IDictionary<int, T> items,
            Func<T, double> getSize)
        {
            _count = count;
            _defaultSize = defaultSize;
            _items = items;
            _getSize = getSize;
        }

        public double GetLocation(int index)
        {
            if (index <= 0)
                return 0;

            EnsureCapacity(index + 1);

            while (_lastCalculated < index)
            {
                double size = _defaultSize();

                if (_items.TryGetValue(_lastCalculated, out var item))
                    size = _getSize(item);

                _locations[_lastCalculated + 1] =
                    _locations[_lastCalculated] + size;

                _lastCalculated++;
            }

            return _locations[index];
        }

        public void UpdateLocation(int fromIndex, double delta)
        {
            if (delta == 0 || _locations == null)
                return;

            for (int i = fromIndex; i <= _lastCalculated; i++)
                _locations[i] += delta;
        }

        public void Reset()
        {
            _lastCalculated = 0;

            if (_locations != null && _locations.Length > 0)
                _locations[0] = 0;
        }

        private void EnsureCapacity(int requiredCapacity = 0)
        {
            int count = Math.Max(_count() + 1, requiredCapacity);

            if (_locations == null)
            {
                _locations = new double[Math.Max(16, count)];
            }
       
            if (_locations.Length >= count)
                return;

            int size = _locations.Length;

            while (size < count)
                size *= 2;

            Array.Resize(ref _locations, size);
        }
    }
}
