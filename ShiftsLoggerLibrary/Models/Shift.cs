namespace ShiftsLoggerLibrary.Models;

public class Shift
{
    public int Id { get; set; }
    public DateTime? StartTime { get; set; } = null;
    public DateTime? EndTime { get; set; } = null;
    public int EmployeeId { get; set; }
}