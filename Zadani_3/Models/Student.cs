namespace Zadani_3.Models;

/// <summary>
/// Uchovává údaje o studentovi a jeho známkách.
/// </summary>
public class Student
{
    /// <summary>
    /// Jméno studenta.
    /// </summary>
    public string FirstName { get; set; } = "";

    /// <summary>
    /// Příjmení studenta.
    /// </summary>
    public string LastName { get; set; } = "";

    /// <summary>
    /// Třída studenta.
    /// </summary>
    public string ClassName { get; set; } = "";

    /// <summary>
    /// Jedinečné osobní číslo studenta.
    /// </summary>
    public string PersonalNumber { get; set; } = "";

    /// <summary>
    /// Seznam známek studenta.
    /// </summary>
    public List<Grade> Grades { get; set; } = new();

    /// <summary>
    /// Přidá studentovi novou známku.
    /// </summary>
    public void AddGrade(string subject, int value)
    {
        Grades.Add(new Grade
        {
            Subject = subject,
            Value = value
        });
    }

    /// <summary>
    /// Vypočítá průměr známek studenta.
    /// </summary>
    public double GetAverage()
    {
        if (Grades.Count == 0)
            return 0;

        return Grades.Average(grade => grade.Value);
    }
}