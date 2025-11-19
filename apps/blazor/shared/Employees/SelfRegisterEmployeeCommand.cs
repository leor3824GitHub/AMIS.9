namespace AMIS.Blazor.Shared.Employees;

public class SelfRegisterEmployeeCommand
{
    public string Name { get; set; } = string.Empty;
    public string? Designation { get; set; }
    public string? ResponsibilityCode { get; set; }
}
