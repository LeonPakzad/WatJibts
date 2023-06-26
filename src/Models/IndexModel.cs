using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using src.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class IndexModel
{
    public IEnumerable<LunchSession>? publicLunchSessions{get;set;}
    public IEnumerable<LunchSession>? privateLunchSessions{get;set;}
    public LunchSession? LunchSession {get;set;}
}