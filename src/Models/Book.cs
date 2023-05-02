using System.ComponentModel.DataAnnotations;

namespace src.Models;

public class Book
{
    public string Name { get; set; }
    public string Author { get; set; }
    public string ISBN {get;set;}
    public int? Erscheinungsjahr {get;set;}
    
    [Range(1, Int32.MaxValue)]
    public int? Auflage {get; set;}
}