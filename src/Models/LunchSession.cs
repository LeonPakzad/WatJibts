using System.ComponentModel.DataAnnotations.Schema;

public class LunchSession
{
    public int Id {get;set;}
    public DateTime lunchTime {get; set;}
    public bool participating {get; set;}

    public bool isDefault {get; set;}
    public int weekday { get; set; }


    [ForeignKey("Location")]
    public int? fk_foodPlace {get; set;}

    [ForeignKey("Location")]
    public int? fk_eatingPlace {get; set;}

    [ForeignKey("User")]
    public string? fk_user {get; set;}

}

public enum weekday  
{  
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday 
}  
