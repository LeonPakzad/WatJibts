using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using src.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class IndexModel
{
    public IEnumerable<LunchSession>? LunchSessions{get;set;}
    public LunchSession? LunchSession {get;set;}
}