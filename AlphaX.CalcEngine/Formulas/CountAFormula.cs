﻿namespace AlphaX.CalcEngine.Formulas
{
    public class CountAFormula : Formula
    {
        public CountAFormula():base("COUNTA")
        {
            this.MinArgs = 0;
            this.MaxArgs = int.MaxValue;
        }
        public override CalcValue Calculate(params CalcValue[] values)
        {
            int count = 0;
            for (int i = 0; i < values.Length; i++)
            {
                var calcValue = values[i];
                switch (calcValue.Kind)
                {
                    case CalcValueKind.Array:
                        foreach (var numValue in (object[,])calcValue.Value)
                        {
                            if(numValue !=null )
                            count++;
                        }
                        break;

                    case CalcValueKind.Number:
                    case CalcValueKind.Float:
                    case CalcValueKind.String:
                    case CalcValueKind.Date:
                        if(calcValue.Value != null)
                        count++;
                        break;

                    case CalcValueKind.Error:
                        break;

                    default:
                        throw new CalcEngineException("Invalid argument for formula.");
                }
            }

            return new CalcValue()
            {
                Kind = CalcValueKind.Number,
                Value = count
            };
        }

        public override string GetDescription()
        {
            return "Counts the number of cells in a range that are not empty";
        }
    }
}
