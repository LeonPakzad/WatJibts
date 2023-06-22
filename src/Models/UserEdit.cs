using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using src.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
public class UserEdit
{
    public User? User{get;set;}
    public List<SelectListItem> LocationsToEat { get; set; }
    public List<SelectListItem> LocationsToGetFood { get; set; }
}