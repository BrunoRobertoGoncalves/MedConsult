using Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.EntityConfiguration
{
    internal class ClinicalCaseConfiguration : IEntityTypeConfiguration<ClinicalCase>
    {
        public void Configure(EntityTypeBuilder<ClinicalCase> builder)
        {
            builder.ToTable("ClinicalCases");
            builder.HasKey(cc => cc.Id);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(cc => cc.RequestedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(cc => cc.AssignedSpecialistId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            builder.Property(cc => cc.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(cc => cc.ClinicalContext)
                   .IsRequired()
                   .HasColumnType("text");

            builder.Property(cc => cc.DiagnosticHypothesis)
                   .IsRequired()
                   .HasColumnType("text");

            builder.Property(cc => cc.Question)
                   .IsRequired()
                   .HasColumnType("text");

            builder.Property(cc => cc.SpecialityRequired)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(cc => cc.Status)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(cc => cc.CreatedAt)
                   .IsRequired();

            builder.HasMany(cc => cc.Opinions)
                   .WithOne()
                   .HasForeignKey(o => o.ClinicalCaseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(cc => cc.Replies)
                   .WithOne()
                   .HasForeignKey(r => r.ClinicalCaseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(cc => cc.StatusHistory)
                   .WithOne()
                   .HasForeignKey(h => h.ClinicalCaseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(cc => cc.CaseExams)
                   .WithOne()
                   .HasForeignKey(ce => ce.ClinicalCaseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}