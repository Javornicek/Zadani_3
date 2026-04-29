using Spectre.Console;
using System.IO;
using Zadani_3.Helpers;
using Zadani_3.Models;
using Zadani_3.Services;

School school = new School();
string inputFilePath = Path.Combine("Data", "students_input.json");
string outputFilePath = Path.Combine("Data", "students_output.json");
Directory.CreateDirectory("Data");
bool running = true;


while (running)
{
    AnsiConsole.Clear();

    AnsiConsole.Write(
        new FigletText("Studenti")
            .Centered()
            .Color(Color.Green));

    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Vyber akci:")
            .PageSize(10)
            .AddChoices(
                "Přidat studenta",
                "Smazat studenta",
                "Vypsat všechny studenty",
                "Vyhledat studenta",
                "Přidat známku",
                "Zobrazit průměr studenta",
                "Uložit studenty do souboru",
                "Načíst studenty ze souboru",
                "Ukončit program"));

    AnsiConsole.Clear();

    switch (choice)
    {
        case "Přidat studenta":
            AddStudent();
            break;

        case "Smazat studenta":
            RemoveStudent();
            break;

        case "Vypsat všechny studenty":
            WriteStudents(school.GetStudents());
            break;

        case "Vyhledat studenta":
            SearchStudent();
            break;

        case "Přidat známku":
            AddGrade();
            break;

        case "Zobrazit průměr studenta":
            ShowAverage();
            break;

        case "Uložit studenty do souboru":
            if (school.SaveToFile(outputFilePath))
                AnsiConsole.MarkupLine($"[green]Studenti byli uloženi do:[/] {Path.GetFullPath(outputFilePath)}");
            else
                AnsiConsole.MarkupLine("[red]Studenty se nepodařilo uložit.[/]");
            break;

        case "Načíst studenty ze souboru":
            if (school.LoadFromFile(inputFilePath))
                AnsiConsole.MarkupLine($"[green]Studenti byli načteni ze souboru:[/] {Path.GetFullPath(inputFilePath)}");
            else
                AnsiConsole.MarkupLine("[red]Soubor se nepodařilo načíst.[/]");
            break;

        case "Ukončit program":
            running = false;
            break;
    }

    if (running)
    {
        AnsiConsole.MarkupLine($"[grey]Vstupní soubor: {Path.GetFullPath(inputFilePath)}[/]");
        AnsiConsole.MarkupLine($"[grey]Výstupní soubor: {Path.GetFullPath(outputFilePath)}[/]");
        AnsiConsole.MarkupLine("[grey]Stiskni libovolnou klávesu pro pokračování...[/]");
        Console.ReadKey();
    }
}

void AddStudent()
{
    string firstName = InputHelper.ReadRequiredText("Zadej jméno:");
    string lastName = InputHelper.ReadRequiredText("Zadej příjmení:");
    string className = InputHelper.ReadRequiredText("Zadej třídu:");
    string personalNumber = InputHelper.ReadPersonalNumber("Zadej osobní číslo:");

    Student student = new Student
    {
        FirstName = firstName,
        LastName = lastName,
        ClassName = className,
        PersonalNumber = personalNumber
    };

    if (school.AddStudent(student))
        AnsiConsole.MarkupLine("[green]Student byl přidán.[/]");
    else
        AnsiConsole.MarkupLine("[red]Student s tímto osobním číslem už existuje.[/]");
}

void RemoveStudent()
{
    string personalNumber = InputHelper.ReadRequiredText("Zadej osobní číslo studenta:");

    if (school.RemoveStudent(personalNumber))
        AnsiConsole.MarkupLine("[green]Student byl smazán.[/]");
    else
        AnsiConsole.MarkupLine("[red]Student nebyl nalezen.[/]");
}

void SearchStudent()
{
    string searchTerm = InputHelper.ReadRequiredText("Zadej hledaný výraz:");
    List<Student> foundStudents = school.SearchStudent(searchTerm);

    WriteStudents(foundStudents);
}

void AddGrade()
{
    string personalNumber = InputHelper.ReadRequiredText("Zadej osobní číslo studenta:");
    Student? student = school.FindByPersonalNumber(personalNumber);

    if (student == null)
    {
        AnsiConsole.MarkupLine("[red]Student nebyl nalezen.[/]");
        return;
    }

    string subject = InputHelper.ReadRequiredText("Zadej předmět:");
    int value = InputHelper.ReadGrade("Zadej známku 1-5:");

    student.AddGrade(subject, value);

    AnsiConsole.MarkupLine("[green]Známka byla přidána.[/]");
}

void ShowAverage()
{
    string personalNumber = InputHelper.ReadRequiredText("Zadej osobní číslo studenta:");
    Student? student = school.FindByPersonalNumber(personalNumber);

    if (student == null)
    {
        AnsiConsole.MarkupLine("[red]Student nebyl nalezen.[/]");
        return;
    }

    if (student.Grades.Count == 0)
    {
        AnsiConsole.MarkupLine("[yellow]Student zatím nemá žádné známky.[/]");
        return;
    }

    AnsiConsole.MarkupLine(
        $"Průměr studenta [green]{student.FirstName} {student.LastName}[/] je [yellow]{student.GetAverage():0.00}[/].");
}

void WriteStudents(List<Student> students)
{
    if (students.Count == 0)
    {
        AnsiConsole.MarkupLine("[yellow]Žádní studenti nebyli nalezeni.[/]");
        return;
    }

    Table table = new Table();
    table.Border = TableBorder.Rounded;

    table.AddColumn("Osobní číslo");
    table.AddColumn("Jméno");
    table.AddColumn("Příjmení");
    table.AddColumn("Třída");
    table.AddColumn("Počet známek");
    table.AddColumn("Průměr");

    foreach (Student student in students)
    {
        string average = student.Grades.Count == 0
            ? "-"
            : student.GetAverage().ToString("0.00");

        table.AddRow(
            student.PersonalNumber,
            student.FirstName,
            student.LastName,
            student.ClassName,
            student.Grades.Count.ToString(),
            average);
    }

    AnsiConsole.Write(table);
}