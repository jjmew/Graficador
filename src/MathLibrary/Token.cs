using System;

namespace MathLibrary
{

    internal enum TokenType
    {
        Operator,
        Variable,
        Operand
    }

    internal enum OperatorType
    {
        Unary,
        Binary,
        Ternary,
        Unkown
    }

    internal class Token
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
}
