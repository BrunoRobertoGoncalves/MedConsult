using Domain.Core.Models;
using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Address { get; }
        private Email(string address) 
        { 
            Address = address;
        }
        public static Email Create(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new DomainException("E-mail não pode ser vazio.");

            if (!Regex.IsMatch(address, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new DomainException($"E-mail '{address}' é inválido.");

            return new Email(address);
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }
    }
}
