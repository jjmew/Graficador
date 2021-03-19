using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graficador
{
    public class FormulaSolver
    {
        public enum TokenType
        {
            Operator,
            Variable,
            Operand 
        }

        public class Token
        {
            private TokenType type;
            private string value;

            public TokenType Type { get => type; set => type = value; }
            public string Value { get => value; set => this.value = value; }

            public Token(TokenType type, string value)
            {
                Type = type;
                this.Value = value;
            }
        }

        private static string[] Delimeters = { "*", "+", "-", "/", "^", "(","{","[","]","}",")"};
        private static string[] Operators =  { "*", "+", "-", "/", "^" };
        
        private static int GetOperatorPrecedence (string s)
        {
            s = s.Trim();
            if (s =="+" || s=="-")
            {
                return 1;
            }
            else if(s=="*" || s=="/")
            {
                return 2;
            }
            else if (s == "^")
            {
                return 3;

            }
            else if (s=="(")
            {
                return 4;
            }
            
            return 0;
        }
        public static bool IsOperator(string s)
        {
            s = s.Trim();

            foreach(string op in Operators)
            {
                if (op == s)
                    return true;
            }
            
            return false;
        }

        public static bool IsDelimeter(string s)
        {
            s = s.Trim();

            foreach (string op in Delimeters)
            {
                if (op == s)
                    return true;
            }

            return false;
        }
        public static List<Token> TokenizeInfixFormula(string infix)
        {
            List<Token> tokens = new List<Token>();
            string token = string.Empty;
            foreach(char c in infix)
            {
                if( IsDelimeter(c.ToString()))
                {
                    if (token != string.Empty)
                        tokens.Add(new Token(TokenType.Operand, token.Trim()));
                    tokens.Add( new Token(TokenType.Operator,c.ToString()));
                    token = string.Empty;
                }
                else
                {
                    token = token + c.ToString();            
                }
            }

            if( token!=string.Empty)
                tokens.Add(new Token(TokenType.Operand, token.Trim()));
            
            return tokens;
        }

        public static double EvaluatePostFix(List<Token> postfix)
        {
            double result =0;
            Stack<Token> tokenStacks = new Stack<Token>();

            foreach(Token t in postfix)
            {
                if(t.Type == TokenType.Operand)
                {
                    tokenStacks.Push(t);
                }
                else
                {
                    double x = double.Parse(tokenStacks.Pop().Value);
                    double y = double.Parse(tokenStacks.Pop().Value);
                    switch(t.Value)
                    {
                        case "+":
                            {
                                result = x + y;
                                break;
                            }

                        case "-":
                            {
                                result =  y-x;
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
                    }
                    tokenStacks.Push(new Token(TokenType.Operand, result.ToString()));
                }
            }
            return result;
        }

        public static string TokensToString(List<Token> tokens)
        {
            string s = String.Empty;
            foreach(Token t in tokens)
            {
                s = s + t.Value + " ";
            }

            return s;
        }

        public static bool TryParseInfixToPostFix(string infix, out string postfix)
        {
            List<Token> tokens;
            bool result = TryParseInfixToPostFix(infix, out tokens);
            postfix = TokensToString(tokens);
            return result;
        }
        public static bool TryParseInfixToPostFix(string infix, out List<Token> postfix)
        {
            postfix = null;
    
            Stack<Token> operators = new Stack<Token>();

            List<Token> tokens = TokenizeInfixFormula(infix);
            List<Token> postFixTokens = new List<Token>();

            foreach( Token t in tokens)
            {
                if(t.Type == TokenType.Operand)
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
