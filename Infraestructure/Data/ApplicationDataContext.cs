using Domain.Core.Data;
using Microsoft.EntityFrameworkCore;
using Domain.AggregatesModel.ClinicalCaseAggregate;
using Domain.AggregatesModel.UserAggregate;
using Domain.AggregatesModel.ExamAggregate;
using Infraestructure.EntityConfiguration;

namespace Infraestructure.Data
{
    public class ApplicationDataContext : DbContext, IUnitOfWork
    {
        public DbSet<ClinicalCase> ClinicalCases { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<CaseReply> CaseReplies { get; set; }
        public DbSet<CaseStatusHistory> CaseStatusHistories { get; set; }
        public DbSet<CaseExam> CaseExams { get; set; }

        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClinicalCaseConfiguration());

            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.ApplyConfiguration(new ExamConfiguration());

            modelBuilder.ApplyConfiguration(new OpinionConfiguration());

            modelBuilder.ApplyConfiguration(new CaseReplyConfiguration());

            modelBuilder.ApplyConfiguration(new CaseStatusHistoryConfiguration());

            modelBuilder.ApplyConfiguration(new CaseExamConfiguration());
        }

    }
}
