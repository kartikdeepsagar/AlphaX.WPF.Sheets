using AlphaX.CalcEngine.Parsers;
using AlphaX.FormulaEngine;
using System.Threading.Tasks;

namespace AlphaX.CalcEngine.Evaluator
{
    public class CalcEngineContext : IEngineContext
    {
        private readonly IDataProvider _dataProvider;
        private string _currentSheetName;

        public CalcEngineContext(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void SetCurrentSheet(string sheetName)
        {
            _currentSheetName = sheetName;
        }

        public Task<object> Resolve(string key)
        {
            if (key.Contains(":"))
            {
                var rangeRef = new CellRangeRef(key);
                var sheetName = string.IsNullOrEmpty(rangeRef.SheetName) ? _currentSheetName : rangeRef.SheetName;
                var value = _dataProvider.GetRangeValue(sheetName, rangeRef.TopRow, rangeRef.LeftColumn, rangeRef.RowCount, rangeRef.ColumnCount);
                if (value is object[,] array2D)
                {
                    var flatArray = new object[array2D.Length];
                    int index = 0;
                    for (int r = 0; r < array2D.GetLength(0); r++)
                    {
                        for (int c = 0; c < array2D.GetLength(1); c++)
                        {
                            flatArray[index++] = array2D[r, c];
                        }
                    }
                    return Task.FromResult<object>(flatArray);
                }
                return Task.FromResult<object>(value);
            }
            else
            {
                var cellRef = new CellRef(key);
                var sheetName = string.IsNullOrEmpty(cellRef.SheetName) ? _currentSheetName : cellRef.SheetName;
                var value = _dataProvider.GetValue(sheetName, cellRef.Row, cellRef.Column);
                return Task.FromResult<object>(value);
            }
        }
    }
}
