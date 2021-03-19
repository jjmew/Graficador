using System;
using System.Collections.Generic;
using System.Text;

namespace MathLibrary
{
    public class PostFixExpression
    {
        internal List<Token> TokenizedExpression;
        public PostFixExpression()
        {
            this.TokenizedExpression = null;
        }

        public bool TryParsePostFixExpression(string inFixExpression)
        {

            if (PostFix.TryParseInfixToPostFix(inFixExpression, out TokenizedExpression))
            {
                return true;
            }
            else
            {
                TokenizedExpression = null;
                return false;
            }
        }

        /// <summary>
        /// Tries to evaluate a postfix expresion given a variable and a value
        /// </summary>
        /// <param name="variable">The string that represent the variable (i.ex "x" in the expression "x+1")</param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns>True if the evaluation was succesfull</returns>
        public bool TryToEvaluateForVariable(string variable, double value, ref double result)
        {
            if(TokenizedExpression != null)
            {
                result = PostFix.EvaluatePostFixForVariable(this.TokenizedExpression, variable, value);
                return true;
            }
            return false;
        }

    }
}
