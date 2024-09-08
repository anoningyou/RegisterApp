using System.Text.Json;
using API.Data.Interfaces;
using API.Entities.Accounting;
using API.Entities.Places;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Seed;

public class Seed
{
    /// <summary>
    /// Represents an asynchronous operation that can return a value.
    /// </summary>
    /// <typeparam name="TResult">The type of the result produced by the task.</typeparam>
    public static async Task SeedUsers(UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {
        if (await userManager.Users.AnyAsync())
            return;

        List<AppRole> roles =
        [
            new AppRole { Name = "Member" },
            new AppRole { Name = "Admin" },
            new AppRole { Name = "Moderator" }
        ];

        foreach (AppRole role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        AppUser admin = new()
        {
            UserName = "admin"
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, ["Admin","Moderator"]);
    }

    /// <summary>
    /// Seeds countries into the database.
    /// </summary>
    /// <param name="uow">The unit of work.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedPlaces(IUnitOfWork<DataContext> uow)
    {
        IRepository<Country, Guid> repository = uow.GetRepository<Country, Guid>();

        if (await (await repository.GetAllAsync<Country>()).AnyAsync()) 
            return;

        List<Country> countries = new(100);

        for (int i = 0; i < 100; i++)
        {
            Country country = new()
            {
                Id = Guid.NewGuid(),
                Name = $"Country {i + 1}",
                Provinces = new List<Province>(10)
            };

            for (int j = 0; j < 10; j++)
            {
                country.Provinces.Add(new Province
                {
                    Id = Guid.NewGuid(),
                    Name = $"Province {i + 1} {j + 1}",
                    CountryId = country.Id
                });
            }
            countries.Add(country);
        }

        await repository.AddRangeAsync(countries);

        await uow.Complete();
    }
}
