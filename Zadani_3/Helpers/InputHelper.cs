using Spectre.Console;

namespace Zadani_3.Helpers;

/// <summary>
/// Pomocná třída pro načítání a kontrolu vstupů.
/// </summary>
public static class InputHelper
{
    /// <summary>
    /// Načte povinný textový údaj.
    /// </summary>
    public static string ReadRequiredText(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>(message)
                .Validate(value =>
                {
                    if (string.IsNullOrWhiteSpace(value))
                        return ValidationResult.Error("[red]Hodnota nesmí být prázdná.[/]");

                    return ValidationResult.Success();
                }));
    }

    /// <summary>
    /// Načte osobní číslo studenta.
    /// </summary>
    public static string ReadPersonalNumber(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>(message)
                .Validate(value =>
                {
                    if (string.IsNullOrWhiteSpace(value))
                        return ValidationResult.Error("[red]Osobní číslo nesmí být prázdné.[/]");

                    if (value.Length < 3)
                        return ValidationResult.Error("[red]Osobní číslo musí mít alespoň 3 znaky.[/]");

                    return ValidationResult.Success();
                }));
    }

    /// <summary>
    /// Načte známku v rozsahu 1 až 5.
    /// </summary>
    public static int ReadGrade(string message)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>(message)
                .Validate(value =>
                {
                    if (value < 1 || value > 5)
                        return ValidationResult.Error("[red]Známka musí být od 1 do 5.[/]");

                    return ValidationResult.Success();
                }));
    }
}