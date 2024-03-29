using ShiftsLoggerLibrary.Models;
using ShiftsLoggerLibrary.DTOs;

namespace ShiftsLoggerAPI;

public interface IShiftService
{
    Task<List<Shift>> GetShiftsAsync();
    Task<List<Shift>> GetShiftsByEmployeeIdAsync(int id);
    Task<Shift?> GetShiftByIdAsync(int id);
    Task<Shift?> StartShiftAsync(StartShift shift);
    Task<Shift?> EndShiftAsync(EndShift shift);
    Task<bool> DeleteShiftByIdAsync(int id);
}