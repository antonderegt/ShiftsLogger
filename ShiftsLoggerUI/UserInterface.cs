using Spectre.Console;
using static ShiftsLoggerUI.Enums;
using ShiftsLoggerLibrary.Models;
using ShiftsLoggerLibrary.DTOs;


namespace ShiftsLoggerUI;

public class UserInterface
{
    private static int employeeId;
    public static async Task MainMenu()
    {
        AnsiConsole.Clear();
        employeeId = AnsiConsole.Ask<int>("What is your employee id?");

        while (true)
        {
            AnsiConsole.Clear();
            MainMenuOption option = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOption>()
                    .Title("What do you want to [blue]do[/]?")
                    .AddChoices(
                        MainMenuOption.StartShift,
                        MainMenuOption.EndShift,
                        MainMenuOption.UpdateShift,
                        MainMenuOption.DeleteShift,
                        MainMenuOption.ShowShifts,
                        MainMenuOption.Quit
                    ));

            switch (option)
            {
                case MainMenuOption.StartShift:
                    await StartShift();
                    break;
                case MainMenuOption.EndShift:
                    await EndShift();
                    break;
                case MainMenuOption.UpdateShift:
                    UpdateShift();
                    break;
                case MainMenuOption.DeleteShift:
                    await DeleteShift();
                    break;
                case MainMenuOption.ShowShifts:
                    await ShowShifts();
                    AnsiConsole.Markup("Press enter to return to menu...");
                    Console.ReadLine();
                    break;
                case MainMenuOption.Quit:
                    QuitApplication();
                    break;
            }
        }
    }

    private static async Task StartShift()
    {
        DataAccess dataAccess = new();
        Shift? shift = await dataAccess.GetRunningShiftAsync(employeeId);
        if (shift != null)
        {
            AnsiConsole.MarkupLine("[red]You are already on shift.[/] Press enter to return to menu...");
            Console.ReadLine();
            return;
        }

        string start = AnsiConsole.Ask<string>("Start time (yyyy-mm-dd hh:mm)?");
        DateTime startTime;
        while (!DateTime.TryParse(start, out startTime))
        {
            AnsiConsole.MarkupLine("[red]Error parsing date, try again...[/]");
            start = AnsiConsole.Ask<string>("Start time (yyyy-mm-dd hh:mm)?");
        }

        StartShift startShift = new() { StartTime = startTime, EmployeeId = employeeId };
        shift = await dataAccess.StartShiftAsync(startShift);

        if (shift == null)
        {
            AnsiConsole.MarkupLine("[red]Unable to start a shift.[/] Press enter to return to menu...");
            Console.ReadLine();
            return;
        }
        else
        {
            if (shift != null)
            {
                AnsiConsole.MarkupLine($"[green]Shift started at {shift.StartTime}.[/] Press enter to return to menu...");
                Console.ReadLine();
            }
        }
    }

    private static async Task EndShift()
    {
        DataAccess dataAccess = new();
        Shift? shift = await dataAccess.GetRunningShiftAsync(employeeId);
        if (shift == null)
        {
            AnsiConsole.MarkupLine("[red]You are not on a shift.[/] Press enter to return to menu...");
            Console.ReadLine();
            return;
        }

        string end = AnsiConsole.Ask<string>("End time (yyyy-mm-dd hh:mm)?");
        DateTime endTime;
        while (!DateTime.TryParse(end, out endTime))
        {
            AnsiConsole.MarkupLine("[red]Error parsing date, try again...[/]");
            end = AnsiConsole.Ask<string>("End time (yyyy-mm-dd hh:mm)?");
        }

        EndShift endShift = new() { EndTime = endTime, Id = shift.Id };
        shift = await dataAccess.EndShiftAsync(endShift);

        if (shift == null)
        {
            AnsiConsole.MarkupLine("[red]Unable to end shift.[/] Press enter to return to menu...");
            Console.ReadLine();
            return;
        }
        else
        {
            if (shift != null)
            {
                AnsiConsole.MarkupLine($"[green]Shift ended at {shift.EndTime}.[/] Press enter to return to menu...");
                Console.ReadLine();
            }
        }
    }

    private static void UpdateShift()
    {

    }

    private static async Task DeleteShift()
    {
        DataAccess dataAccess = new();
        IEnumerable<Shift>? shifts = await dataAccess.GetShiftsAsync(employeeId);

        if (shifts == null || !shifts.Any())
        {
            AnsiConsole.Markup("[red]No shifts yet![/] Press enter to return to menu...");
            Console.ReadLine();
            return;
        }

        await ShowShifts();

        int shiftId = AnsiConsole.Ask<int>("Which shift do you want to [blue]delete[/]?");
        if (!AnsiConsole.Confirm($"Are you sure you want to delete shift {shiftId}?"))
        {
            return;
        }

        bool result = await dataAccess.DeleteShiftAsync(shiftId);

        if (result)
        {
            AnsiConsole.MarkupLine($"[green]Shift {shiftId} deleted.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Unable to delete shift {shiftId}.[/]");
        }
    }

    private static async Task ShowShifts(IEnumerable<Shift>? shifts = null)
    {
        if (shifts == null)
        {
            DataAccess dataAccess = new();
            shifts = await dataAccess.GetShiftsAsync(employeeId);
        }

        if (shifts == null || !shifts.Any())
        {
            AnsiConsole.Markup("[red]No shifts yet![/] ");
            return;
        }

        Table table = new()
        {
            Title = new TableTitle("My shifts")
        };

        table.AddColumn("Id");
        table.AddColumn("Start time");
        table.AddColumn("End time");
        table.AddColumn("Employee id");

        foreach (Shift shift in shifts)
        {
            table.AddRow(shift.Id.ToString(), shift.StartTime.ToString() ?? "-", shift.EndTime.ToString() ?? "-", shift.EmployeeId.ToString());
        }

        AnsiConsole.Write(table);
    }

    private static void QuitApplication()
    {
        Environment.Exit(0);
    }
}