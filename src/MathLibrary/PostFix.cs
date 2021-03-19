using System;
using System.Collections.Generic;
using System.Text;

namespace MathLibrary
{
    internal class PostFix
    {
        private static string[] Delimeters = { "*", "+", "-", "/", "^", "(", "{", "[", "]", "}", ")","sin","cos" };
        private static string[] Operators = { "*", "+", "-", "/", "^", "sin", "cos" };
        private static int GetOperatorPrecedence(string s)
        {
            s = s.Trim();
            if (s == "+" || s == "-")
            {
                return 1;
            }
            else if (s == "*" || s == "/")
            {
                return 2;
            }
            else if (s == "^")
            {
                return 3;

            }
            else if (s == "sin" || s == "cos")
            {
                return 4;
            }
            else if (s == "(")
            {
                return 5;
            }

            return 0;
        }
        internal static bool IsOperator(string s)
        {
            s = s.Trim().ToLower();

            foreach (string op in Operators)
            {
                if (op == s)
                    return true;
            }

            return false;
        }

        internal static bool IsDelimeter(string s)
        {
            s = s.Trim();

            foreach (string op in Delimeters)
            {
                if (op == s)
                    return true;
            }

            return false;
        }

        internal static OperatorType GetOperatorType(string s)
        {
            if( s == "*" || s=="+" || s=="-" || s=="/" || s=="^")
            {
                return OperatorType.Binary;

            }
            else if(s=="sin"||s=="cos")
            {
                return OperatorType.Unary;
            }

            return OperatorType.Unkown;
        }
        internal static List<Token> TokenizeInfixFormula(string infix)
        {
            List<Token> tokens = new List<Token>();
            string token = string.Empty;
            
            infix = infix.Replace(" ", string.Empty); // removing the spaces simplifies the logic

            int i = 0;
            while( i < infix.Length)
            {
                bool foundDelimeter = false;
                foreach (string delimeter in Delimeters)
                {                    
                    if(delimeter.Length + i -1 < infix.Length && delimeter.ToLower() == infix.Substring(i, delimeter.Length).ToLower())
                    {
                        i = i + delimeter.Length;
                        if (token != string.Empty)
                            tokens.Add(new Token(TokenType.Operand, token.Trim()));
                        tokens.Add(new Token(TokenType.Operator, delimeter));
                        token = string.Empty;
                        foundDelimeter = true;
                        break;
                    }
                }

                if (!foundDelimeter)
                {
                    token = token + infix[i];
                    i++;
                }
            }

            if (token != string.Empty)
                tokens.Add(new Token(TokenType.Operand, token.Trim()));

            return tokens;
        }

        internal static double EvaluatePostFixForVariable(List<Token> postfix)
        {
            return EvaluatePostFixForVariable(postfix, string.Empty, 0);
        }

        internal static double EvaluatePostFixForVariable(List<Token> postfix , string variableName, double variableValue=0)
        {
            //This function needs to be update to account for unary operators

            double result = 0;
            Stack<Token> tokenStacks = new Stack<Token>();

            foreach (Token t in postfix)
            {
                if (t.Type == TokenType.Operand)
                {
                    if (t.Value.ToLower() == variableName.ToLower())
                    {
                        Token evaluatedToken = new Token(TokenType.Operand, variableValue.ToString());
                        tokenStacks.Push(evaluatedToken);
                    }
                    else
                    {
                        tokenStacks.Push(t);
                    }
                }
                else
                {
                    OperatorType opertatortype = GetOperatorType(t.Value);
                    double x=0, y=0;
                    if (opertatortype == OperatorType.Unary && tokenStacks.Count > 0)
                    {
                        x = double.Parse(tokenStacks.Pop().Value);
                    }
                    else if (opertatortype == OperatorType.Binary && tokenStacks.Count > 1)
                    {
                        x = double.Parse(tokenStacks.Pop().Value);
                        y = double.Parse(tokenStacks.Pop().Value);
                    }
                    else 
                    {
                        throw new InvalidPostFixExpressionException("The postfix < "+ postfix.ToString() +"> expression is invalid" );
                    }

                    switch (t.Value)
                    {
                        case "+":
                            {
                                result = x + y;
                                break;
                            }

                        case "-":
                            {
                                result = y - x;
                                break;
                            }

                        case "*":
                            {
                                result = x * y;
                                break;
                            }

                        case "/":
                            {
                                result = y / x;
                                break;
                            }
                        case "^":
                            {
                                result = Math.Pow(y, x);
                                break;
                            }
                        case "sin":
                            {
                                result = Math.Sin(x);
                                break;
                            }
                        case "cos":
                            {
                                result = Math.Cos(x);
                                break;
                            }
                    }
                    tokenStacks.Push(new Token(TokenType.Operand, result.ToString()));
                }
            }
            return result;
        }

        internal static string TokensToString(List<Token> tokens)
        {
            string s = String.Empty;
            foreach (Token t in tokens)
            {
                s = s + t.Value + " ";
            }

            return s;
        }

        internal static bool TryParseInfixToPostFix(string infix, out string postfix)
        {
            List<Token> tokens;
            bool result = TryParseInfixToPostFix(infix, out tokens);
            postfix = TokensToString(tokens);
            return result;
        }
        internal static bool TryParseInfixToPostFix(string infix, out List<Token> postfix)
        {
            postfix = null;
            Stack<Token> operators = new Stack<Token>();
            List<Token> tokens = TokenizeInfixFormula(infix);
            List<Token> postFixTokens = new List<Token>();

            foreach (Token t in tokens)
            {
                if (t.Type == TokenType.Operand)
                {
                    postFixTokens.Add(t);
                }
                else
                {
                    if (t.Value == ")")
                    {
                        bool foundOpenbracker = false;
                        while (operators.Count > 0 && foundOpenbracker == false)
                        {
                            Token t1 = operators.Pop();
                            if (t1.Value == "(")
                            {
                                foundOpenbracker = true;
                            }
                            else
                            {
                                postFixTokens.Add(t1);
                            }
                        }
                    }
                    else
                    {
                        while (operators.Count > 0 && operators.Peek().Value != "(" && GetOperatorPrecedence(operators.Peek().Value) >= GetOperatorPrecedence(t.Value))
                        {
                            postFixTokens.Add(operators.Pop());
                        }
                        operators.Push(t);
                    }

                }
            }

            while (operators.Count > 0)
            {
                postFixTokens.Add(operators.Pop());
            }

            postfix = postFixTokens;

            return true;
        }
    }
}
