using Spectre.Console;
using static ShiftsLoggerUI.Enums;
using ShiftsLoggerLibrary.Models;


namespace ShiftsLoggerUI;

public class UserInterface
{
    private static int employeeId;
    public static async Task MainMenu()
    {
        AnsiConsole.Clear();
        employeeId = AnsiConsole.Ask<int>("What is your employee id?");
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
                StartShift();
                break;
            case MainMenuOption.EndShift:
                EndShift();
                break;
            case MainMenuOption.UpdateShift:
                UpdateShift();
                break;
            case MainMenuOption.DeleteShift:
                await DeleteShift();
                break;
            case MainMenuOption.ShowShifts:
                await ShowShifts();
                AnsiConsole.MarkupLine("Press enter to return to menu...");
                Console.ReadLine();
                break;
            case MainMenuOption.Quit:
                QuitApplication();
                break;
        }
    }

    private static void StartShift()
    {

    }

    private static void EndShift()
    {

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
            AnsiConsole.Markup("[red]No shifts yet![/] Press enter to return to menu...");
            Console.ReadLine();
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