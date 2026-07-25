using System;

namespace AlphaX.Sheets
{
    internal abstract class HeadersBase : IDisposable
    {
        protected WorkSheet _workSheet;

        public WorkSheet WorkSheet => _workSheet;

        internal HeadersBase(WorkSheet workSheet)
        {
            _workSheet = workSheet;
        }

        public virtual void Dispose()
        {
            _workSheet = null;
        }
    }
}
