using Domain.AggregatesModel.ClinicalCaseAggregate;
using Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.EntityConfiguration
{
    internal class CaseStatusHistoryConfiguration : IEntityTypeConfiguration<CaseStatusHistory>
    {
        public void Configure(EntityTypeBuilder<CaseStatusHistory> builder)
        {
            builder.ToTable("CaseStatusHistories");
            builder.HasKey(h => h.Id);

            builder.Property(h => h.ClinicalCaseId)
                   .IsRequired();

            builder.Property(h => h.FromStatus)
                   .HasConversion<string>()
                   .IsRequired(false);

            builder.Property(h => h.ToStatus)
                   .HasConversion<string>()
                   .IsRequired();

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(h => h.ChangedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(h => h.ChangedAt)
                   .IsRequired();
        }
    }
}