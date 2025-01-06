using System;

namespace AlphaX.Sheets
{
    public abstract class BaseEventArgs : EventArgs
    {
        /// <summary>
        /// Performed action.
        /// </summary>
        public SheetAction Action { get; internal set; }
        /// <summary>
        /// Change type
        /// </summary>
        public ChangeType ChangeType { get; internal set; }
        public object OldValue { get; internal set; }
        public object NewValue { get; internal set; }
    }
}
