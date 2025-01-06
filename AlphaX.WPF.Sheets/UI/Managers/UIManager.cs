using System;

namespace AlphaX.WPF.Sheets.UI.Managers
{
    internal abstract class UIManager : IDisposable
    {
        protected AlphaXSpread Spread { get; private set; }

        public UIManager(AlphaXSpread spread)
        {
            Spread = spread;
        }

        public void Dispose()
        {
            Spread = null;
        }
    }
}
