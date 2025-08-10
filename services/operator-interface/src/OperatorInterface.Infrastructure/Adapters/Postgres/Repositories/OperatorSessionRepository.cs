using Microsoft.EntityFrameworkCore;
using OperatorInterface.Core.Domain.Model;
using OperatorInterface.Core.Domain.SharedKernel;
using OperatorInterface.Core.Ports;

namespace OperatorInterface.Infrastructure.Adapters.Postgres.Repositories;

public class OperatorSessionRepository : IOperatorSessionRepository
    {
        private readonly ApplicationDbContext _context;
        
        public OperatorSessionRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<OperatorSession?> GetByIdAsync(SessionId sessionId)
        {
            var session = await _context.OperatorSessions
                .Include(x => x.ClientSessions)
                .FirstOrDefaultAsync(s => s.Id == sessionId.Value);

            return session;
        }
        
        public async Task AddAsync(OperatorSession session)
        {
            await _context.OperatorSessions.AddAsync(session);
        }
        
        public Task UpdateAsync(OperatorSession session)
        {
            _context.OperatorSessions.Update(session);
            return Task.CompletedTask;
        }
    }