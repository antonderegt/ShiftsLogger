using Spectre.Console;
using ShiftsLoggerLibrary;
using static ShiftsLoggerUI.Enums;


namespace ShiftsLoggerUI;

public class UserInterface
{
    private static int employeeId;
    public static void MainMenu()
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
                DeleteShift();
                break;
            case MainMenuOption.ShowShifts:
                ShowShifts();
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

    private static void DeleteShift()
    {

    }

    private static void ShowShifts()
    {

    }

    private static void QuitApplication()
    {
        Environment.Exit(0);
    }
}