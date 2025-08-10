using Microsoft.EntityFrameworkCore;
using OperatorInterface.Core.Domain.Model;
using OperatorInterface.Infrastructure.Adapters.Postgres.EntityConfigurations;

namespace OperatorInterface.Infrastructure.Adapters.Postgres;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<OperatorSession> OperatorSessions { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OperatorSessionConfiguration());
        modelBuilder.ApplyConfiguration(new ClientSessionConfiguration());
    }
}