using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OperatorInterface.Infrastructure.Adapters.Postgres;

namespace OperatorInterface.Api;

public class DatabaseDesignTimeDbContextFactory 
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseNpgsql();
        return new ApplicationDbContext(builder.Options);
    }
}