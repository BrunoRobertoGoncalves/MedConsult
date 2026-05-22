using Domain.Core.Models;
using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.UserAggregate
{
    public class User : Entity<Guid>
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Crm Crm { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRole Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<SpecialityType> _specialities = new();
        public IReadOnlyCollection<SpecialityType> Specialities => _specialities.AsReadOnly();

        private User() { }

        private User(string name, Email email, Crm crm, string passwordHash, UserRole role, IReadOnlyCollection<SpecialityType> specialities)
        {
            Name = name;
            Email = email;
            Crm = crm;
            PasswordHash = passwordHash;
            Role = role;
            _specialities = specialities.ToList();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public static User CreateUser(string name, string email, string crm, string passwordHash, UserRole role, IReadOnlyCollection<SpecialityType> specialities)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email cannot be empty.");
            if (string.IsNullOrWhiteSpace(crm))
                throw new DomainException("CRM cannot be empty.");
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainException("Password cannot be empty.");
            if (!specialities.Any())
                throw new DomainException("O médico deve ter pelo menos uma especialidade.");

            var emailObj = Email.Create(email);
            var crmObj = Crm.Create(crm);

            return new User(name, emailObj, crmObj, passwordHash, role, specialities);
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new DomainException("User is already inactive.");
            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive)
                throw new DomainException("User is already active.");
            IsActive = true;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainException("Name cannot be empty.");

            if (newName.Equals(Name))
                throw new DomainException("New name cannot be the same as the current name.");

            Name = newName;
        }

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new DomainException("Email cannot be empty.");
            if (newEmail.Equals(Email.Address))
                throw new DomainException("New email cannot be the same as the current email.");

            Email = Email.Create(newEmail);
        }

        public void UpdatePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new DomainException("Password cannot be empty.");
            if (newPasswordHash == PasswordHash)
                throw new DomainException("New password cannot be the same as the current password.");

            PasswordHash = newPasswordHash;
        }

        public void UpdateRole(UserRole newRole)
        {
            if (newRole == Role)
                throw new DomainException("New role cannot be the same as the current role.");

            Role = newRole;
        }

        public void AddSpeciality(SpecialityType speciality)
        {
            if (_specialities.Contains(speciality))
                throw new DomainException("Especialidade já cadastrada.");

            _specialities.Add(speciality);
        }

        public void RemoveSpeciality(SpecialityType speciality)
        {
            if (!_specialities.Contains(speciality))
                throw new DomainException("Especialidade não encontrada.");

            if (_specialities.Count == 1)
                throw new DomainException("O médico deve ter pelo menos uma especialidade.");

            _specialities.Remove(speciality);
        }
    }
}