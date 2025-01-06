using System;

namespace AlphaX.CalcEngine.Utils
{
    internal class DataUtils
    {
        
        public static CalcValue Transform(object data) {
            if (data == null)
            {
                return null;
            }else if(data is CalcValue)
            {
                return (CalcValue)data;
            }
            Type type = data.GetType();
            CalcValueKind kind;

            switch (Type.GetTypeCode(type)) {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return null;
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt32:
                case TypeCode.UInt16:
                case TypeCode.UInt64:
                    kind = CalcValueKind.Number;
                    break;
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    kind = CalcValueKind.Float;
                    break;
                case TypeCode.DateTime:
                    kind = CalcValueKind.Date;
                    break;
                case TypeCode.Boolean:
                    kind = CalcValueKind.Bool;
                    break;
                case TypeCode.Char:
                case TypeCode.String:
                    kind = CalcValueKind.String;
                    break;
                default:
                    kind = CalcValueKind.Unknown;
                    break;
            }
            return new CalcValue { Kind = kind, Value = data };
        }

    }
}
