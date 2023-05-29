using System.ComponentModel.DataAnnotations.Schema;

public class LunchSession
{
    public int Id {get;set;}
    public DateTime day {get;set;}
    public DateTime lunchTime {get; set;}
    public bool participating {get; set;}
    
    [ForeignKey("Location")]
    public int? fk_location {get; set;}
    [ForeignKey("User")]
    public int? fk_user {get; set;}
}
