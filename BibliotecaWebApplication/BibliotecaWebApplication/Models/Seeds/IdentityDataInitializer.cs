using Microsoft.AspNetCore.Identity;

public class IdentityDataInitializer
{
    public static async Task SeedData(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await EnsureRoleAsync(roleManager, "Root");
        await EnsureUserAsync(userManager, "superusuario@bibliotecasena.edu.co", "Bibli0t3c4s3n@", "Root");
    }

    private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    private static async Task EnsureUserAsync(UserManager<IdentityUser> userManager, string userName, string password, string roleName)
    {
        var user = await userManager.FindByEmailAsync(userName);
        if (user == null)
        {
            user = new IdentityUser { UserName = userName, Email = userName };
            var createUserResult = await userManager.CreateAsync(user, password);
            if (createUserResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}
