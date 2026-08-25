using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class InvalidCaseStatusTransitionException : DomainException
    {
        public InvalidCaseStatusTransitionException() : base("Invalid case status transition.") { }
        public InvalidCaseStatusTransitionException(string message) : base(message) { }
        public InvalidCaseStatusTransitionException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
