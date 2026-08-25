using Domain.Enums;

namespace Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Crm { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public IReadOnlyCollection<SpecialityType> Specialities { get; set; } = Array.Empty<SpecialityType>();
    }
}
