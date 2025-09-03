using Microsoft.EntityFrameworkCore;
using OperatorInterface.Core.Domain.Model;
using OperatorInterface.Core.Domain.SharedKernel;
using OperatorInterface.Infrastructure.Adapters.Postgres;
using OperatorInterface.Infrastructure.Adapters.Postgres.Repositories;
using Testcontainers.PostgreSql;

namespace OperatorInterface.IntegrationTests.Repositories
{
    public class OperatorSessionRepositoryShould : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:17.5")
            .WithDatabase("order")
            .WithUsername("username")
            .WithPassword("secret")
            .WithCleanUp(true)
            .Build();
        
        private ApplicationDbContext _context = null!;
        
        // Test data
        private readonly OperatorId _testOperatorId = new("OP123");
        private readonly WorkplaceCode _testWorkplaceCode = new("WP001");
        private readonly TicketNumber _testTicketNumber = new("T12345");
        private readonly List<ServiceInfo> _testAssignedServices = new()
        {
            new ServiceInfo("S001", "Passport Service"),
            new ServiceInfo("S002", "Visa Service")
        };

        public OperatorSessionRepositoryShould()
        {
        }

        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();
        
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(
                    _postgreSqlContainer.GetConnectionString(),
                    sqlOptions => { sqlOptions.MigrationsAssembly("OperatorInterface.Infrastructure"); })
                .Options;
            _context = new ApplicationDbContext(contextOptions);
            _context.Database.Migrate();
        }

        public async Task DisposeAsync()
        {
            await _postgreSqlContainer.DisposeAsync().AsTask();
        }

        [Fact]
        public async Task SaveNewOperatorSession_WhenValidDataProvided()
        {
            // Arrange - Create a new operator session with all required data
            var session = OperatorSession.Create(
                _testOperatorId,
                _testWorkplaceCode,
                _testAssignedServices);

            // Act - Save the session to repository
            var repository = new OperatorSessionRepository(_context);
            var unitOfWork = new UnitOfWork(_context);

            await repository.AddAsync(session);
            await unitOfWork.SaveChangesAsync();
            
            // Assert - Verify session was saved correctly
            var savedSession = await repository.GetByIdAsync(session.SessionId);
            
            Assert.NotNull(savedSession);
            Assert.Equal(_testOperatorId, savedSession.OperatorId);
            Assert.Equal(_testWorkplaceCode, savedSession.WorkplaceCode);
            Assert.Equal(SessionStatus.Authorized, savedSession.Status);
            
            // Verify assigned services were saved in separate table
            var savedServices = savedSession.AssignedServices;
            
            Assert.Equal(2, savedServices.Count);
            Assert.Contains(savedServices, s => s.ServiceCode == "S001");
            Assert.Contains(savedServices, s => s.ServiceCode == "S002");
        }
        
        [
        
            Fact]
        public async Task Maintain_Session_History()
        {
            // Arrange - Save initial session
            var session = OperatorSession.Create(
                _testOperatorId,
                _testWorkplaceCode,
                _testAssignedServices);
            
            var repository = new OperatorSessionRepository(_context);
            var unitOfWork = new UnitOfWork(_context);

            await repository.AddAsync(session);
            await unitOfWork.SaveChangesAsync();
            
            // Modify session state
            session.OpenSession();
            session.RequestClient();
            session.AssignClient(_testTicketNumber);
            session.StartClientSession();
            session.CompleteClientSession();
            
            await repository.UpdateAsync(session);
            await unitOfWork.SaveChangesAsync();
            
            // Assert - Verify session was updated correctly
            var updatedSession = await repository.GetByIdAsync(session.SessionId);
            
            // Modify session state
            updatedSession.RequestClient();
            updatedSession.AssignClient(_testTicketNumber);
            updatedSession.StartClientSession();
            updatedSession.CompleteClientSession();
            
            await repository.UpdateAsync(session);
            await unitOfWork.SaveChangesAsync();
            
            Assert.NotNull(updatedSession);
            Assert.Equal(SessionStatus.ReadyToWork, updatedSession.Status);
            Assert.Equal(2, updatedSession.ClientSessions.Count);
        }
