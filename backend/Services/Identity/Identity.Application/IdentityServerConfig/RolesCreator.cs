using Microsoft.AspNetCore.Identity;

namespace Identity.Application.IdentityServerConfig
{
    public class RolesCreator
    {
        public static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("Visitor") is null)
            {
                await roleManager.CreateAsync(new IdentityRole("Visitor"));
            }

            if (await roleManager.FindByNameAsync("Admin") is null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
        }
    }
}
