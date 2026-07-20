using AlphaX.FormulaEngine;

namespace AlphaX.CalcEngine.Utils
{
    internal static class FormulaEngineConverter
    {
        public static CalcValue ConvertToCalcValue(object value)
        {
            if (value == null)
                return new CalcValue { Kind = CalcValueKind.Number, Value = 0 };
            
            if (value is double d)
                return new CalcValue { Kind = CalcValueKind.Float, Value = d };
            
            if (value is int i)
                return new CalcValue { Kind = CalcValueKind.Number, Value = i };
            
            if (value is bool b)
                return new CalcValue { Kind = CalcValueKind.Bool, Value = b };
            
            if (value is string s)
                return new CalcValue { Kind = CalcValueKind.String, Value = s };
            
            if (value is object[] array1D)
            {
                var transformedValue = new CalcValue[1, array1D.Length];
                for (var idx = 0; idx < array1D.Length; idx++)
                {
                    transformedValue[0, idx] = DataUtils.Transform(array1D[idx]);
                }
                return new CalcValue
                {
                    Kind = CalcValueKind.Array,
                    Value = transformedValue
                };
            }

            if (value is object[,] array)
            {
                var transformedValue = new CalcValue[array.GetLength(0), array.GetLength(1)];
                for (var r = 0; r < array.GetLength(0); r++)
                {
                    for (var c = 0; c < array.GetLength(1); c++)
                    {
                        transformedValue[r, c] = DataUtils.Transform(array[r, c]);
                    }
                }
                return new CalcValue
                {
                    Kind = CalcValueKind.Array,
                    Value = transformedValue
                };
            }

            return new CalcValue { Kind = CalcValueKind.Error, Value = new ValueError() };
        }
    }
}
