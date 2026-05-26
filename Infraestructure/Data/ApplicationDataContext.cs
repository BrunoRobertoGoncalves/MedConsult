using Domain.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Data
{
    public class ApplicationDataContext : DbContext, IUnitOfWork
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
        {
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public async Task<bool> SaveEntitiesAsync()
        {
            await SaveChangesAsync();
            return true;
        }
    }
}
