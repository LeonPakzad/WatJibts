using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using src.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Collections;
using src.Data;
using src.Models;


public class HomeIndexModel
{
    public ICollection<LunchSessionModel>? publicLunchSessions{get;set;}
    public IEnumerable<LunchSessionModel>? privateLunchSessions{get;set;}
    public LunchSession? LunchSession {get;set;}

    /**
    / gets an icollection of publicLuchSessions
    / grouping all lunchSessions with same parameters
    / return new List with concat-names of those with equal lunchSession
    **/
    public List<LunchSessionModel> groupPublicLunchSessions(ICollection<LunchSessionModel> publicLunchSessions)
    {
        var lunchSessionGroups = publicLunchSessions.GroupBy(ls => new { ls.lunchTime, ls.fk_eatingPlace, ls.fk_foodPlace});
        List<LunchSessionModel> newLunchSession = new List<LunchSessionModel>();
        
        foreach(var group in lunchSessionGroups)
        {
            foreach (var ls in group)
            {
                if(group.FirstOrDefault().fk_user != ls.fk_user)
                {
                    group.FirstOrDefault().userName = String.Format("{0}, {1}", 
                        group.FirstOrDefault().userName,
                        ls.userName);
                }
            }

            newLunchSession.Add(group.FirstOrDefault());
        }
        return newLunchSession;
    }
}