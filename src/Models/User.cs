using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

public class User : IdentityUser
{
    // public int id {get;set;}
    // public string? name { get; set; }
    // public string? email {get; set;}
    // public string? password {get; set;}

    // app-specific additional user vars
    public DateTime? preferredLunchTime {get;set;}

    [ForeignKey("Location")]
    public int? fk_defaultPlaceToEat {get; set;}
    
    [ForeignKey("Location")]
    public int? fk_defaultPlaceToGetFood {get; set;}
}
