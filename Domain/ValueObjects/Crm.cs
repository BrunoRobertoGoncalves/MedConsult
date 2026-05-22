using Domain.Core.Models;
using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public class Crm : ValueObject
    {
        public string Number { get; }
        private Crm(string number)
        {
            Number = number;
        }
        public static Crm Create(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new DomainException("CRM não pode ser vazio.");

            if(!Regex.IsMatch(number, @"^\d{4,6}\/[A-Z]{2}$"))
                throw new DomainException("CRM inválido. Formato esperado: 123456/SP");

            return new Crm(number);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }
    }
}
