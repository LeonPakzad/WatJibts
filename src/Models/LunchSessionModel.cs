using System.ComponentModel.DataAnnotations.Schema;

public class LunchSessionModel : LunchSession
{
    public string? userName {get; set;}
    public string? foodPlace {get; set;}
    public string? eatingPlace {get; set;}
}