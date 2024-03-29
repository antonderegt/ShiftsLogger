using ShiftsLoggerLibrary.Models;
using ShiftsLoggerLibrary.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ShiftsLoggerAPI;

public class ShiftController
{
    public static async Task<Results<Ok<Shift>, NotFound>> GetShiftByIdAsync(int id, IShiftService service)
    {
        Shift? targetShift = await service.GetShiftByIdAsync(id);

        return targetShift is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(targetShift);
    }

    public static async Task<List<Shift>> GetShiftsAsync(IShiftService service)
    {
        return await service.GetShiftsAsync();
    }

    public static async Task<List<Shift>> GetShiftsByEmployeeIdAsync(int id, IShiftService service)
    {
        return await service.GetShiftsByEmployeeIdAsync(id);
    }

    public static async Task<Results<Created<Shift>, BadRequest<string>>> StartShiftAsync(StartShift startShift, IShiftService service)
    {
        Shift? shift = await service.StartShiftAsync(startShift);

        if (shift == null)
        {
            return TypedResults.BadRequest("Employee already has a shift running.");
        }

        return TypedResults.Created("/shifts/{id}", shift);
    }

    public static async Task<Results<Created<Shift>, NotFound>> EndShiftAsync(EndShift endShift, IShiftService service)
    {
        Shift? shift = await service.EndShiftAsync(endShift);

        if (shift == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Created("/shifts/{id}", shift);
    }

    public static async Task<Results<NoContent, NotFound>> DeleteShiftAsync(int id, IShiftService service)
    {
        if (!await service.DeleteShiftByIdAsync(id))
        {
            return TypedResults.NotFound();
        }

        return TypedResults.NoContent();
    }
}