using Microsoft.AspNetCore.Identity;
using VideoClubWebApp.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        // Crear roles
        string[] roleNames = { "Admin", "Cajero" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Crear usuario administrador
        var adminUser = await userManager.FindByEmailAsync("admin@videoclub.com");
        if (adminUser == null)
        {
            var user = new IdentityUser
            {
                UserName = "admin@videoclub.com",
                Email = "admin@videoclub.com",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        // Crear usuario cajero
        var cashierUser = await userManager.FindByEmailAsync("cajero@videoclub.com");
        if (cashierUser == null)
        {
            var user = new IdentityUser
            {
                UserName = "cajero@videoclub.com",
                Email = "cajero@videoclub.com",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, "Cajero123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Cajero");
            }
        }
    }
}