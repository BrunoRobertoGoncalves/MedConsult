using Domain.AggregatesModel.ClinicalCaseAggregate;
using Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.EntityConfiguration
{
    internal class OpinionConfiguration : IEntityTypeConfiguration<Opinion>
    {
        public void Configure(EntityTypeBuilder<Opinion> builder)
        {
            builder.ToTable("Opinions");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.ClinicalCaseId)
                   .IsRequired();

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(o => o.SpecialistId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(o => o.Content)
                   .IsRequired()
                   .HasColumnType("text");

            builder.Property(o => o.Agreement)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(o => o.ConductRecommendation)
                   .IsRequired()
                   .HasColumnType("text");

            builder.Property(o => o.CreatedAt)
                   .IsRequired();
        }
    }
}