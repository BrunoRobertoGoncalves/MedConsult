using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    internal class InvalidCaseStatusTransitionException : Exception
    {
        public InvalidCaseStatusTransitionException() { }
        public InvalidCaseStatusTransitionException(string message) : base(message) { }
        public InvalidCaseStatusTransitionException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