/*
        [Fact]
        public async Task ThrowException_WhenSavingDuplicateSessionId()
        {
            // Arrange - Create and save a session first
            var session = OperatorSession.Create(
                _testSessionId,
                _testOperatorId,
                _testWorkplaceCode,
                _testAssignedServices);
            
            var repository = new OperatorSessionRepository(_context);
            var unitOfWork = new UnitOfWork(_context);
            
            await repository.AddAsync(session);

            // Create another session with same ID
            var duplicateSession = OperatorSession.Create(
                _testSessionId, // Same ID
                new OperatorId("OP456"),
                new WorkplaceCode("WP002"),
                _testAssignedServices);

            // Act & Assert - Should throw exception when saving duplicate
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repository.AddAsync(duplicateSession));

            await unitOfWork.SaveChangesAsync();
            
            Assert.Contains("already exists", exception.Message);
            Assert.Contains("Use UpdateAsync instead", exception.Message);
        }

        [Fact]
        public async Task ReturnOperatorSession_WhenGetByIdWithExistingSession()
        {
            // Arrange - Save a session with client sessions and assigned services
            var session = CreateCompleteTestSession();
            await _repository.SaveAsync(session);

            // Act - Retrieve session by ID
            var retrievedSession = await _repository.GetByIdAsync(_testSessionId);

            // Assert - Verify all data was loaded correctly
            Assert.NotNull(retrievedSession);
            Assert.Equal(_testSessionId, retrievedSession.SessionId);
            Assert.Equal(_testOperatorId, retrievedSession.OperatorId);
            Assert.Equal(_testWorkplaceCode, retrievedSession.WorkplaceCode);
            
            // Verify assigned services were loaded
            Assert.Equal(2, retrievedSession.AssignedServices.Count);
            Assert.Contains(retrievedSession.AssignedServices, s => s.ServiceCode == "S001");
            Assert.Contains(retrievedSession.AssignedServices, s => s.ServiceCode == "S002");
            
            // Verify client sessions were loaded
            Assert.Single(retrievedSession.ClientSessions);
            Assert.Equal(_testTicketNumber, retrievedSession.ClientSessions.First().TicketNumber);
        }

        [Fact]
        public async Task ReturnNull_WhenGetByIdWithNonExistentSession()
        {
            // Arrange - Use a non-existent session ID
            var nonExistentSessionId = new SessionId(Guid.NewGuid());

            // Act - Try to retrieve non-existent session
            var retrievedSession = await _repository.GetByIdAsync(nonExistentSessionId);

            // Assert - Should return null for non-existent session
            Assert.Null(retrievedSession);
        }

        [Fact]
        public async Task ReturnActiveSession_WhenGetActiveSessionByOperatorWithActiveSession()
        {
            // Arrange - Create session in working state (not closed)
            var session = OperatorSession.Create(
                _testSessionId,
                _testOperatorId,
                _testWorkplaceCode,
                _testAssignedServices);
            
            session.StartWork(); // Make it active
            await _repository.SaveAsync(session);

            // Act - Get active session for operator
            var activeSession = await _repository.GetActiveSessionByOperatorAsync(_testOperatorId);

            // Assert - Should return the active session
            Assert.NotNull(activeSession);
            Assert.Equal(_testSessionId, activeSession.SessionId);
            Assert.Equal(SessionStatus.ReadyToWork, activeSession.Status);
        }

        [Fact]
        public async Task ReturnNull_WhenGetActiveSessionByOperatorWithClosedSession()
        {
            // Arrange - Create and close a session
            var session = CreateCompleteTestSession();
            session.StartWork();
            session.CloseSession(); // Close the session
            await _repository.SaveAsync(session);

            // Act - Try to get active session for operator with closed session
            var activeSession = await _repository.GetActiveSessionByOperatorAsync(_testOperatorId);

            // Assert - Should return null since session is closed
            Assert.Null(activeSession);
        }

        [Fact]
        public async Task ReturnNull_WhenGetActiveSessionByOperatorWithNoSessions()
        {
            // Arrange - Use operator that has no sessions
            var operatorWithNoSessions = new OperatorId("OP999");

            // Act - Try to get active session for operator with no sessions
            var activeSession = await _repository.GetActiveSessionByOperatorAsync(operatorWithNoSessions);

            // Assert - Should return null when operator has no sessions
            Assert.Null(activeSession);
        }

        [Fact]
        public async Task UpdateExistingSession_WhenValidSessionProvided()
        {
            // Arrange - Save initial session
            var session = OperatorSession.Create(
                _testSessionId,
                _testOperatorId,
                _testWorkplaceCode,
                _testAssignedServices);
            
            await _repository.SaveAsync(session);

            // Modify session state
            session.StartWork();
            session.RequestClient();
            session.AssignClient(_testTicketNumber);

            // Act - Update the session
            await _repository.UpdateAsync(session);

            // Assert - Verify session was updated correctly
            var updatedSession = await _repository.GetByIdAsync(_testSessionId);
            
            Assert.NotNull(updatedSession);
            Assert.Equal(SessionStatus.WaitingClient, updatedSession.Status);
            Assert.NotNull(updatedSession.SessionStartTime);
            Assert.Single(updatedSession.ClientSessions);
            Assert.NotNull(updatedSession.CurrentClientSession);
            Assert.Equal(_testTicketNumber, updatedSession.CurrentClientSession.TicketNumber);
        }

        [Fact]
        public async Task ThrowException_WhenUpdatingNonExistentSession()
        {
            // Arrange - Create session that was never saved
            var nonExistentSession = OperatorSession.Create(
                new SessionId(Guid.NewGuid()),
                _testOperatorId,
                _testWorkplaceCode,
                _testAssignedServices);

            // Act & Assert - Should throw exception when updating non-existent session
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _repository.UpdateAsync(nonExistentSession));
            
            Assert.Contains("not found", exception.Message);
            Assert.Contains("Use SaveAsync for new sessions", exception.Message);
        }

        [Fact]
        public async Task UpdateClientSessions_WhenSessionContainsMultipleClientSessions()
        {
            // Arrange - Save session with one client
            var session = CreateCompleteTestSession();
            await _repository.SaveAsync(session);

            // Add more client sessions
            session.StartWork();
            session.RequestClient();
            session.AssignClient(_testTicketNumber);
            session.StartClientSession();
            session.CompleteClientSession();

            // Add second client
            session.RequestClient();
            session.AssignClient(new TicketNumber("T67890"));

            // Act - Update session with multiple client sessions
            await _repository.UpdateAsync(session);

            // Assert - Verify all client sessions were saved
            var updatedSession = await _repository.GetByIdAsync(_testSessionId);
            
            Assert.NotNull(updatedSession);
            Assert.Equal(2, updatedSession.ClientSessions.Count);
            Assert.Contains(updatedSession.ClientSessions, cs => cs.TicketNumber.Value == "T12345");
            Assert.Contains(updatedSession.ClientSessions, cs => cs.TicketNumber.Value == "T67890");
            
            // Verify current client session points to the second client
            Assert.NotNull(updatedSession.CurrentClientSession);
            Assert.Equal("T67890", updatedSession.CurrentClientSession.TicketNumber.Value);
        }

        [Fact]
        public async Task UpdateAssignedServices_WhenSessionServicesChange()
        {
            // Arrange - Save session with initial services
            var session = OperatorSession.Create(
                _testSessionId,
                _testOperatorId,
                _testWorkplaceCode,
                _testAssignedServices);
            
            await _repository.SaveAsync(session);

            // Create updated session with different services
            var newServices = new List<ServiceInfo>
            {
                new ServiceInfo("S003", "Driver License", "Driver license services"),
                new ServiceInfo("S004", "Registration", "Vehicle registration")
            };
            
            var updatedSession = OperatorSession.Create(
                _testSessionId,
                _testOperatorId,
                _testWorkplaceCode,
                newServices);

            // Act - Update session with new services
            await _repository.UpdateAsync(updatedSession);

            // Assert - Verify services were updated in database
            var savedServices = await _context.OperatorSessionServices
                .Where(s => s.OperatorSessionId == _testSessionId.Value)
                .ToListAsync();
            
            Assert.Equal(2, savedServices.Count);
            Assert.Contains(savedServices, s => s.ServiceCode == "S003");
            Assert.Contains(savedServices, s => s.ServiceCode == "S004");
            Assert.DoesNotContain(savedServices, s => s.ServiceCode == "S001");
            Assert.DoesNotContain(savedServices, s => s.ServiceCode == "S002");
        }

        [Fact]
        public async Task LoadCurrentClientSession_WhenSessionHasActiveClient()
        {
            // Arrange - Create session with active client
            var session = CreateCompleteTestSession();
            session.StartWork();
            session.RequestClient();
            session.AssignClient(_testTicketNumber);
            session.StartClientSession(); // Make client active
            
            await _repository.SaveAsync(session);

            // Act - Load session from database
            var loadedSession = await _repository.GetByIdAsync(_testSessionId);

            // Assert - Verify current client session is loaded correctly
            Assert.NotNull(loadedSession);
            Assert.NotNull(loadedSession.CurrentClientSession);
            Assert.Equal(_testTicketNumber, loadedSession.CurrentClientSession.TicketNumber);
            Assert.True(loadedSession.CurrentClientSession.IsActive);
        }

        [Fact]
        public async Task LoadNullCurrentClientSession_WhenSessionHasNoActiveClient()
        {
            // Arrange - Create session with completed client (no active client)
            var session = CreateCompleteTestSession();
            session.StartWork();
            session.RequestClient();
            session.AssignClient(_testTicketNumber);
            session.StartClientSession();
            session.CompleteClientSession(); // Complete the client session
            
            await _repository.SaveAsync(session);

            // Act - Load session from database
            var loadedSession = await _repository.GetByIdAsync(_testSessionId);

            // Assert - Verify current client session is null when no active client
            Assert.NotNull(loadedSession);
            Assert.Null(loadedSession.CurrentClientSession);
            Assert.Single(loadedSession.ClientSessions);
            Assert.True(loadedSession.ClientSessions.First().IsCompleted);
        }

        [Fact]
        public async Task PreserveSessionTimestamps_WhenSavingAndLoading()
        {
            // Arrange - Create session with timestamps
            var session = OperatorSession.Create(
                _testSessionId,
                _testOperatorId,
                _testWorkplaceCode,
                _testAssignedServices);
            
            session.StartWork();
            var startTime = session.SessionStartTime!.Value;
            
            session.CloseSession();
            var endTime = session.SessionEndTime!.Value;

            // Act - Save and reload session
            await _repository.SaveAsync(session);
            var loadedSession = await _repository.GetByIdAsync(_testSessionId);

            // Assert - Verify timestamps are preserved with correct precision
            Assert.NotNull(loadedSession);
            Assert.NotNull(loadedSession.SessionStartTime);
            Assert.NotNull(loadedSession.SessionEndTime);
            
            // PostgreSQL preserves microsecond precision
            Assert.Equal(startTime, loadedSession.SessionStartTime.Value, TimeSpan.FromMilliseconds(1));
            Assert.Equal(endTime, loadedSession.SessionEndTime.Value, TimeSpan.FromMilliseconds(1));
        }

        [Fact]
        public async Task HandleConcurrentAccess_WhenMultipleOperationsOnSameSession()
        {
            // Arrange - Save initial session
            var session = OperatorSession.Create(
                _testSessionId,
                _testOperatorId,
                _testWorkplaceCode,
                _testAssignedServices);
            
            await _repository.SaveAsync(session);

            // Act - Simulate concurrent access by loading session twice and modifying both
            var session1 = await _repository.GetByIdAsync(_testSessionId);
            var session2 = await _repository.GetByIdAsync(_testSessionId);
            
            Assert.NotNull(session1);
            Assert.NotNull(session2);
            
            // Modify first session
            session1.StartWork();
            await _repository.UpdateAsync(session1);
            
            // Modify second session (this should work since EF tracks changes)
            session2.StartWork();
            
            // Assert - Second update should not fail (EF handles this scenario)
            await _repository.UpdateAsync(session2);
            
            var finalSession = await _repository.GetByIdAsync(_testSessionId);
            Assert.NotNull(finalSession);
            Assert.Equal(SessionStatus.ReadyToWork, finalSession.Status);
        }

        // ================== HELPER METHODS ==================

        private OperatorSession CreateCompleteTestSession()
        {
            return OperatorSession.Create(
                _testSessionId,
                _testOperatorId,
                _testWorkplaceCode,
                _testAssignedServices);
        }

        [Fact]
        public async Task ThrowException_WhenSavingNullSession()
        {
            // Act & Assert - Should throw ArgumentNullException when saving null session
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _repository.SaveAsync(null!));
        }

        [Fact]
        public async Task ThrowException_WhenUpdatingNullSession()
        {
            // Act & Assert - Should throw ArgumentNullException when updating null session
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _repository.UpdateAsync(null!));
        }

        [Fact]
        public async Task HandleEmptyAssignedServices_WhenSessionHasNoServices()
        {
            // Arrange - Create session with empty services list
            var session = OperatorSession.Create(
                _testSessionId,
                _testOperatorId,
                _testWorkplaceCode,
                new List<ServiceInfo>());

            // Act - Save session with no assigned services
            await _repository.SaveAsync(session);

            // Assert - Verify session is saved correctly with empty services
            var loadedSession = await _repository.GetByIdAsync(_testSessionId);
            
            Assert.NotNull(loadedSession);
            Assert.Empty(loadedSession.AssignedServices);
            
            // Verify no service records in database
            var serviceCount = await _context.OperatorSessionServices
                .CountAsync(s => s.OperatorSessionId == _testSessionId.Value);
            
            Assert.Equal(0, serviceCount);
        }
        
        */
    }
}