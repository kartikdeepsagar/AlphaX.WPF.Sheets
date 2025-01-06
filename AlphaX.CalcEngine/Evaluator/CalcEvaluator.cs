using AlphaX.CalcEngine.Formulas;
using AlphaX.CalcEngine.Parsers;
using AlphaX.CalcEngine.Resources;
using AlphaX.CalcEngine.Utils;
using System;
using System.Collections.Generic;

namespace AlphaX.CalcEngine
{
    internal class CalcEvaluator
    {
        private Dictionary<string, Formula> _formulas;
        private IDataProvider _dataProvider;

        public CalcEvaluator(IDataProvider dataProvider)
        {
            _formulas = new Dictionary<string, Formula>();
            _dataProvider = dataProvider;
            RegisterInbuiltFormulas();
        }

        public CalcValue EvaluateExpressionTree(CalcParserResult root, string defaultSheetName = "")
        {
            return EvaluateNode(root, defaultSheetName);
        }

        private CalcValue EvaluateNode(CalcParserResult node, string defaultSheetName = "")
        {
            if (node == null)
                throw new CalcEngineException(ExceptionMessages.InvalidFormula);

            switch (node.Kind)
            {
                case CalcParserResultKind.Number:
                    return new CalcValue() { 
                        Kind = CalcValueKind.Number,
                        Value = node.ComputedValue
                    };
                case CalcParserResultKind.Float:
                    return new CalcValue()
                    {
                        Kind = CalcValueKind.Float,
                        Value = node.ComputedValue
                    };
                case CalcParserResultKind.Formula:
                    return EvaluateFormula(node, defaultSheetName);
                case CalcParserResultKind.CellRef:
                    return EvaluateCellRef(node, defaultSheetName);
                case CalcParserResultKind.CellRangeRef:
                    return EvaluateCellRangeRef(node, defaultSheetName);
                case CalcParserResultKind.String:
                    return new CalcValue()
                    {
                        Kind = CalcValueKind.String,
                        Value = node.ComputedValue
                    };
                case CalcParserResultKind.Bool:
                    return new CalcValue()
                    {
                        Kind = CalcValueKind.Bool,
                        Value = node.ComputedValue
                    };
                case CalcParserResultKind.VarName:
                    return EvaluateCustomName(node, defaultSheetName);
            }

            var op = ((CalcOperators)node.ComputedValue);


            var leftVal = EvaluateNode(node.Childs[0], defaultSheetName);
            var rightVal = EvaluateNode(node.Childs[1], defaultSheetName);

            // check for string operator
            if (op == CalcOperators.ConcateString)
            {
                if (leftVal.Kind != CalcValueKind.String || rightVal.Kind != CalcValueKind.String)
                {
                    return new CalcValue()
                    {
                        Kind = CalcValueKind.Error,
                        Value = new ValueError()
                    };
                }

                return new CalcValue()
                {
                    Kind = CalcValueKind.String,
                    Value = (string)leftVal.Value + (string)rightVal.Value,
                };
            }


            // arithmetic operators
            double compLeftVal, compRightVal;
            switch (leftVal.Kind)
            {
                case CalcValueKind.Number:
                case CalcValueKind.Float:
                case CalcValueKind.Bool:
                    compLeftVal = Convert.ToDouble(leftVal.Value);
                    break;
                default:
                    return new CalcValue()
                    {
                        Kind = CalcValueKind.Error,
                        Value = new ValueError()
                    };
            }

            switch (rightVal.Kind)
            {
                case CalcValueKind.Number:
                case CalcValueKind.Float:
                case CalcValueKind.Bool:
                    compRightVal = Convert.ToDouble(rightVal.Value);
                    break;
                default:
                    return new CalcValue()
                    {
                        Kind = CalcValueKind.Error,
                        Value = new ValueError()
                    };
            }

            switch (op)
            {
                case CalcOperators.Plus: 
                    return new CalcValue() { 
                        Kind = CalcValueKind.Float,
                        Value = compLeftVal + compRightVal
                    };
                case CalcOperators.Minus:
                    return new CalcValue()
                    {
                        Kind = CalcValueKind.Float,
                        Value = compLeftVal - compRightVal
                    };
                case CalcOperators.Multiply:
                    return new CalcValue()
                    {
                        Kind = CalcValueKind.Float,
                        Value = compLeftVal * compRightVal
                    };
                case CalcOperators.Divide:
                    if(compRightVal == 0)
                    {
                        return new CalcValue() {
                            Kind= CalcValueKind.Error,
                            Value= new DivideByZeroError()
                        };
                    }else
                    {
                        return new CalcValue()
                        {
                            Kind = CalcValueKind.Float,
                            Value = compLeftVal / compRightVal
                        };
                    }
            }

            throw new Exception(ExceptionMessages.UnknownOperator);
        }

