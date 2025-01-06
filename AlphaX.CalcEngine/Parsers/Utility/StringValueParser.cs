using System.Text;

namespace AlphaX.CalcEngine.Parsers
{
    internal class StringValueParser : Parser
    {

        public StringValueParser()
        {

        }

        public override ParserState Parse(ParserState state)
        {

            if (state.IsError)
            {
                return state;
            }

            var str = state.InputString.Substring(state.Index);

            if (!str.StartsWith("\""))
            {
                return UpdateError(state, new ParserError($"No match found, expected \" but got {str}"));
            }

            var strBuffer = new StringBuilder();
            int i = 1;
            for(; i < str.Length; i++)
            {
                if(str[i] == '"' && (i != str.Length - 1 && str[i + 1] == '"'))
                {
                    strBuffer.Append(str[i]);
                    i++;
                }else if(str[i] == '"' && (i == str.Length - 1 || str[i + 1] != '"'))
                {
                    var res = strBuffer.ToString();
                    return UpdateState(state, state.Index + i + 1, new StringResult(res));
                }else
                {
                    strBuffer.Append(str[i]);
                }
            }

            
            return UpdateError(state, new ParserError ($"No match found, expected string value but got {str}" ));
            
        }
    }
}
