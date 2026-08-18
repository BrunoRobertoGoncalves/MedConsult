using Domain.AggregatesModel.UserAggregate;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Infraestructure.EntityConfiguration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.OwnsOne(u => u.Email, e =>
            {
                e.Property(x => x.Address)
                 .HasColumnName("Email")
                 .IsRequired();
            });

            builder.OwnsOne(u => u.Crm, c =>
            {
                c.Property(x => x.Number)
                 .HasColumnName("Crm")
                 .IsRequired();
            });

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            builder.Property(u => u.Role)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(u => u.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            builder.Property<IReadOnlyCollection<SpecialityType>>("Specialities")
                   .HasField("_specialities")
                   .HasConversion(
                       v => string.Join(',', v.Select(s => s.ToString())),
                       v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                             .Select(s => Enum.Parse<SpecialityType>(s))
                             .ToList()
                   )
                   .HasColumnName("Specialities")
                   .HasColumnType("text")
                   .IsRequired();
        }
    }
}       