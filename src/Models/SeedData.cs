using Microsoft.EntityFrameworkCore;

namespace src.Data;

using System;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new WatDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<WatDbContext>>()))
        {
            // Look for tables, if they are filled the db is seeded
            if (context.User.Any())
            {
                return;
            }
            
            context.User.AddRange(
                new User
                {
                    name = "admin",
                    email = "pakzadleon@gmail.com",
                    password = "12345678",
                    preferredLunchTime = DateTime.Now
                }
            );

            context.Location.AddRange(
                new Location
                {
                    name = "GetFood",
                    description = "Place to get Food from",
                    isPlaceToEat = false,
                    isPlaceToGetFood = true,
                },
                new Location
                {
                    name = "Dinner Table",
                    description = "Place to eat",
                    isPlaceToEat = true,
                    isPlaceToGetFood = false,
                }
            );

            context.SaveChanges();
        }
    }
}
