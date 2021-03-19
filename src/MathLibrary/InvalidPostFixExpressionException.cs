using System;
using System.Collections.Generic;
using System.Text;

namespace MathLibrary
{
    public class InvalidPostFixExpressionException : Exception
    {
        public InvalidPostFixExpressionException() : base() { }
        public InvalidPostFixExpressionException(string message) : base(message) { }
        public InvalidPostFixExpressionException(string message, Exception inner) : base(message, inner) { }
    }
}
