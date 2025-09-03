using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OperatorInterface.Core.Domain.Model;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Infrastructure.Adapters.Postgres.EntityConfigurations;

public class OperatorSessionConfiguration : IEntityTypeConfiguration<OperatorSession>
{
    public void Configure(EntityTypeBuilder<OperatorSession> builder)
    {
        builder.ToTable("operator_sessions");

        builder.HasKey(x => x.Id);

        builder
            .Property(c => c.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");
        
        builder.Property(x => x.OperatorId)
            .HasConversion(
                operatorId => operatorId.Value,
                value => new OperatorId(value))
            .HasColumnName("operator_id")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.WorkplaceCode)
            .HasConversion(
                workplaceCode => workplaceCode.Value,
                value => new WorkplaceCode(value))
            .HasColumnName("workplace_code")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasColumnName("status")
            .IsRequired();

        builder.Property(x => x.SessionStartTime)
            .HasColumnName("session_start_time");

        builder.Property(x => x.SessionEndTime)
            .HasColumnName("session_end_time");
        
        // Configure AssignedServices as PostgreSQL array of custom type
        builder.OwnsMany(x => x.AssignedServices, j =>
        {
            j.ToJson("assigned_services");
        });
        
        builder.Ignore(x => x.CurrentClientSession);
    }
}