namespace MockOperatorAuthorizationService.Models;

public class MockOperator
{
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
    public string FullName { get; set; } = "";
    public string AssignedWorkplace { get; set; } = "";
    public string WindowCode { get; set; } = "";
    public string WindowDisplayName { get; set; } = "";
    public string Location { get; set; } = "";
    public bool IsActive { get; set; } = true;
}

public class ActiveSession
{
    public string SessionId { get; set; } = "";
    public string OperatorLogin { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime LastActivity { get; set; }
}