using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using src.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
public class UserProfile
{
    public User? user {get;set;}
    public LunchSession? defaultLunchSession{get;set;}

    public IEnumerable<LunchSession>? userLunchSessions {get;set;}
    public IEnumerable<LunchSession>? defaultLunchSessions{get;set;}
}