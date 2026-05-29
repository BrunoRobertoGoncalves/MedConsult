using Domain.AggregatesModel.ClinicalCaseAggregate;
using Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.EntityConfiguration
{
    internal class CaseReplyConfiguration : IEntityTypeConfiguration<CaseReply>
    {
        public void Configure(EntityTypeBuilder<CaseReply> builder)
        {
            builder.ToTable("CaseReplies");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ClinicalCaseId)
                   .IsRequired();

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(r => r.RequesterId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(r => r.Content)
                   .IsRequired()
                   .HasColumnType("text");

            builder.Property(r => r.CreatedAt)
                   .IsRequired();
        }
    }
}