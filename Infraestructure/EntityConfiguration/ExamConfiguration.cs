using Domain.AggregatesModel.ExamAggregate;
using Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.EntityConfiguration
{
    internal class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("Exams");
            builder.HasKey(e => e.Id);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(e => e.UploadedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.FileName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(e => e.FileSize)
                   .IsRequired();

            builder.Property(e => e.StoragePath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(e => e.Type)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(e => e.ProcessingStatus)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(e => e.ExtractedData)
                   .HasColumnType("text")
                   .IsRequired(false);

            builder.Property(e => e.AlteredValues)
                   .HasColumnType("text")
                   .IsRequired(false);

            builder.Property(e => e.CreatedAt)
                   .IsRequired();
        }
    }
}