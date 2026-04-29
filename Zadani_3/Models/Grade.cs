namespace Zadani_3.Models;

/// <summary>
/// Reprezentuje jednu známku z určitého předmětu.
/// </summary>
public class Grade
{
    /// <summary>
    /// Název předmětu.
    /// </summary>
    public string Subject { get; set; } = "";

    /// <summary>
    /// Hodnota známky od 1 do 5.
    /// </summary>
    public int Value { get; set; }
}