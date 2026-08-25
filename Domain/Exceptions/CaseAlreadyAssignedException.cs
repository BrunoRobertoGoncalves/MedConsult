using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class CaseAlreadyAssignedException : DomainException
    {
        public CaseAlreadyAssignedException() : base("Case is already assigned to a specialist.") { }
        public CaseAlreadyAssignedException(string message) : base(message) { }
        public CaseAlreadyAssignedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
