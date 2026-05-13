using Microsoft.EntityFrameworkCore;
using SharedPool.Domain.Entities;
using SharedPool.Infrastructure.Interceptors;
using System.Reflection;

namespace SharedPool.Infrastructure.Contexts
{
    public class SharedPoolDbContext : DbContext
    {
        public SharedPoolDbContext(DbContextOptions<SharedPoolDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseSplit> ExpenseSplits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurations klasöründeki tüm IEntityTypeConfiguration arayüzünü uygulayan sınıfları otomatik bulur ve uygular.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        // Infrastructure/Contexts/SplitwiseDbContext.cs dosyasına eklenecek kısım:
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Interceptor'ı sisteme tanıtıyoruz
            optionsBuilder.AddInterceptors(new AuditableEntityInterceptor());
            base.OnConfiguring(optionsBuilder);
        }
    }
}
