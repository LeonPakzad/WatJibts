
public class LunchTime
{
    public int id {get;set;}
    public DateTime day {get;set;}
    public bool participating {get; set;}
    public int? fk_location {get; set;}
    public int? fk_user {get; set;}
}
