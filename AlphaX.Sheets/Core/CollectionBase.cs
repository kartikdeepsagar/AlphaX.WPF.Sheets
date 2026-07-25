using System;
using System.Collections.Generic;

namespace AlphaX.Sheets
{
    internal abstract class CollectionBase<T> where T : class
    {
        protected internal SortedDictionary<int, T> InternalCollection;
        public T this[int index]
        {
            get
            {
                return GetItem(index, true);
            }
        }

        internal CollectionBase()
        {
            InternalCollection = new SortedDictionary<int, T>();
        }

        internal abstract double GetLocation(int index, bool recalculate = false);
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
}
