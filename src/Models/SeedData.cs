using Microsoft.EntityFrameworkCore;

namespace src.Data;

using System;
using Microsoft.AspNetCore.Identity;
using src.Models;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] {"Admin", "User"};

        foreach (var role in roles)
        {
            if(!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        using (var context = new WatDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<WatDbContext>>()))
        {
            // Look for tables, if they are filled the db is seeded
            if (context.Location.Any())
            {
                return;
            }

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
