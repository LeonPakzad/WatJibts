using Microsoft.EntityFrameworkCore;

namespace src.Data;

using System;
using src.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new WatDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<WatDbContext>>()))
        {
            // Look for tables, if they are filled the db is seeded
            if (context.Location.Any())
            {
                return;
            }
            
            // context.User.AddRange(
            //     new User
            //     {
            //         name = "admin",
            //         email = "pakzadleon@gmail.com",
            //         password = "12345678",
            //         preferredLunchTime = DateTime.Now
            //     }
            // );

            context.Location.AddRange(
                new Location
                {
                    name = "Marks Supermarkt",
                    description = "Der Perfekte Ort um Speisen zu besorgen",
                    isPlaceToEat = false,
                    isPlaceToGetFood = true,
                },
                new Location
                {
                    name = "Stammtisch",
                    description = "Der Perfekte Ort um seine Mahlzeiten zu verzehren",
                    isPlaceToEat = true,
                    isPlaceToGetFood = false,
                },
                new Location
                {
                    name = "Dimars Döner",
                    description = "Ein schönes Lokal in der Nähe zum Essen holen sowie verspeisen",
                    isPlaceToEat = true,
                    isPlaceToGetFood = true,
                }
            );

            context.SaveChanges();
        }
    }
}
