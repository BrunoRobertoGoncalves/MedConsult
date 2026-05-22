using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    internal class CaseAlreadyAssignedException : Exception
    {
        public CaseAlreadyAssignedException() { }
        public CaseAlreadyAssignedException(string message) : base(message) { }
        public CaseAlreadyAssignedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
