using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

public class User : IdentityUser
{
    public DateTime? preferredLunchTime {get;set;}

    [ForeignKey("Location")]
    public int? fk_defaultPlaceToEat {get; set;}
    
    [ForeignKey("Location")]
    public int? fk_defaultPlaceToGetFood {get; set;}

    public static implicit operator EntityState(User v)
    {
        throw new NotImplementedException();
    }
}
