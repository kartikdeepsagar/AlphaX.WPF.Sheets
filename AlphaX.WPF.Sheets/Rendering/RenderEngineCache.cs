using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace AlphaX.WPF.Sheets.Rendering
{
    internal class RenderEngineCache
    {
        private Dictionary<Renderer, Dictionary<(int, int), Drawing>> _drawingStore;

        public RenderEngineCache()
        {
            _drawingStore = new Dictionary<Renderer, Dictionary<(int, int), Drawing>>();
        }

        public void RegisterCacheType(Renderer type)
        {
            _drawingStore.Add(type, new Dictionary<(int, int), Drawing>());
        }

        /// <summary>
        /// Adds drawing to cache.
        /// </summary>
        /// <param name="cacheType"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="drawing"></param>
        public void AddDrawing(Renderer cacheType, int row, int col, Drawing drawing)
        {
            if (!_drawingStore[cacheType].ContainsKey((row, col)))
            {
                _drawingStore[cacheType].Add((row, col), drawing);
            }
            else
            {
                _drawingStore[cacheType][(row, col)] = drawing;
            }
        }

        /// <summary>
        /// Clears cache.
        /// </summary>
        public void Clear()
        {
            foreach(var stores in _drawingStore)
            {
                stores.Value.Clear();
            }
            GC.Collect();
        }

        /// <summary>
        /// Gets the drawing object from cache if exists.
        /// </summary>
        /// <param name="cacheType"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="drawing"></param>
        /// <returns></returns>
        public bool TryGetDrawing(Renderer cacheType, int row, int col, out Drawing drawing)
        {
            return _drawingStore[cacheType].TryGetValue((row, col), out drawing);
        }

        /// <summary>
        /// Removes the drawing object from cache
        /// </summary>
        /// <param name="cacheType"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void RemoveFromCache(Renderer cacheType, int row, int col)
        {
            if (_drawingStore[cacheType].ContainsKey((row, col)))
            {
                cacheType.Drawing.Children.Remove(_drawingStore[cacheType][(row, col)]);
                _drawingStore[cacheType].Remove((row, col));
            }
        }
    }
}