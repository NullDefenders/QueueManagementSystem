using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OperatorInterface.Core.Domain.Model;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Infrastructure.Adapters.Postgres.EntityConfigurations;

public class ClientSessionConfiguration : IEntityTypeConfiguration<ClientSession>
{
    public void Configure(EntityTypeBuilder<ClientSession> builder)  
    {
        builder.ToTable("client_sessions");
            
        builder.HasKey(x => x.Id);
            
        builder
            .Property(entity => entity.Id)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .IsRequired();
        
        builder.Property(x => x.TicketNumber)
            .HasConversion(
                ticketNumber => ticketNumber.Value,
                value => new TicketNumber(value))
            .HasColumnName("ticket_number")
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(x => x.AssignmentTime)
            .HasColumnName("assignment_time")
            .IsRequired();
            
        builder.Property(x => x.StartTime)
            .HasColumnName("start_time");
            
        builder.Property(x => x.EndTime)
            .HasColumnName("end_time");
            
        builder.Property(x => x.Result)
            .HasConversion<string>()
            .HasColumnName("result");
            
        builder
            .Property("OperatorSessionId")
            .HasColumnName("operator_session_id")
            .IsRequired();
    }
}