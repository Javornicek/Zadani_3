using System.Text.Json;
using Zadani_3.Models;

namespace Zadani_3.Services;

/// <summary>
/// Spravuje studenty a ukládání dat do souboru.
/// </summary>
public class School
{
    private List<Student> students = new();

    /// <summary>
    /// Přidá studenta, pokud už neexistuje stejné osobní číslo.
    /// </summary>
    public bool AddStudent(Student student)
    {
        bool exists = students.Any(s => s.PersonalNumber == student.PersonalNumber);

        if (exists)
            return false;

        students.Add(student);
        return true;
    }

    /// <summary>
    /// Smaže studenta podle osobního čísla.
    /// </summary>
    public bool RemoveStudent(string personalNumber)
    {
        Student? student = FindByPersonalNumber(personalNumber);

        if (student == null)
            return false;

        students.Remove(student);
        return true;
    }

    /// <summary>
    /// Vrátí všechny studenty.
    /// </summary>
    public List<Student> GetStudents()
    {
        return students;
    }

    /// <summary>
    /// Vyhledá studenty podle jména, příjmení nebo třídy.
    /// </summary>
    public List<Student> SearchStudent(string searchTerm)
    {
        string text = searchTerm.ToLower();

        return students
            .Where(s =>
                s.FirstName.ToLower().Contains(text) ||
                s.LastName.ToLower().Contains(text) ||
                s.ClassName.ToLower().Contains(text))
            .ToList();
    }

    /// <summary>
    /// Najde studenta podle osobního čísla.
    /// </summary>
    public Student? FindByPersonalNumber(string personalNumber)
    {
        return students.FirstOrDefault(s => s.PersonalNumber == personalNumber);
    }

    /// <summary>
    /// Uloží seznam studentů do JSON souboru.
    /// </summary>
    public bool SaveToFile(string filePath)
    {
        try
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(students, options);
            File.WriteAllText(filePath, json);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Chyba při ukládání: " + ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Načte studenty z JSON souboru.
    /// </summary>
    public bool LoadFromFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Soubor nebyl nalezen: " + Path.GetFullPath(filePath));
                return false;
            }

            string json = File.ReadAllText(filePath);

            List<Student>? loadedStudents = JsonSerializer.Deserialize<List<Student>>(json);

            if (loadedStudents == null)
            {
                Console.WriteLine("Soubor je prázdný nebo má špatný formát.");
                return false;
            }

            students = loadedStudents;
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Chyba při načítání: " + ex.Message);
            return false;
        }
    }
}