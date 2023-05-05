public class User
{
    public int id {get;set;}
    public string? name { get; set; }
    public string? email {get; set;}
    public string? password {get; set;}
    public DateTime? preferredLunchTime {get;set;}
    public int? fk_defaultPlaceToEat {get; set;}
    public int? fk_defaultPlaceToGetFood {get; set;}

}
