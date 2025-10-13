using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore;

namespace Infrastructure.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public static DatabaseContext Create(IMongoDatabase database) =>
            new(new DbContextOptionsBuilder<DatabaseContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName).Options);

        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<Record> Records { get; set; }
    }
}