using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using src.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

public class IndexModel
{
    public ICollection<LunchSession>? publicLunchSessions{get;set;}
    public IEnumerable<LunchSession>? privateLunchSessions{get;set;}
    public LunchSession? LunchSession {get;set;}

    // gets an icollection of publicsessions and returns a icollection in which lunchsessions with same parameters are 
    public ICollection<LunchSession> groupPublicLunchSessions(ICollection<LunchSession> publicLunchSessions)
    {
        List<int> done = new List<int>();

        for (int index = 0; index < publicLunchSessions.Count(); index++)
        {
            if(done.Contains(index))
            {
                continue;
            }
            for (int indexTwo = index+1; indexTwo < publicLunchSessions.Count(); indexTwo++)
            {
                if ( 
                    publicLunchSessions.ElementAt(index).lunchTime         == publicLunchSessions.ElementAt(indexTwo).lunchTime  
                    && publicLunchSessions.ElementAt(index).fk_eatingPlace == publicLunchSessions.ElementAt(indexTwo).fk_eatingPlace
                    && publicLunchSessions.ElementAt(index).fk_foodPlace   == publicLunchSessions.ElementAt(indexTwo).fk_foodPlace )
                {
                    publicLunchSessions.ElementAt(index).fk_user =
                        String.Format("{0}+{1}", 
                        publicLunchSessions.ElementAt(index).fk_user, 
                        // publicLunchSessions.ElementAt(indexTwo).fk_user),
                        String.Format("{0}+{1}|", 
                        index.ToString(),
                        indexTwo.ToString()));

                    publicLunchSessions.Remove(publicLunchSessions.ElementAt(indexTwo));
                    done.Add(index);
                }
            }
        }
        return publicLunchSessions;
    }
}