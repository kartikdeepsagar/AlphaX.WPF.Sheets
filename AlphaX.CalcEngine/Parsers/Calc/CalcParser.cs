using System;
using System.Collections.Generic;
using System.Linq;
using AlphaX.CalcEngine.Resources;

namespace AlphaX.CalcEngine.Parsers
{
    internal class CalcParser
    {
        // operator priority
        private Dictionary<string, int> _operatorPriority;

        public CalcParser()
        {
            _operatorPriority = new Dictionary<string, int>()
            {
                { ParserTokens.Plus, 2 },
                { ParserTokens.Minus, 2 },
                { ParserTokens.Multiply, 3 },
                { ParserTokens.Divide, 3 }
            };
        }

        // run parser and return the evaluated calc chain
        public CalcParserResult Run(string equation)
        {
            var state = new ParserState()
            {
                InputString = equation,
                Index = 0,
                IsError = false,
                Result = null,
                Error = null
            };

            var infix = ParserProvider.EquationParser.Parse(state);
            if (infix.IsError)
            {
                throw new Exception(string.Format(ExceptionMessages.Invalid_Equation, infix.Error));
            }else if (infix.Index < equation.Length)
            {
                throw new Exception(string.Format(ExceptionMessages.Invalid_Equation, "Expected end of input"));
            }
            var calcParseResultArr = (infix.Result as ArrayResult).Value.Select(el => el as CalcParserResult).ToArray();
            var prefix = InfixToPrefix(calcParseResultArr);
            var calcChain = PrefixToCalcChain(prefix);
            return calcChain;
        }

        // convert prefix callparseresult array to calc result tree
        private CalcParserResult PrefixToCalcChain(CalcParserResult[] prefix)
        {
            var pendingNodes = new Stack<CalcParserResult>();
            CalcParserResult root = null;

            for(var i = 0; i < prefix.Length; i++)
            {
                if(root == null)
                {
                    root = prefix[i];
                }

                if(pendingNodes.Count > 0)
                {
                    var lastPending = pendingNodes.Peek();
                    lastPending.AddChild(prefix[i]);
                    if(lastPending.Childs.Length == 2)
                    {
                        pendingNodes.Pop();
                    }
                }

                if(prefix[i].Kind == CalcParserResultKind.Operator)
                {
                    pendingNodes.Push(prefix[i]);
                }
            }

            if (pendingNodes.Count > 0)
            {
                throw new Exception(ExceptionMessages.InvalidOperandsCount);
            }

            return root;
        }

        // convert CalcParseResult array to prefix form
        private CalcParserResult[] InfixToPrefix(CalcParserResult[] parserResult)
        {
            var reverse = parserResult.Reverse().Select((el) => {
                switch (el.Kind)
                {
                    case CalcParserResultKind.OpenParan: return new CalcParserResult(CalcParserResultKind.CloseParan, ParserTokens.CloseBracket);
                    case CalcParserResultKind.CloseParan: return new CalcParserResult(CalcParserResultKind.OpenParan, ParserTokens.OpenBracket); 
                    default: return el;
                }
            }).ToArray();

            return InfixToPostfix(reverse).Reverse().ToArray();
        }

        // convert CalcParseResult array to postfix form
        private CalcParserResult[] InfixToPostfix(CalcParserResult[] parserResult)
        {
            var operatorStack = new Stack<CalcParserResult>();
            var outputList = new List<CalcParserResult>();

            foreach (var cur in parserResult)
            {
                switch (cur.Kind)
                {
                    case CalcParserResultKind.CloseParan:
                        var op = operatorStack.Count > 0 ? operatorStack.Pop() : null;
                        while (op != null && op.Kind != CalcParserResultKind.OpenParan)
                        {
                            outputList.Add(op);
                            op = operatorStack.Count > 0 ? operatorStack.Pop() : null;
                        }
                        break;

                    case CalcParserResultKind.OpenParan:
                        operatorStack.Push(cur);
                        break;

                    case CalcParserResultKind.Operator:
                        int c = operatorStack.Count;
                        // stack is empty, push operator
                        if (c == 0)
                        {
                            operatorStack.Push(cur);
                            break;
                        }
                        else
                        {
                            var lastOperator = operatorStack.Peek();
                            if (lastOperator.Kind == CalcParserResultKind.OpenParan ||
                                _operatorPriority[cur.Value] > _operatorPriority[lastOperator.Value])
                            {
                                operatorStack.Push(cur);
                            }
                            else
                            {
                                while (lastOperator != null &&
                                    lastOperator.Kind == CalcParserResultKind.Operator &&
                                    _operatorPriority[lastOperator.Value] >= _operatorPriority[cur.Value])
                                {
                                    outputList.Add(lastOperator);
                                    operatorStack.Pop();
                                    lastOperator = operatorStack.Count > 0 ? operatorStack.Peek() : null;
                                }

                                operatorStack.Push(cur);
                            }
                        }
                        break;

                    default: outputList.Add(cur); break;
                }


            }

            
            while (operatorStack.Count > 0)
            {
                outputList.Add(operatorStack.Pop());
            }

            return outputList.ToArray();
        }
    }
}
