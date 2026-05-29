using Domain.AggregatesModel.ClinicalCaseAggregate;
using Domain.AggregatesModel.ExamAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.EntityConfiguration
{
    internal class CaseExamConfiguration : IEntityTypeConfiguration<CaseExam>
    {
        public void Configure(EntityTypeBuilder<CaseExam> builder)
        {
            builder.ToTable("CaseExams");
            builder.HasKey(ce => ce.Id);

            builder.Property(ce => ce.ClinicalCaseId)
                   .IsRequired();

            builder.HasOne<Exam>()
                   .WithMany()
                   .HasForeignKey(ce => ce.ExamId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ce => ce.LinkedAt)
                   .IsRequired();

            builder.HasIndex(ce => new { ce.ClinicalCaseId, ce.ExamId })
                   .IsUnique();
        }
    }
}