namespace src.Models;

using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new WatDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<WatDbContext>>()))
        {
            // Look for any users.
            if (context.User.Any())
            {
                return; // DB has been seeded
            }
            context.User.AddRange(
                new User
                {
                    Name = "admin",
                    Email = "pakzadleon@gmail.com",
                    Password = "12345678",
                }
            );

            context.SaveChanges();
        }
    }
}