        private CalcValue EvaluateCustomName(CalcParserResult varname, string defaultSheetName = "")
        {
            // currently custom names are not supported, will result in #Name error
            // TODO: update to add support for custom names
            return new CalcValue()
            {
                Kind = CalcValueKind.Error,
                Value = new NameError()
            };
        }

        private CalcValue EvaluateFormula(CalcParserResult formula, string defaultSheetName = "")
        {
            if (_formulas.ContainsKey(formula.Value))
            {
                var f = _formulas[formula.Value];
                var values = new List<CalcValue>();
                if(formula.Childs.Length > f.MaxArgs || formula.Childs.Length < f.MinArgs)
                {
                    throw new CalcEngineException(ExceptionMessages.InvalidFormulaArgs);
                }
                foreach(var arg in formula.Childs)
                {
                    values.Add(EvaluateNode(arg, defaultSheetName));
                }

                return f.Calculate(values.ToArray());
            }
            else
            {
                return new CalcValue()
                {
                    Kind= CalcValueKind.Error,
                    Value = new NameError()
                };
            }
        }

        private CalcValue EvaluateCellRef(CalcParserResult cellRefResult, string defaultSheetName = "")
        {
            var cellRef = (CellRef)cellRefResult.ComputedValue;
            var value = _dataProvider.GetValue(string.IsNullOrEmpty(cellRef.SheetName) ? defaultSheetName: cellRef.SheetName, cellRef.Row, cellRef.Column);

            return DataUtils.Transform(value);
        }

        private CalcValue EvaluateCellRangeRef(CalcParserResult cellRangeRefResult, string defaultSheetName = "")
        {
            var rangeRef = (CellRangeRef)cellRangeRefResult.ComputedValue;
            var value = _dataProvider.GetRangeValue(string.IsNullOrEmpty(rangeRef.SheetName) ? defaultSheetName : rangeRef.SheetName, rangeRef.TopRow, rangeRef.LeftColumn, rangeRef.RowCount, rangeRef.ColumnCount);

            var transformedValue = new CalcValue[value.GetLength(0), value.GetLength(1)];
            for(var r = 0; r < value.GetLength(0); r++)
            {
                for(var c = 0; c < value.GetLength(1); c++)
                {
                    transformedValue[r, c] = (DataUtils.Transform(value[r, c]));
                }
            }

            return new CalcValue()
            {
                Kind = CalcValueKind.Array,
                Value = transformedValue
            };
        }

        private void RegisterInbuiltFormulas()
        {
            RegisterFormula(new SumFormula());
            RegisterFormula(new CountFormula());
            RegisterFormula(new AverageFormula());
            RegisterFormula(new CountAFormula());
        }

        public void RegisterFormula(Formula formula)
        {
            if (_formulas.ContainsKey(formula.Name.ToLowerInvariant()))
                throw new Exception(string.Format(ExceptionMessages.FormulaRegisterError, formula.Name));

            _formulas.Add(formula.Name.ToUpperInvariant(), formula);
        }

        public IEnumerable<Formula> GetRegisteredFormulas()
        {
            return _formulas.Values;
        }

        public void FillDependencies(CalcParserResult node, IList<object> dependencies)
        {
            if (node == null)
            {
                return;
            }

            switch (node.Kind)
            {
                case CalcParserResultKind.CellRef:
                case CalcParserResultKind.CellRangeRef:
                    dependencies.Add(node.ComputedValue);
                    return;
                case CalcParserResultKind.Formula:
                case CalcParserResultKind.Operator:
                    foreach(var child in node.Childs)
                    {
                        FillDependencies(child, dependencies);
                    }
                    return;
                default:
                    return;
            }

        }
    }
}
