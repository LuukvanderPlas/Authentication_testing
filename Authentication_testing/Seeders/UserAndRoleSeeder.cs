using Microsoft.AspNetCore.Identity;

namespace Authentication_testing.Seeders {
    public class UserAndRoleSeeder {

        public static async Task SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager) {
            string[] roles = { "Medewerker", "Manager", "Eigenaar" };

            foreach (var roleName in roles) {
                if (await roleManager.RoleExistsAsync(roleName) == false) {
                    IdentityRole role = new IdentityRole {
                        Name = roleName
                    };

                    await roleManager.CreateAsync(role);
                }
            }
        }

        public static async Task SeedUsers(UserManager<IdentityUser> userManager) {
            string[,] users = {
                { "Medewerker1@localhost", "Ab12345@", "Medewerker" },
                { "Manager1@localhost", "Ab12345@", "Manager" },
                { "OpperBaas@localhost", "Ab12345@", "Eigenaar" }
            };

            for (int i = 0; i < users.GetLength(0); i++) {
                var email = users[i, 0];
                var password = users[i, 1];
                var role = users[i, 2];

                if (await userManager.FindByEmailAsync(email) == null) {
                    IdentityUser identityUser = new IdentityUser {
                        UserName = email,
                        Email = email
                    };

                    IdentityResult result = await userManager.CreateAsync(identityUser, password);

                    if (result.Succeeded) {
                        await userManager.AddToRoleAsync(identityUser, role);
                    }
                }
            }
        }
    }
}
