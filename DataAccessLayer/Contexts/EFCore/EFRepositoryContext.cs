using Microsoft.EntityFrameworkCore;
using EntityLayer.Models;
using DataAccessLayer.Contexts.EFCore.Configs;

namespace DataAccessLayer.Contexts.EFCore
{
    public class EFRepositoryContext : DbContext
    {
        public EFRepositoryContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfig());
        }
    }
}
